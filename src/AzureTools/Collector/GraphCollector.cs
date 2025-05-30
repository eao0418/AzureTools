namespace AzureTools.Automation.Collector
{
    using Azure.Core;
    using AzureTools.Authentication.Settings;
    using AzureTools.Client;
    using AzureTools.Client.Model;
    using AzureTools.Repository;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Diagnostics.CodeAnalysis;
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
                    ObjectType = typeof(User)
                };

                return JsonSerializer.Serialize(nextRequest, new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
            else
            {
                _logger.LogInformation("No more pages of users to collect for tenant {TenantId}", tenantId);
                return string.Empty;
            }
        }

        public async Task<string> CollectNextObjectAsync(EnumerationRequest enumerationRequest, CancellationToken stopToken)
        {
            if (enumerationRequest == null)
            {
                throw new ArgumentNullException(nameof(enumerationRequest), "Enumeration request cannot be null.");
            }
            _logger.LogInformation("Collecting next object from URL {Url} for tenant {TenantId} with execution ID {ExecutionId}",
                enumerationRequest.Url, enumerationRequest.TenantId, enumerationRequest.executionId);

            string url;

            switch (enumerationRequest.ObjectType)
            {
                case Type t when t == typeof(User):
                    var result = await _graphClient.GetGraphObjectsAsync<User>(
                        enumerationRequest.Url,
                        enumerationRequest.SettingKey,
                        enumerationRequest.TenantId,
                        enumerationRequest.executionId,
                        stopToken);
                    if (result?.Value != null)
                    {
                        await _objectRepository.WriteAsync(result.Value);
                        _logger.LogInformation("Successfully collected users from URL {Url} for tenant {TenantId}",
                            enumerationRequest.Url, enumerationRequest.TenantId);
                    }
                    else
                    {
                        _logger.LogWarning("No users found at URL {Url} for tenant {TenantId}", enumerationRequest.Url, enumerationRequest.TenantId);
                    }
                    url = result?.ODataNextLink ?? string.Empty;
                    break;
                case Type t when t == typeof(Group):
                    var groupResult = await _graphClient.GetGraphObjectsAsync<Group>(
                        enumerationRequest.Url,
                        enumerationRequest.SettingKey,
                        enumerationRequest.TenantId,
                        enumerationRequest.executionId,
                        stopToken);
                    if (groupResult?.Value != null)
                    {
                        await _objectRepository.WriteAsync(groupResult.Value);
                        _logger.LogInformation("Successfully collected groups from URL {Url} for tenant {TenantId}",
                            enumerationRequest.Url, enumerationRequest.TenantId);
                    }
                    else
                    {
                        _logger.LogWarning("No groups found at URL {Url} for tenant {TenantId}", enumerationRequest.Url, enumerationRequest.TenantId);
                    }
                    url = groupResult?.ODataNextLink ?? string.Empty;
                    break;
                case Type t when t == typeof(ServicePrincipal):
                    var spResult = await _graphClient.GetGraphObjectsAsync<ServicePrincipal>(
                        enumerationRequest.Url,
                        enumerationRequest.SettingKey,
                        enumerationRequest.TenantId,
                        enumerationRequest.executionId,
                        stopToken);
                    if (spResult?.Value != null)
                    {
                        await _objectRepository.WriteAsync(spResult.Value);
                        _logger.LogInformation("Successfully collected ServicePrincipal from URL {Url} for tenant {TenantId}",
                            enumerationRequest.Url, enumerationRequest.TenantId);
                    }
                    else
                    {
                        _logger.LogWarning("No ServicePrincipal found at URL {Url} for tenant {TenantId}", enumerationRequest.Url, enumerationRequest.TenantId);
                    }
                    url = spResult?.ODataNextLink ?? string.Empty;
                    break;
                case Type t when t == typeof(ApplicationRegistration):
                    var applicationRegistration = await _graphClient.GetGraphObjectsAsync<ApplicationRegistration>(
                        enumerationRequest.Url,
                        enumerationRequest.SettingKey,
                        enumerationRequest.TenantId,
                        enumerationRequest.executionId,
                        stopToken);
                    if (applicationRegistration?.Value != null)
                    {
                        await _objectRepository.WriteAsync(applicationRegistration.Value);
                        _logger.LogInformation("Successfully collected applicationRegistration from URL {Url} for tenant {TenantId}",
                            enumerationRequest.Url, enumerationRequest.TenantId);
                    }
                    else
                    {
                        _logger.LogWarning("No applicationRegistration found at URL {Url} for tenant {TenantId}", enumerationRequest.Url, enumerationRequest.TenantId);
                    }
                    url = applicationRegistration?.ODataNextLink ?? string.Empty;
                    break;
                case Type t when t == typeof(DirectoryRole):
                    var directoryRole = await _graphClient.GetGraphObjectsAsync<DirectoryRole>(
                        enumerationRequest.Url,
                        enumerationRequest.SettingKey,
                        enumerationRequest.TenantId,
                        enumerationRequest.executionId,
                        stopToken);
                    if (directoryRole?.Value != null)
                    {
                        await _objectRepository.WriteAsync(directoryRole.Value);
                        _logger.LogInformation("Successfully collected directoryRole from URL {Url} for tenant {TenantId}",
                            enumerationRequest.Url, enumerationRequest.TenantId);
                    }
                    else
                    {
                        _logger.LogWarning("No directoryRole found at URL {Url} for tenant {TenantId}", enumerationRequest.Url, enumerationRequest.TenantId);
                    }
                    url = directoryRole?.ODataNextLink ?? string.Empty;
                    break;
                default:
                    throw new Exception($"Unsupported object type: {enumerationRequest.ObjectType?.FullName ?? "null"}");
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                _logger.LogInformation("No more pages to collect for tenant {TenantId} with execution ID {ExecutionId}",
                    enumerationRequest.TenantId, enumerationRequest.executionId);
                return string.Empty;
            }

            return JsonSerializer.Serialize(new EnumerationRequest
            {
                Url = url,
                SettingKey = enumerationRequest.SettingKey,
                TenantId = enumerationRequest.TenantId,
                executionId = enumerationRequest.executionId,
                ObjectType = enumerationRequest.ObjectType
            }, new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
