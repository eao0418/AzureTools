// Program.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using AzureTools.Client;
    using AzureTools.Automation.Collector;
    using AzureTools.Repository;
    using AzureTools.Automation.Messaging;
    using AzureTools.Authentication;
    using Microsoft.Extensions.Configuration;
    using AzureTools.Authentication.Settings;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("config/local.config.json", optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
            })
            .ConfigureFunctionsWorkerDefaults()
			.ConfigureServices((context, service) =>
			{
				service.AddHttpClient<GraphClient>(client =>
                {
                    client.BaseAddress = new Uri("https://graph.microsoft.com");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });
                service.Configure<AuthenticationSettings>(context.Configuration.GetSection("authSettings"));
                service.AddSingleton<GraphCollector>();
                service.AddSingleton<IGraphClient, GraphClient>();
                service.AddSingleton<IObjectRepository, LocalFileRepository>();
                service.AddSingleton<IMessageFactory, MessageFactory>();
                service.AddAuthenticationServices();
                service.AddLogging();
            })
            .Build();

            await host.RunAsync();
        }
    }
}