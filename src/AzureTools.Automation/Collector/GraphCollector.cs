// GraphCollector.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Automation.Collector
{
    using AzureTools.Authentication.Settings;
    using AzureTools.Automation.Messaging;
    using AzureTools.Client;
    using AzureTools.Client.Model;
    using AzureTools.Repository;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public sealed class GraphCollector
    {
        private readonly IGraphClient _graphClient;
        private readonly IObjectRepository _objectRepository;
        private readonly ILogger<GraphCollector> _logger;
        private readonly IMessageFactory _messageFactory;
        // For this iteration, support only one authentication settings key. 
        private readonly AuthenticationSettings _authSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphCollector"/> class.
        /// </summary>
        /// <param name="graphClient">The client to interact with the graph api.</param>
        /// <param name="objectRepository">How data is being written out.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="authSettingsOption">The authenticationsettings to use to auth to the graph api.</param>
        /// <exception cref="ArgumentNullException">If a required argument is null.</exception>
        public GraphCollector(
            IGraphClient graphClient,
            IObjectRepository objectRepository,
            ILogger<GraphCollector> logger,
            IOptions<AuthenticationSettings> authSettingsOption,
            IMessageFactory messageFactory)
        {
            _graphClient = graphClient ?? throw new ArgumentNullException(nameof(graphClient));
            _objectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _authSettings = authSettingsOption?.Value ?? throw new ArgumentNullException(nameof(authSettingsOption));
            _messageFactory = messageFactory ?? throw new ArgumentNullException(nameof(messageFactory));
        }

        public async Task CollectUsersAsync(string executionId, CancellationToken stopToken = default)
        {
            var tenantId = _authSettings.TenantId;

            _logger.LogInformation("Collecting users for tenant {TenantId} with execution ID {ExecutionId}", _authSettings.TenantId, executionId);

            var usersResponse = await _graphClient.GetUsersAsync(_authSettings, executionId, stopToken);

            if (usersResponse?.Value == null)
            {
                _logger.LogWarning("No users found for tenant {TenantId}", tenantId);
                return;
            }

            await _objectRepository.WriteAsync(usersResponse.Value);
            _logger.LogInformation("Successfully collected {Count} users for tenant {TenantId}", usersResponse.Value.Count(), tenantId);

            if (string.IsNullOrWhiteSpace(usersResponse?.ODataNextLink) is false)
            {
                var nextRequest = new EnumerationRequest
                {
                    Url = usersResponse.ODataNextLink,
                    SettingKey = _authSettings.GetAuthKey(),
                    TenantId = tenantId,
                    executionId = executionId,
                    ObjectType = typeof(User)
                };

                await _messageFactory.SendMessageAsync("ObjectEnumeration", "request", JsonUtil.Serialize(nextRequest));
            }
            else
            {
                _logger.LogInformation("No more pages of users to collect for tenant {TenantId}", tenantId);
                return;
            }
        }

        public async Task CollectGroupsAsync(string executionId, CancellationToken stopToken = default)
        {
            var tenantId = _authSettings.TenantId;

            _logger.LogInformation("Collecting groups for tenant {TenantId} with execution ID {ExecutionId}", _authSettings.TenantId, executionId);

            var groupsResponse = await _graphClient.GetGroupsAsync(_authSettings, executionId, stopToken);

            if (groupsResponse?.Value == null)
            {
                _logger.LogWarning("No groups found for tenant {TenantId}", tenantId);
                return;
            }

            // Write group Ids to the queue for group memebership processing.
            foreach (var group in groupsResponse.Value)
            {
                var groupMembershipRequest = new EnumerationRequest
                {
                    Url = string.Empty, // This will be filled in later.
                    SettingKey = _authSettings.GetAuthKey(),
                    TenantId = tenantId,
                    executionId = executionId,
                    ObjectType = typeof(GroupMember)
                };
                // Create a message for each group to process its members.
                await _messageFactory.SendMessageAsync("GroupMemberEnumeration", "id", group.Id);
            }

            await _objectRepository.WriteAsync(groupsResponse.Value);
            _logger.LogInformation("Successfully collected {Count} groups for tenant {TenantId}", groupsResponse.Value.Count(), tenantId);

            if (string.IsNullOrWhiteSpace(groupsResponse?.ODataNextLink) is false)
            {
                var nextRequest = new EnumerationRequest
                {
                    Url = groupsResponse.ODataNextLink,
                    SettingKey = _authSettings.GetAuthKey(),
                    TenantId = tenantId,
                    executionId = executionId,
                    ObjectType = typeof(Group)
                };

                await _messageFactory.SendMessageAsync("ObjectEnumeration", "request", JsonUtil.Serialize(nextRequest));
            }
            else
            {
                _logger.LogInformation("No more pages of groups to collect for tenant {TenantId}", tenantId);
                return;
            }
        }

        public async Task CollectServicePrincipalsAsync(string executionId, CancellationToken stopToken = default)
        {
            var tenantId = _authSettings.TenantId;

            _logger.LogInformation("Collecting serviceprincipals for tenant {TenantId} with execution ID {ExecutionId}", _authSettings.TenantId, executionId);

            var serviceprincipalresponse = await _graphClient.GetServicePrincipalsAsync(_authSettings, executionId, stopToken);

            if (serviceprincipalresponse?.Value == null)
            {
                _logger.LogWarning("No serviceprincipals found for tenant {TenantId}", tenantId);
                return;
            }

            await _objectRepository.WriteAsync(serviceprincipalresponse.Value);
            _logger.LogInformation("Successfully collected {Count} serviceprincipals for tenant {TenantId}", serviceprincipalresponse.Value.Count(), tenantId);

            if (string.IsNullOrWhiteSpace(serviceprincipalresponse?.ODataNextLink) is false)
            {
                var nextRequest = new EnumerationRequest
                {
                    Url = serviceprincipalresponse.ODataNextLink,
                    SettingKey = _authSettings.GetAuthKey(),
                    TenantId = tenantId,
                    executionId = executionId,
                    ObjectType = typeof(ServicePrincipal)
                };

                await _messageFactory.SendMessageAsync("ObjectEnumeration", "request", JsonUtil.Serialize(nextRequest));
            }
            else
            {
                _logger.LogInformation("No more pages of serviceprincipals to collect for tenant {TenantId}", tenantId);
                return;
            }
        }

        public async Task CollectAppRegistrationsAsync(string executionId, CancellationToken stopToken = default)
        {
            var tenantId = _authSettings.TenantId;

            _logger.LogInformation("Collecting ApplicationRegistration for tenant {TenantId} with execution ID {ExecutionId}", _authSettings.TenantId, executionId);

            var appRegistrationResponse = await _graphClient.GetApplicationRegistrationsAsync(_authSettings, executionId, stopToken);

            if (appRegistrationResponse?.Value == null)
            {
                _logger.LogWarning("No ApplicationRegistration found for tenant {TenantId}", tenantId);
                return;
            }

            await _objectRepository.WriteAsync(appRegistrationResponse.Value);
            _logger.LogInformation("Successfully collected {Count} ApplicationRegistration for tenant {TenantId}", appRegistrationResponse.Value.Count(), tenantId);

            if (string.IsNullOrWhiteSpace(appRegistrationResponse?.ODataNextLink) is false)
            {
                var nextRequest = new EnumerationRequest
                {
                    Url = appRegistrationResponse.ODataNextLink,
                    SettingKey = _authSettings.GetAuthKey(),
                    TenantId = tenantId,
                    executionId = executionId,
                    ObjectType = typeof(ApplicationRegistration)
                };

                await _messageFactory.SendMessageAsync("ObjectEnumeration", "request", JsonUtil.Serialize(nextRequest));
            }
            else
            {
                _logger.LogInformation("No more pages of ApplicationRegistration to collect for tenant {TenantId}", tenantId);
                return;
            }
        }

        public async Task CollectDirectoryRolesAsync(string executionId, CancellationToken stopToken = default)
        {
            var tenantId = _authSettings.TenantId;

            _logger.LogInformation("Collecting DirectoryRole for tenant {TenantId} with execution ID {ExecutionId}", _authSettings.TenantId, executionId);

            var directoryRoleResponse = await _graphClient.GetEntraDirectoryRoles(_authSettings, executionId, stopToken);

            if (directoryRoleResponse?.Value == null)
            {
                _logger.LogWarning("No DirectoryRole found for tenant {TenantId}", tenantId);
                return;
            }

            await _objectRepository.WriteAsync(directoryRoleResponse.Value);
            _logger.LogInformation("Successfully collected {Count} DirectoryRole for tenant {TenantId}", directoryRoleResponse.Value.Count(), tenantId);

            if (string.IsNullOrWhiteSpace(directoryRoleResponse?.ODataNextLink) is false)
            {
                var nextRequest = new EnumerationRequest
                {
                    Url = directoryRoleResponse.ODataNextLink,
                    SettingKey = _authSettings.GetAuthKey(),
                    TenantId = tenantId,
                    executionId = executionId,
                    ObjectType = typeof(DirectoryRole)
                };

                await _messageFactory.SendMessageAsync("ObjectEnumeration", "request", JsonUtil.Serialize(nextRequest)  );
            }
            else
            {
                _logger.LogInformation("No more pages of DirectoryRole to collect for tenant {TenantId}", tenantId);
                return;
            }
        }

        public async Task CollectNextObjectAsync<T>(EnumerationRequest enumerationRequest, CancellationToken stopToken)
            where T : ModelBase
        {
            if (enumerationRequest == null)
            {
                throw new ArgumentNullException(nameof(enumerationRequest), "Enumeration request cannot be null.");
            }
            _logger.LogInformation("Collecting next object from URL {Url} for tenant {TenantId} with execution ID {ExecutionId}",
                enumerationRequest.Url, enumerationRequest.TenantId, enumerationRequest.executionId);

            var result = await _graphClient.GetGraphObjectsAsync<T>(
                enumerationRequest.Url,
                enumerationRequest.SettingKey,
                enumerationRequest.TenantId,
                enumerationRequest.executionId,
                stopToken);

            if (result?.Value == null)
            {
                return;
            }

            await _objectRepository.WriteAsync(result.Value);

            if (string.IsNullOrWhiteSpace(result?.ODataNextLink) is false)
            {
                var nextRequest = new EnumerationRequest
                {
                    Url = result.ODataNextLink,
                    SettingKey = _authSettings.GetAuthKey(),
                    TenantId = enumerationRequest.TenantId,
                    executionId = enumerationRequest.executionId,
                    ObjectType = typeof(T)
                };

                await _messageFactory.SendMessageAsync("ObjectEnumeration", "request", JsonUtil.Serialize(nextRequest));
            }
            else
            {
                _logger.LogInformation("No more pages of {T} to collect for tenant {TenantId}", typeof(T), enumerationRequest.TenantId);
                return;
            }
        }
    }
}
