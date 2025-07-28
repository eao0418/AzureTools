namespace AzureTools.Client
{
    using AzureTools.Authentication;
    using AzureTools.Authentication.Settings;
    using AzureTools.Client.Settings;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    /// <summary>
    /// Extension methods for configuring the Graph API client in the service collection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the Graph API client to the service collection with configuration settings.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> implementation.</param>
        /// <param name="config">The <see cref="IConfiguration"/> implementation.</param>
        /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddGraphClient(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<GraphClientSettings>(config.GetSection(GraphClientSettings.ConfigurationSectionName));
            services.PostConfigure<GraphClientSettings>(s => {
                
                //
                // Allows the user to dynamically configure which properties they try to get from the endpoint, depending on their permissions.
                //
                if (s.UserProperties.Count == 0)
                {
                    s.UsersEndpoint = GraphEndpoints.UsersEndpointBase;
                }
                else
                {
                    s.UsersEndpoint = $"{GraphEndpoints.UsersEndpointBase}?$select={string.Join(",", s.UserProperties)}";
                }
                
                if (s.GroupProperties.Count == 0)
                {
                    s.GroupsEndpoint = GraphEndpoints.GroupsEndpoint;
                }
                else
                {
                    s.GroupsEndpoint = $"{GraphEndpoints.GroupsEndpointBase}{string.Join(",", s.GroupProperties)}";
                }
                
                if (s.ApplicationRegistrationProperties.Count == 0)
                {
                    s.ApplicationRegistrationEndpoint = GraphEndpoints.ApplicationRegistrationEndpoint;
                }
                else
                {
                    s.ApplicationRegistrationEndpoint = $"{GraphEndpoints.ApplicationsEndpointBase}{string.Join(",", s.ApplicationRegistrationProperties)}";
                }
                
                if (s.ApplicationOwnerProperties.Count == 0)
                {
                    s.ApplicationOwnersEndpoint = GraphEndpoints.ApplicationOwnersEndpoint;
                }
                else
                {
                    s.ApplicationOwnersEndpoint = $"{GraphEndpoints.ApplicationOwnersEndpointBase}{string.Join(",", s.ApplicationOwnerProperties)}";
                }

                if (s.GroupMemberProperties.Count == 0)
                {
                    s.GroupMembersEndpoint = GraphEndpoints.GroupMembersEndpoint;
                }
                else
                {
                    s.GroupMembersEndpoint = $"{GraphEndpoints.GroupMembersEndpointBase}{string.Join(",", s.GroupMemberProperties)}";
                }

                if (s.ServicePrincipalProperties.Count == 0)
                {
                    s.ServicePrincipalsEndpoint = GraphEndpoints.ServicePrincipalsEndpoint;
                }
                else
                {
                    s.ServicePrincipalsEndpoint = $"{GraphEndpoints.ServicePrincipalsEndpointBase}{string.Join(",", s.ServicePrincipalProperties)}";
                }

                if (s.DirectoryRoleProperties.Count == 0)
                {
                    s.DirectoryRolesEndpoint = GraphEndpoints.DirectoryRolesEndpoint;
                }
                else
                {
                    s.DirectoryRolesEndpoint = $"{GraphEndpoints.DirectoryRolesEndpointBase}{string.Join(",", s.DirectoryRoleProperties)}";
                }

            });
            services.Configure<AuthenticationSettings>(config.GetSection("graphAuthenticationSettings"));
            services.AddHttpClient<GraphClient>(client =>
            {
                client.BaseAddress = new Uri("https://graph.microsoft.com");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddSingleton<IGraphClient, GraphClient>();
            services.AddAuthenticationServices();
            return services;
        }

        public static IServiceCollection AddARMClient(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AuthenticationSettings>(config.GetSection("armAuthenticationSettings"));
            services.AddHttpClient<ARMClient>(client =>
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            services.AddSingleton<IARMClient, ARMClient>();
            services.AddAuthenticationServices();
            return services;
        }
    }
}
