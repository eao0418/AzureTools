// ArmTimerTriggers.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Automation.Arm.Functions.Triggers
{
    using AzureTools.Automation.Arm.Collector;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Logging;

    public class ArmTimerTriggers
    {
        private readonly ILogger _logger;
        private readonly ARMCollector _armCollector;

        public ArmTimerTriggers(ILoggerFactory loggerFactory, ARMCollector collector)
        {
            _logger = loggerFactory.CreateLogger<ArmTimerTriggers>();
            _armCollector = collector ?? throw new ArgumentNullException(nameof(collector));
        }

        [Function(nameof(StartARMTenantCollection))]
        [FixedDelayRetry(5, "00:00:10")]
        public async Task StartARMTenantCollection([TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo,
            FunctionContext context)
        {
            _logger.LogInformation("C# {trigger} Timer trigger function executed at: {time}",
                nameof(StartARMTenantCollection),
                DateTime.Now);

            try
            {
                _logger.LogInformation("Data collection started.");
                await _armCollector.CollectTenantsAsync(context.InvocationId.ToString(), context.CancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                _logger.LogError($"Status Code: {(ex.Data.Contains("StatusCode") ? ex.Data["StatusCode"] : "Unknown")}");
                _logger.LogError($"Inner Exception: {ex.InnerException?.Message}");
            }
            finally
            {
                _logger.LogInformation("C# {trigger} trigger function completed at: {time}", nameof(StartARMTenantCollection), DateTime.Now);
            }
        }

        [Function(nameof(StartARMSubscriptionCollection))]
        [FixedDelayRetry(5, "00:00:10")]
        public async Task StartARMSubscriptionCollection([TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo,
            FunctionContext context)
        {
            _logger.LogInformation("C# {trigger} Timer trigger function executed at: {time}",
                nameof(StartARMSubscriptionCollection),
                DateTime.Now);

            try
            {
                _logger.LogInformation("Data collection started.");
                await _armCollector.CollectSubscriptionsAsync(context.InvocationId.ToString(), context.CancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                _logger.LogError($"Status Code: {(ex.Data.Contains("StatusCode") ? ex.Data["StatusCode"] : "Unknown")}");
                _logger.LogError($"Inner Exception: {ex.InnerException?.Message}");
            }
            finally
            {
                _logger.LogInformation("C# {trigger} trigger function completed at: {time}", nameof(StartARMSubscriptionCollection), DateTime.Now);
            }
        }
    }
}