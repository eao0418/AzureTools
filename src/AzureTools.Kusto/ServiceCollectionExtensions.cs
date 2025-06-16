// ServiceCollectionExtensions.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Repository
{
    using AzureTools.Kusto;
    using AzureTools.Kusto.Authentication;
    using AzureTools.Kusto.Settings;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Adds the necessary services to the <see cref="IServiceCollection"/> for Kusto writer support.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="config">The <see cref="IConfiguration"/> to get the correct settings.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddKustoWriter(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<KustoSettings>(config.GetSection(KustoSettings.ConfigurationSectionName));
            services.AddSingleton<IKustoConnectionStringProvider, KustoConnectionStringProvider>();
            services.AddSingleton<IKustoWriter, KustoStreamWriter>();
            return services;
        }

        /// <summary>
        /// Adds the necessary services to the <see cref="IServiceCollection"/> for Kusto writer support.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="config">The <see cref="IConfiguration"/> to get the correct settings.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddKustoReader(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<KustoSettings>(config.GetSection(KustoSettings.ConfigurationSectionName));
            services.AddSingleton<IKustoConnectionStringProvider, KustoConnectionStringProvider>();
            services.AddSingleton<IKustoReader, KustoReader>();
            return services;
        }
    }
}
