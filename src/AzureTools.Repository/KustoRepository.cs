// KustoRepository.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Repository
{
    using AzureTools.Repository.Model;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using AzureTools.Repository.Settings;
    using AzureTools.Kusto;

    /// <summary>
    /// Object repository implementation that writes to Azure Data Explorer (Kusto).
    /// </summary>
    public sealed class KustoRepository : IObjectRepository
    {
        private readonly RepositorySettings _settings;
        private readonly IKustoWriter _kustoWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="KustoRepository"/> class.
        /// </summary>
        /// <param name="settings">The <see cref="RepositorySettings"/>.</param>
        /// <param name="kustoWriter">The <see cref="IKustoWriter"/> implementation.</param>
        /// <exception cref="ArgumentNullException">If a required value is null.</exception>
        /// <exception cref="ArgumentException">When a required setting is null or whitespace.</exception>
        public KustoRepository(IOptions<RepositorySettings> settings, IKustoWriter kustoWriter)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _kustoWriter = kustoWriter ?? throw new ArgumentNullException(nameof(kustoWriter));

            if (string.IsNullOrWhiteSpace(_settings.DatabaseName))
            {
                throw new ArgumentException("Database name must be provided in the repository settings.", nameof(settings));
            }
        }

        public async Task WriteAsync<T>(IEnumerable<T> items) where T : DtoBase
        {
            if (items == null || items.Any() is false)
            {
                throw new ArgumentException(nameof(items), "Items cannot be null or Empty");
            }

#pragma warning disable CS8604 // Possible null reference argument. This is checked on the ctor.
            await _kustoWriter.IngestAsync(
                typeof(T).Name,
                databaseName: _settings.DatabaseName,
                items,
                default);
#pragma warning restore CS8604 // Possible null reference argument.
        }
    }
}
