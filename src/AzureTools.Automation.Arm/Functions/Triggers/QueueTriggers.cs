// QueueTriggers.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Automation.Arm.Functions.Triggers
{
    using AzureTools.Automation.Arm.Collector;
    using AzureTools.Automation.Arm.Messaging;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Logging;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class QueueTriggers
    {
        private readonly ILogger<QueueTriggers> _logger;
        private readonly ARMCollector _armCollector;

        public QueueTriggers(ILogger<QueueTriggers> logger,
            ARMCollector armCollector)
        {
            _logger = logger;
            _armCollector = armCollector;
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