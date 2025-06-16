

namespace AzureTools.Secrets.Automation.Triggers
{
    using AzureTools.Secrets.Automation.Service;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Logging;

    public class Timers
    {
        private readonly ILogger<Timers> _logger;
        private readonly ApplicationCredentialManager _applicationCredentialManager;

        public Timers(
            ILogger<Timers> logger,
            ApplicationCredentialManager applicationCredentialManager
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _applicationCredentialManager = applicationCredentialManager ?? throw new ArgumentNullException(nameof(applicationCredentialManager));
        }

        /// <summary>
        /// Checks and updates application credentials daily at 9:30 AM UTC.
        /// </summary>
        /// <param name="timerInfo">The timer info.</param>
        /// <param name="context">The function Context.</param>
        /// <returns>A Task representing the result of the asynchronous call.</returns>
        [Function(nameof(CheckAndUpdateApplicationCredentials))]
        [FixedDelayRetry(5, "00:00:10")]
        public async Task CheckAndUpdateApplicationCredentials(
                [TimerTrigger("0 30 9 * * *")] TimerInfo timerInfo,
                FunctionContext context)
        {
            _logger.LogInformation("C# {trigger} Timer trigger function executed at: {time}",
                nameof(CheckAndUpdateApplicationCredentials),
                DateTime.Now);

            try
            {
                await _applicationCredentialManager.CheckForPasswordsReadyToExpire(context.CancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating credentials.");
            }
            finally
            {
                _logger.LogInformation("C# Timer trigger function completed at: {time}", DateTime.Now);

            }
        }
    }
}