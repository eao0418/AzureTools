namespace AzureTools.Authentication
{
    using AzureTools.Authentication.Cache;
    using AzureTools.Authentication.Provider;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the authentication services to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddSingleton<ITokenCredentialProvider, TokenCredentialProvider>();
            services.AddSingleton<ITokenCache, TokenCache>();
            return services;
        }
    }
}
