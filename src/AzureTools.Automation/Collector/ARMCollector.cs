// ARMCollector.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Automation.Collector
{
    using AzureTools.Authentication.Settings;
    using AzureTools.Automation.Messaging;
    using AzureTools.Client;
    using AzureTools.Repository;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class ARMCollector(
        ILogger<ARMCollector> logger,
        IARMClient armClient,
        IObjectRepository objectRepository,
        IOptionsMonitor<AuthenticationSettings> authSettingsOption,
        IMessageFactory messageFactory)
    {
        private const string ArmAuthSettingsKey = "armAuthenticationSettings";
        private readonly ILogger<ARMCollector> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IARMClient _armClient = armClient ?? throw new ArgumentNullException(nameof(armClient));
        private readonly IObjectRepository _objectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
        private readonly AuthenticationSettings _authSettings = authSettingsOption?.Get(ArmAuthSettingsKey) ?? throw new ArgumentNullException(nameof(authSettingsOption));
        private readonly IMessageFactory _messageFactory = messageFactory ?? throw new ArgumentNullException(nameof(messageFactory));

        /// <summary>
        /// Retrieves a list of tenants from the Azure Management API using the provided authentication settings and execution ID.
        /// </summary>
        /// <param name="executionId">The unique Id for the request.</param>
        /// <param name="stopToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{ARMResponse}"/> representing the result of the request.</returns>
        public async Task CollectTenantsAsync(string? executionId = default, CancellationToken stopToken = default)
        {

            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            _logger.LogInformation("Getting tenants from the ARM API with execution ID: {ExecutionId} with endpoint {e}", executionId, ARMEndpoints.ListTenantsEndpoint);

            var response = await _armClient.GetTenantsAsync(_authSettings, executionId, stopToken);

            if (response == null || response.Value == null || !response.Value.Any())
            {
                _logger.LogWarning("No tenants found for execution ID: {ExecutionId}", executionId);
                return;
            }

            _logger.LogInformation("Found {Count} tenants for execution ID: {ExecutionId}", response.Value.Count, executionId);

            await _objectRepository.WriteAsync(response.Value);
        }

        /// <summary>
        /// Retrieves a list of subscriptions from the Azure Management API using the provided authentication settings and execution ID.
        /// </summary>
        /// <param name="executionId">The unique Id for the request.</param>
        /// <param name="stopToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{ARMResponse}"/> representing the result of the request.</returns>
        public async Task CollectSubscriptionsAsync(string? executionId = default, CancellationToken stopToken = default)
        {

            if (string.IsNullOrWhiteSpace(executionId))
            {
                executionId = Guid.NewGuid().ToString();
            }

            _logger.LogInformation("Getting tenants from the ARM API with execution ID: {ExecutionId} with endpoint {e}", executionId, ARMEndpoints.ListTenantsEndpoint);

            var response = await _armClient.GetSubscriptionsAsync(_authSettings, executionId, stopToken);

            if (response == null || response.Value == null || !response.Value.Any())
            {
                _logger.LogWarning("No subscriptions found for execution ID: {ExecutionId}", executionId);
                return;
            }

            foreach (var sub in response.Value)
            {
                var subEnumerationRequest = new SubscriptionMessage
                {
                    AuthSettingsKey = _authSettings.GetAuthKey(),
                    TenantId = sub.TenantId,
                    ExecutionId = executionId,
                    SubscriptionId = sub.SubscriptionId,
                };

                await _messageFactory.SendMessageAsync(MessageTopics.SubscriptionEnumerationTopic, "Subscription", JsonUtil.Serialize(subEnumerationRequest));
            }

            _logger.LogInformation("Found {Count} subscriptions for execution ID: {ExecutionId}", response.Value.Count, executionId);

            await _objectRepository.WriteAsync(response.Value);
        }


        public async Task CollectResourcesAsync(SubscriptionMessage message, CancellationToken stopToken)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (string.IsNullOrWhiteSpace(message.SubscriptionId))
            {
                throw new ArgumentException("SubscriptionId cannot be null or empty.", nameof(message.SubscriptionId));
            }

            _logger.LogInformation("Getting tenants from the ARM API with execution ID: {ExecutionId} with endpoint {e}", message.ExecutionId, ARMEndpoints.ListTenantsEndpoint);

            var response = await _armClient.GetResourcesForSubscriptionAsync(message.AuthSettingsKey, message.SubscriptionId, message.SubscriptionId, stopToken);

            if (response == null || response.Value == null || !response.Value.Any())
            {
                _logger.LogWarning("No resources found for execution ID: {ExecutionId}", message.ExecutionId);
                return;
            }

            foreach (var resource in response.Value)
            {
                var resourceEnumerationRequest = new ResourcePropertyMessage
                {
                    AuthSettingsKey = _authSettings.GetAuthKey(),
                    TenantId = resource.TenantId,
                    ExecutionId = message.ExecutionId,
                    ResourceId = resource.Id,
                };

                _logger.LogInformation("Collecting information about resource: {resourceId}", resource.Id);

                await _messageFactory.SendMessageAsync(MessageTopics.ResourcePropertiesTopic, "Resource", JsonUtil.Serialize(resourceEnumerationRequest));
            }

            _logger.LogInformation("Found {Count} resources for execution ID: {ExecutionId}", response.Value.Count, message.ExecutionId);
        }

        public async Task GetResourcePropertiesForResourceAsync(ResourcePropertyMessage message, CancellationToken stopToken)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (string.IsNullOrWhiteSpace(message.ResourceId))
            {
                throw new ArgumentException("ResourceId cannot be null or empty.", nameof(message.ResourceId));
            }
            var response = await _armClient.GetResourcePropertiesAsync(
                _authSettings.GetAuthKey(),
                message.ResourceId,
                message.TenantId,
                message.ExecutionId,
                stopToken);

            if (response == null || response.Value == null)
            {
                _logger.LogWarning("No resource properties found for ResourceId: {ResourceId} in TenantId: {TenantId}", message.ResourceId, message.TenantId);
                return;
            }

            _logger.LogInformation("Found properties for resource: {ResourceId} in TenantId: {TenantId}", message.ResourceId, message.TenantId);

            await _objectRepository.WriteAsync(response.Value);
        }
    }
}
