
namespace AzureTools.Automation.Functions.Triggers
{
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Logging;

    public class QueueTriggers
    {
        private readonly ILogger<QueueTriggers> _logger;

        public QueueTriggers(ILogger<QueueTriggers> logger)
        {
            _logger = logger;
        }

        public async Task KafkaTrigger(
            [KafkaTrigger(
            "%BrokerList%",
            "ObjectEnumeration",
            ConsumerGroup = "%ConsumerGroup%",
            Protocol = BrokerProtocol.SaslSsl,
            Username = "%KafkaConnection%",
            Password = "%ConnectionString%")] string kafkaEvent, FunctionContext functionContext)
        {

        }
    }
}