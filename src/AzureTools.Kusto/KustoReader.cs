// KustoReader.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Kusto
{
    using AzureTools.Kusto.Authentication;
    using AzureTools.Kusto.Settings;
    using global::Kusto.Data.Common;
    using global::Kusto.Data.Net.Client;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class KustoReader : IKustoReader
    {
        private readonly ILogger<KustoReader> _logger;
        private readonly IKustoConnectionStringProvider _connectionStringProvider;
        private readonly KustoSettings _settings;
        private readonly ConcurrentDictionary<string, ICslQueryProvider> _providers = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="KustoReader"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="connectionStringProvider">The <see cref="IKustoConnectionStringProvider"/> implementation.</param>
        /// <param name="settings">The <see cref="KustoSettings"/>.</param>
        /// <exception cref="ArgumentNullException">If a required value is null.</exception>
        public KustoReader(
            ILogger<KustoReader> logger,
            IKustoConnectionStringProvider connectionStringProvider,
            IOptions<KustoSettings> settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _connectionStringProvider = connectionStringProvider ?? throw new ArgumentNullException(nameof(connectionStringProvider));
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));

            Init();
        }

        /// <summary>
        /// Executes a Kusto query asynchronously and returns the results as an enumerable collection of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to convert.</typeparam>
        /// <param name="databaseName">The database to execute the query on.</param>
        /// <param name="query">The kusto query.</param>
        /// <param name="stopToken">The stop token.</param>
        /// <returns>An IEnumerable of results.</returns>
        /// <exception cref="InvalidOperationException">If there is no </exception>
        public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string databaseName, string query, CancellationToken stopToken) where T : class, new()
        {
            if (_providers.TryGetValue(databaseName, out var provider))
            {
                var reader = await provider.ExecuteQueryAsync(databaseName, query, new ClientRequestProperties(), stopToken);
                return reader.ToEnumerable<T>();
            }

            throw new InvalidOperationException($"No provider found for database '{databaseName}'. Ensure that the KustoReader is initialized with the correct settings.");
        }

        /// <summary>
        /// Initializes the KustoReader by loading authentication settings and creating providers.
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
                var connectionString = _connectionStringProvider.GetConnectionString(authSettings) ??
                    throw new InvalidOperationException($"Connection string for {authSettings.ClusterUrl} is not configured.");
                var provider = KustoClientFactory.CreateCslQueryProvider(connectionString);

                _providers.TryAdd(authSettings.DatabaseName, provider);
            }
        }
    }
}
