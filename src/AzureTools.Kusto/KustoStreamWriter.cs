// KustStreamWriter.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Kusto
{
    using Microsoft.Extensions.Logging;
    using AzureTools.Kusto.Authentication;
    using Microsoft.Extensions.Options;
    using AzureTools.Kusto.Settings;
    using System.Collections.Concurrent;
    using global::Kusto.Ingest;
    using global::Kusto.Data.Ingestion;
    using global::Kusto.Data.Common;
    using AzureTools.Utility;

    public class KustoStreamWriter : IKustoWriter
    {
        private readonly ILogger<KustoStreamWriter> _logger;
        private readonly IKustoConnectionStringProvider _connectionStringProvider;
        private readonly KustoSettings _settings;
        private readonly ConcurrentDictionary<string, IKustoIngestClient> _providers = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="KustoStreamWriter"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="connectionStringProvider">The <see cref="IKustoConnectionStringProvider"/> implementation.</param>
        /// <param name="settings">The <see cref="KustoSettings"/>.</param>
        /// <exception cref="ArgumentNullException">If a required value is null.</exception>
        public KustoStreamWriter(
            ILogger<KustoStreamWriter> logger,
            IKustoConnectionStringProvider connectionStringProvider,
            IOptions<KustoSettings> settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionStringProvider = connectionStringProvider ?? throw new ArgumentNullException(nameof(connectionStringProvider));
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

            Init();
        }

        public async Task IngestAsync<T>(string tableName, string databaseName, IEnumerable<T> items, CancellationToken stopToken)
        {
            if (_providers.TryGetValue(databaseName, out var provider))
            {
                try
                {
                    // Create a MemoryStream to hold the data  
                    using (var stream = new MemoryStream())
                    {
                        await JsonUtil.SerializeAsync(items, stream, stopToken);

                        stream.Seek(0, SeekOrigin.Begin);

                        // Ingest the stream into Kusto  
                        var ingestProperties = new KustoIngestionProperties(databaseName, tableName);
                        ingestProperties.IngestionMapping = new IngestionMapping()
                        {
                            IngestionMappingKind = IngestionMappingKind.Json,
                            IngestionMappingReference = $"{tableName}Mapping",
                        };
                        ingestProperties.Format = DataSourceFormat.multijson;

                        await provider.IngestFromStreamAsync(
                            stream,
                            ingestProperties);

                        _logger.LogInformation("Data written to Kusto table {TableName} in database {DatabaseName}.", tableName, databaseName);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to write data to Kusto table {TableName} in database {DatabaseName}.", tableName, databaseName);
                }
            }
            else
            {
                _logger.LogError("No provider found for database {DatabaseName}.", databaseName);
            }
        }

        /// <summary>
        /// Initializes the KustoWriter by loading authentication settings and creating providers.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        private void Init()
        {
            if (_settings.AuthenticationSettings == null || _settings.AuthenticationSettings.Count == 0)
            {
                throw new InvalidOperationException("Kusto authentication settings are not configured.");
            }

            foreach (var authSettings in _settings.AuthenticationSettings)
            {
                var connectionString = _connectionStringProvider.GetConnectionString(authSettings);
                if (connectionString is null)
                {
                    throw new InvalidOperationException($"Connection string for {authSettings.ClusterUrl} is not configured.");
                }

                var provider = KustoIngestFactory.CreateQueuedIngestClient(connectionString);

                _providers.TryAdd(authSettings.DatabaseName, provider);
            }
        }
    }
}
