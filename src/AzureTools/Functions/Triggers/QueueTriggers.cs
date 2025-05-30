
namespace AzureTools.Automation.Functions.Triggers
{
    using AzureTools.Automation.Collector;
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

        public async Task<KafkaOutputBinding?> KafkaTrigger(
            [KafkaTrigger(
            "%BrokerList%",
            "ObjectEnumeration",
            ConsumerGroup = "%ConsumerGroup%",
            Protocol = BrokerProtocol.SaslSsl,
            Username = "%KafkaConnection%",
            Password = "%ConnectionString%")] string kafkaEvent, FunctionContext functionContext)
        {
            var request = JsonSerializer.Deserialize<EnumerationRequest>(kafkaEvent);

            if (request == null)
            {
                _logger.LogError("Failed to deserialize Kafka event.");
                return null;
            }

            string result = await _graphCollector.CollectNextObjectAsync(request, functionContext.CancellationToken);

            if (string.IsNullOrWhiteSpace(result))
            {
                _logger.LogInformation("No more objects to process.");
                return null;
            }
            else
            {
                return new KafkaOutputBinding
                {
                    Kevent = result,
                };
            }
        }
    }
}