namespace AzureTools.Automation.Collector
{
    using AzureTools.Authentication.Settings;
    using AzureTools.Client;
    using AzureTools.Repository;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Linq;
    using System.Text.Json;
    using System.Threading.Tasks;

    public sealed class GraphCollector
    {
        private readonly IGraphClient _graphClient;
        private readonly IObjectRepository _objectRepository;
        private readonly ILogger<GraphCollector> _logger;
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
            IOptions<AuthenticationSettings> authSettingsOption)
        {
            _graphClient = graphClient ?? throw new ArgumentNullException(nameof(graphClient));
            _objectRepository = objectRepository ?? throw new ArgumentNullException(nameof(objectRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _authSettings = authSettingsOption?.Value ?? throw new ArgumentNullException(nameof(authSettingsOption));
        }

        public async Task<string> CollectUsersAsync(string executionId, CancellationToken stopToken = default)
        {
            var tenantId = _authSettings.TenantId;

            _logger.LogInformation("Collecting users for tenant {TenantId} with execution ID {ExecutionId}", _authSettings.TenantId, executionId);

            var usersResponse = await _graphClient.GetUsersAsync(_authSettings, executionId, stopToken);

            if (usersResponse?.Value == null)
            {
                _logger.LogWarning("No users found for tenant {TenantId}", tenantId);
                return string.Empty;
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
                    Type = typeof(Client.Model.User)
                };

                return JsonSerializer.Serialize(nextRequest, new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            else
            {
                _logger.LogInformation("No more pages of users to collect for tenant {TenantId}", tenantId);
                return string.Empty;
            }
        }
    }
}
