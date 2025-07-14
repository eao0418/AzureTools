// QueueTriggers.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Automation.Functions.Triggers
{
    using AzureTools.Automation.Collector;
    using AzureTools.Automation.Messaging;
    using AzureTools.Client.Model;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Logging;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class QueueTriggers
    {
        private readonly ILogger<QueueTriggers> _logger;
        private readonly GraphCollector _graphCollector;
        private readonly ARMCollector _armCollector;

        public QueueTriggers(ILogger<QueueTriggers> logger,
            GraphCollector graphCollector,
            ARMCollector armCollector)
        {
            _logger = logger;
            _graphCollector = graphCollector;
            _armCollector = armCollector;
        }

        [Function(nameof(ProcessObjectEnumerationQueue))]
        public async Task ProcessObjectEnumerationQueue(
            [KafkaTrigger(
               "%BrokerList%",
               MessageTopics.ObjectEnumerationTopic,
               ConsumerGroup = "%ConsumerGroup%",
               Username = "%KafkaConnection%",
               Password = "%ConnectionString%")] string kafkaEvent, FunctionContext functionContext)
        {


            var message = JsonDocument.Parse(kafkaEvent);
            var parsed = message.RootElement
                .GetProperty("Value")
                .GetString();

            if (string.IsNullOrWhiteSpace(parsed))
            {
                return;
            }

            var request = JsonUtil.Deserialize<EnumerationRequest>(parsed);

            if (request == null)
            {
                _logger.LogError("Failed to deserialize Kafka event.");
                return;
            }

            switch (request.ObjectType)
            {
                case Type t when t == typeof(User):
                    await _graphCollector.CollectNextObjectAsync<User>(request, functionContext.CancellationToken);
                    break;
                case Type t when t == typeof(Group):
                    await _graphCollector.CollectNextObjectAsync<Group>(request, functionContext.CancellationToken);
                    break;
                case Type t when t == typeof(DirectoryRole):
                    await _graphCollector.CollectNextObjectAsync<DirectoryRole>(request, functionContext.CancellationToken);
                    break;
                case Type t when t == typeof(ApplicationRegistration):
                    await _graphCollector.CollectNextObjectAsync<ApplicationRegistration>(request, functionContext.CancellationToken);
                    break;
                case Type t when t == typeof(ServicePrincipal):
                    await _graphCollector.CollectNextObjectAsync<ServicePrincipal>(request, functionContext.CancellationToken);
                    break;
                default:
                    _logger.LogError("Unsupported object type: {ObjectType}", request.ObjectType);
                    return;
            }
        }

        [Function(nameof(ProcessGroupMemberEnumerationQueue))]
        public async Task ProcessGroupMemberEnumerationQueue(
            [KafkaTrigger(
               "%BrokerList%",
               MessageTopics.GroupMembershipTopic,
               ConsumerGroup = "%ConsumerGroup%",
               Username = "%KafkaConnection%",
               Password = "%ConnectionString%")] string kafkaMessage, FunctionContext functionContext)
        {

            Console.WriteLine($"Received message: {kafkaMessage}");

            var message = JsonDocument.Parse(kafkaMessage);
            var parsed = message.RootElement
                .GetProperty("Value")
                .GetString();

            if (string.IsNullOrWhiteSpace(parsed))
            {
                return;
            }

            var request = JsonUtil.Deserialize<GroupMembershipMessage>(parsed);

            if (request == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(request.ODataNextLink))
            {
                await _graphCollector.CollectGroupMembersAsync(request, functionContext.CancellationToken);
            }
            else // perform an enumeration request.
            {
                await _graphCollector.CollectNextGroupMembershipAsync(request, functionContext.CancellationToken);
            }
        }

        [Function(nameof(ProcessApplicationRegistrationOwnerQueue))]
        public async Task ProcessApplicationRegistrationOwnerQueue(
            [KafkaTrigger(
               "%BrokerList%",
               MessageTopics.ApplicationRegistrationOwnersTopic,
               ConsumerGroup = "%ConsumerGroup%",
               Username = "%KafkaConnection%",
               Password = "%ConnectionString%")] string kafkaMessage, FunctionContext functionContext)
        {
            var message = JsonDocument.Parse(kafkaMessage);
            var parsed = message.RootElement
                .GetProperty("Value")
                .GetString();

            if (string.IsNullOrWhiteSpace(parsed))
            {
                return;
            }

            var request = JsonUtil.Deserialize<AppRegistrationOwnerRequest>(parsed);    

            if (request == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(request.ODataNextLink))
            {
                await _graphCollector.CollectApplicationRegistrationOwnersAsync(request, functionContext.CancellationToken);
            }
        }

        [Function(nameof(ProcessResourcePropertiesQueue))]
        public async Task ProcessResourcePropertiesQueue(
            [KafkaTrigger(
               "%BrokerList%",
               MessageTopics.ResourcePropertiesTopic,
               ConsumerGroup = "%ConsumerGroup%",
               Username = "%KafkaConnection%",
               Password = "%ConnectionString%")] string kafkaMessage, FunctionContext functionContext)
        {
            var message = JsonDocument.Parse(kafkaMessage);
            var parsed = message.RootElement
                .GetProperty("Value")
                .GetString();

            if (string.IsNullOrWhiteSpace(parsed))
            {
                return;
            }

            var request = JsonUtil.Deserialize<ResourcePropertyMessage>(parsed);

            if (request == null)
            {
                return;
            }

            await _armCollector.GetResourcePropertiesForResourceAsync(request, functionContext.CancellationToken);
        }

        [Function(nameof(ProcessSubscriptionsQueue))]
        public async Task ProcessSubscriptionsQueue(
            [KafkaTrigger(
               "%BrokerList%",
               MessageTopics.SubscriptionEnumerationTopic,
               ConsumerGroup = "%ConsumerGroup%",
               Username = "%KafkaConnection%",
               Password = "%ConnectionString%")] string kafkaMessage, FunctionContext functionContext)
        {
            var message = JsonDocument.Parse(kafkaMessage);
            var parsed = message.RootElement
                .GetProperty("Value")
                .GetString();

            if (string.IsNullOrWhiteSpace(parsed))
            {
                return;
            }

            var request = JsonUtil.Deserialize<SubscriptionMessage>(parsed);

            if (request == null)
            {
                return;
            }

            await _armCollector.CollectResourcesAsync(request, functionContext.CancellationToken);
        }
    }
}