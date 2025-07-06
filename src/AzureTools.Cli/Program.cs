// Program.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Cli
{
    using System.CommandLine;
    using AzureTools.Client;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using AzureTools.Authentication.Settings;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using AzureTools.Authentication;

    public class Program
    {
        private const string ARMAuthSettingsKey = "armAuthenticationSettings";
        private const string GraphAuthSettingsKey = "graphAuthenticationSettings";
        private static readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        [RequiresUnreferencedCode("Calls Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions.Configure<TOptions>(IConfiguration)")]
        [RequiresDynamicCode("Calls Microsoft.Extensions.DependencyInjection.OptionsConfigurationServiceCollectionExtensions.Configure<TOptions>(IConfiguration)")]
        public static async Task<int> Main(string[] args)
        {
            var subscriptionidOption = new Option<string>("--subscriptionid")
            {
                Description = "The subscription ID to use for the command.",
                Required = false,
                Aliases = { "-s" }
            };

            var resourceProviderOption = new Option<string>("--resource-provider")
            {
                Description = "The resource provider to use for the command.",
                Required = false,
                Aliases = { "-rp" }
            };

            var resourceIdOption = new Option<string>("--resourceid")
            {
                Description = "The resource ID to use for the command.",
                Required = false,
                Aliases = { "-r" }
            };

            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("config.local.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<AuthenticationSettings>(ARMAuthSettingsKey, context.Configuration.GetSection(ARMAuthSettingsKey));
                    services.Configure<AuthenticationSettings>(GraphAuthSettingsKey, context.Configuration.GetSection(GraphAuthSettingsKey));
                    services.AddAuthenticationServices();
                    services.AddHttpClient();
                    services.AddSingleton<ARMClient>();
                    services.AddSingleton<IGraphClient, GraphClient>();
                })
                .Build();

            RootCommand rootCommand = new RootCommand("AzureTools Cli");

            #region ARM List Commands
            Command listCommand = new("list", "Lists the results for the given option.")
            {
            };

            var listTenantsCommand = new Command("tenants", "List all tenants");
            listTenantsCommand.SetAction(async => ARMClientAccessor.ListTenants(host, ARMAuthSettingsKey, _cancellationTokenSource.Token));
            listCommand.Add(listTenantsCommand);

            var listSubscriptionsCommand = new Command("subscriptions", "List all subscriptions");
            listSubscriptionsCommand.SetAction(async => ARMClientAccessor.ListSubscriptions(host, ARMAuthSettingsKey, _cancellationTokenSource.Token));
            listCommand.Add(listSubscriptionsCommand);

            var listResourcesCommand = new Command("resources", "List all resources in a subscription");
            listResourcesCommand.SetAction((ParseResult parseResult, CancellationToken stopToken) =>
            {
                string? subscriptionId = parseResult.GetValue<string>("--subscriptionid") ?? string.Empty;
                return ARMClientAccessor.ListResourcesAsync(host, ARMAuthSettingsKey, subscriptionId, stopToken);
            });
            listResourcesCommand.Add(subscriptionidOption);
            listCommand.Add(listResourcesCommand);

            var listResourceVersionsCommand = new Command("resource-versions", "List all versions for a provider in a subscription");
            listResourceVersionsCommand.SetAction((ParseResult parseResult, CancellationToken stopToken) =>
            {
                string? subscriptionId = parseResult.GetValue<string>("--subscriptionid") ?? string.Empty;
                string? resourceProvider = parseResult.GetValue<string>("--resource-provider") ?? string.Empty;
                return ARMClientAccessor.GetApiVersionForResourceAsync(host, ARMAuthSettingsKey, subscriptionId, resourceProvider, stopToken);
            });
            listResourceVersionsCommand.Add(subscriptionidOption);
            listResourceVersionsCommand.Add(resourceProviderOption);
            listCommand.Add(listResourceVersionsCommand);

            var listResourcePropertiesCommand = new Command("resource-properties", "List properties of a specific resource");
            listResourcePropertiesCommand.SetAction((ParseResult ParseResult, CancellationToken stopToken) =>
            {
                string? resourceId = ParseResult.GetValue<string>("--resourceid") ?? string.Empty;
                return ARMClientAccessor.GetResourcePropertiesAsync(host, ARMAuthSettingsKey, resourceId, stopToken);
            });
            listResourcePropertiesCommand.Add(resourceIdOption);
            listCommand.Add(listResourcePropertiesCommand);
            #endregion

            rootCommand.Add(listCommand);


            return await rootCommand.Parse(args).InvokeAsync();
        }
    }
}