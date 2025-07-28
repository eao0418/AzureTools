namespace AzureTools.Secrets.Automation.Service
{
    using AzureTools.Client;
    using AzureTools.Client.Model.Application;
    using AzureTools.Kusto;
    using AzureTools.Secrets.Automation.Model;
    using Microsoft.Extensions.Options;
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using global::Kusto.Data.Common;

    public class ApplicationCredentialManager
    {
        private readonly IGraphClient _graphClient;
        private readonly SecretAutomationSettings _settings;
        private readonly IKustoReader _kustoReader;
        private readonly ILogger<ApplicationCredentialManager> _logger;

        public ApplicationCredentialManager(
            ILogger<ApplicationCredentialManager> logger,
            IGraphClient graphClient,
            IOptions<SecretAutomationSettings> settingsOptions,
            IKustoReader kustoReader
            )
        {
            _graphClient = graphClient ?? throw new ArgumentNullException(nameof(graphClient));
            _settings = settingsOptions?.Value ?? throw new ArgumentNullException(nameof(settingsOptions));
            _kustoReader = kustoReader ?? throw new ArgumentNullException(nameof(kustoReader));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if ( _settings.AuthenticationSettings == null)
            {
                throw new Exception($"{nameof(_settings.AuthenticationSettings)} are required for {nameof(ApplicationCredentialManager)}");
            }
        }

        public async Task CheckForPasswordsReadyToExpire(CancellationToken stopToken)
        {
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "queries", "GetExpiringPasswords.kql");
            var query = await File.ReadAllTextAsync(path, stopToken);

            if (string.IsNullOrEmpty(query))
            {
                throw new Exception("Kusto query for expiring passwords is empty.");
            }

            var results = await _kustoReader.ExecuteQueryAsync<ExpiringApplicationPassword>(
                _settings.DataSourceName,
                query,
                ExpiringApplicationPassword.CreateFromReader,
                stopToken,
                new ClientRequestProperties());

            if ( results == null || results.Any() is false)
            {
                _logger.LogInformation("No expiring passwords found.");
                return;
            }
            else
            {
                _logger.LogInformation("Found {count} expiring passwords.", results.Count());

                foreach (var result in results)
                {
                    await UpdateApplicationPasswordAsync(result.Id, result.DisplayName, stopToken);
                }
            }
        }

        public async Task UpdateApplicationPasswordAsync(string applicationId, string displayName, CancellationToken stopToken)
        {
            if (string.IsNullOrEmpty(applicationId))
            {
                throw new ArgumentException("Application ID cannot be null or empty.", nameof(applicationId));
            }

            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentException("Display name cannot be null or empty.", nameof(displayName));
            }

            // Create a new password credential with the specified display name
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();

            // To-Do: add a configurable expiration time
            var updateRequest = new UpdatePasswordRequest
            {
                PasswordCredential = new PasswordCredential
                {
                    DisplayName = $"{displayName}_{now}"
                }
            };

#pragma warning disable CS8604 // Possible null reference argument. This is checked in the ctor. insert eye roll here for the warning VS.
             await _graphClient.AddApplicationPasswordAsync(applicationId, updateRequest, _settings.AuthenticationSettings, stopToken);
#pragma warning restore CS8604 // Possible null reference argument.

            // To-Do write the result to a secret handler
        }
    }
}
