// ARMClientAccessor.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Cli
{
    using AzureTools.Authentication.Settings;
    using AzureTools.Client;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class ARMClientAccessor
    {
        public static async Task<int> ListTenants(IHost host, string armAuthSettingsKey, CancellationToken stopToken)
        {
            var optionsMonitor = host.Services.GetRequiredService<IOptionsMonitor<AuthenticationSettings>>();
            var armSettings = optionsMonitor.Get(armAuthSettingsKey);

            Console.WriteLine($"Settings: {armSettings.Audience}, {armSettings.AuthenticationType}");

            var armClient = host.Services.GetRequiredService<ARMClient>();
            var tenants = await armClient.GetTenantsAsync(armSettings, stopToken: stopToken);

            if (tenants == null || !tenants.Value.Any())
            {
                Console.WriteLine("No tenants found.");
                return 1;
            }

            foreach (var tenant in tenants.Value)
            {
                Console.WriteLine($"Tenant information: {tenant.ToString()}");

            }
            return 0;
        }

        public static async Task<int> ListSubscriptions(IHost host, string armAuthSettingsKey, CancellationToken stopToken)
        {
            var optionsMonitor = host.Services.GetRequiredService<IOptionsMonitor<AuthenticationSettings>>();
            var armSettings = optionsMonitor.Get(armAuthSettingsKey);

            Console.WriteLine($"Settings: {armSettings.Audience}, {armSettings.AuthenticationType}");

            var armClient = host.Services.GetRequiredService<ARMClient>();
            var subscriptions = await armClient.GetSubscriptionsAsync(armSettings, stopToken: stopToken);

            if (subscriptions == null || !subscriptions.Value.Any())
            {
                Console.WriteLine("No subscriptions found.");
                return 1;
            }

            foreach (var subscription in subscriptions.Value)
            {
                Console.WriteLine($"Subscription information: {subscription.ToString()}");

            }

            return 0;
        }

        public static async Task<int> ListResourcesAsync(IHost host, string armAuthSettingsKey, string subscriptionId, CancellationToken stopToken)
        {
            var optionsMonitor = host.Services.GetRequiredService<IOptionsMonitor<AuthenticationSettings>>();
            var armSettings = optionsMonitor.Get(armAuthSettingsKey);
            Console.WriteLine($"Settings: {armSettings.Audience}, {armSettings.AuthenticationType}");
            var armClient = host.Services.GetRequiredService<ARMClient>();
            var resources = await armClient.GetResourcesForSubscriptionAsync(armSettings, subscriptionId, stopToken: stopToken);
            if (resources == null || !resources.Value.Any())
            {
                Console.WriteLine("No resources found.");
                return 1;
            }
            foreach (var resource in resources.Value)
            {
                Console.WriteLine($"Resource information: {resource.ToString()}");
            }

            return 0;
        }

        public static async Task<int> GetResourcePropertiesAsync(IHost host, string armAuthSettingsKey, string resourceId, CancellationToken stopToken)
        {
            var optionsMonitor = host.Services.GetRequiredService<IOptionsMonitor<AuthenticationSettings>>();
            var armSettings = optionsMonitor.Get(armAuthSettingsKey);
            Console.WriteLine($"Settings: {armSettings.Audience}, {armSettings.AuthenticationType}");
            var armClient = host.Services.GetRequiredService<ARMClient>();
            var resources = await armClient.GetResourcePropertiesAsync(armSettings, resourceId, stopToken: stopToken);
            if (resources == null || !resources.Value.Any())
            {
                Console.WriteLine($"No resource found for id {resourceId}.");
                return 1;
            }
            foreach (var resource in resources.Value)
            {
                Console.WriteLine($"Resource information: {resource.ToString()}");
            }

            return 0;
        }
    }
}
