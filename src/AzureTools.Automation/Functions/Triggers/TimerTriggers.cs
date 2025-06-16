// TimerTriggers.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

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
        public async Task StartGraphUserCollection([TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo,
            FunctionContext context)
        {
            _logger.LogInformation("C# {trigger} Timer trigger function executed at: {time}",
                nameof(StartGraphUserCollection),
                DateTime.Now);

            try
            {
                _logger.LogInformation("Data collection started.");
                await _graphCollector.CollectUsersAsync(context.InvocationId.ToString(), context.CancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                _logger.LogError($"Status Code: {(ex.Data.Contains("StatusCode") ? ex.Data["StatusCode"] : "Unknown")}");
                _logger.LogError($"Inner Exception: {ex.InnerException?.Message}");
            }
            finally
            {
                _logger.LogInformation("C# Timer trigger function completed at: {time}", DateTime.Now);

            }
        }

        [Function(nameof(StartGraphGroupCollection))]
        [FixedDelayRetry(5, "00:00:10")]
        public async Task StartGraphGroupCollection([TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo,
            FunctionContext context)
        {
            _logger.LogInformation("C# {trigger} Timer trigger function executed at: {time}",
                nameof(StartGraphGroupCollection),
                DateTime.Now);

            try
            {
                await _graphCollector.CollectGroupsAsync(
                    context.InvocationId.ToString(),
                    context.CancellationToken);
                _logger.LogInformation("Data collection started.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while collecting data.");
            }
            finally
            {
                _logger.LogInformation("C# Timer trigger function completed at: {time}", DateTime.Now);

            }
        }

        [Function(nameof(StartGraphApplicationRegistrationCollection))]
        [FixedDelayRetry(5, "00:00:10")]
        public async Task StartGraphApplicationRegistrationCollection(
                [TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo,
                FunctionContext context)
        {
            _logger.LogInformation("C# {trigger} Timer trigger function executed at: {time}",
                nameof(StartGraphApplicationRegistrationCollection),
                DateTime.Now);

            try
            {
                _logger.LogInformation("Data collection started.");
                await _graphCollector.CollectAppRegistrationsAsync(
                    context.InvocationId.ToString(),
                    context.CancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while collecting data.");
            }
            finally
            {
                _logger.LogInformation("C# Timer trigger function completed at: {time}", DateTime.Now);

            }
        }

        [Function(nameof(StartGraphServicePrincipalCollection))]
        [FixedDelayRetry(5, "00:00:10")]
        public async Task StartGraphServicePrincipalCollection(
                [TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo,
                FunctionContext context)
        {
            _logger.LogInformation("C# {trigger} Timer trigger function executed at: {time}",
                nameof(StartGraphServicePrincipalCollection),
                DateTime.Now);

            try
            {
                _logger.LogInformation("Data collection started.");
                await _graphCollector.CollectServicePrincipalsAsync(
                    context.InvocationId.ToString(),
                    context.CancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while collecting data.");
            }
            finally
            {
                _logger.LogInformation("C# Timer trigger function completed at: {time}", DateTime.Now);

            }
        }

        [Function(nameof(StartGraphDirectoryRoleCollection))]
        [FixedDelayRetry(5, "00:00:10")]
        public async Task StartGraphDirectoryRoleCollection(
                [TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo,
                FunctionContext context)
        {
            _logger.LogInformation("C# {trigger} Timer trigger function executed at: {time}",
                nameof(StartGraphDirectoryRoleCollection),
                DateTime.Now);

            try
            {
                _logger.LogInformation("Data collection started.");
                await _graphCollector.CollectDirectoryRolesAsync(
                    context.InvocationId.ToString(),
                    context.CancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while collecting data.");
            }
            finally
            {
                _logger.LogInformation("C# Timer trigger function completed at: {time}", DateTime.Now);

            }
        }
    }
}