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

    public class QueueTriggers
    {
        private readonly ILogger<QueueTriggers> _logger;
        private readonly GraphCollector _graphCollector;

        public QueueTriggers(ILogger<QueueTriggers> logger,
            GraphCollector graphCollector)
        {
            _logger = logger;
            _graphCollector = graphCollector;
        }

        /*
        public QueueTriggers(ILogger<QueueTriggers> logger)
        {
            _logger = logger;
        }
        */

        [Function(nameof(ProcessObjectEnumerationQueue))]
        public async Task ProcessObjectEnumerationQueue(
            [KafkaTrigger(
               "%BrokerList%",
               "ObjectEnumeration",
               ConsumerGroup = "%ConsumerGroup%",
               Username = "%KafkaConnection%",
               Password = "%ConnectionString%")] string kafkaEvent, FunctionContext functionContext)
        {
            var request = JsonSerializer.Deserialize<EnumerationRequest>(kafkaEvent);

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
        public void ProcessGroupMemberEnumerationQueue(
            [KafkaTrigger(
               "%BrokerList%",
               "GroupMemberEnumeration",
               ConsumerGroup = "%ConsumerGroup%",
               Username = "%KafkaConnection%",
               Password = "%ConnectionString%")] string kafkaMessage , FunctionContext functionContext)
        {
            
            Console.WriteLine($"Received message: {kafkaMessage}");

            var request = JsonSerializer.Deserialize<KafkaMessage>(kafkaMessage);

            Console.WriteLine($"Deserialized message: {request?.Value}");
        }
    }
}