
namespace AzureTools.Automation.Functions.Triggers
{
    using AzureTools.Automation.Collector;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Logging;

    public class TimerTriggers
    {
        private readonly ILogger _logger;
        private readonly GraphCollector _graphCollector;

        public TimerTriggers(ILoggerFactory loggerFactory, GraphCollector graphCollector)
        {
            _logger = loggerFactory.CreateLogger<TimerTriggers>();
            _graphCollector = graphCollector ?? throw new ArgumentNullException(nameof(graphCollector));
        }

        [Function(nameof(StartGraphUserCollection))]
        [FixedDelayRetry(5, "00:00:10")]
        public async Task<KafkaOutputBinding?> StartGraphUserCollection([TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo,
            FunctionContext context)
        {
            _logger.LogInformation("C# Timer trigger function executed at: {time}", DateTime.Now);

            try
            {
                // Here you would call the method to collect data, e.g.:
                var response = await _graphCollector.CollectUsersAsync(context.InvocationId.ToString(), context.CancellationToken);
                _logger.LogInformation("Data collection started.");

                if (string.IsNullOrWhiteSpace(response) is false)
                {
                    return new KafkaOutputBinding()
                    {
                        Kevent = response,
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while collecting data.");
            }
            finally
            {
                _logger.LogInformation("C# Timer trigger function completed at: {time}", DateTime.Now);

            }

            return null;
        }
    }
}