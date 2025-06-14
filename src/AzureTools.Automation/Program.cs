// Program.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools
{
    using AzureTools.Automation.Collector;
    using AzureTools.Automation.Messaging;
    using AzureTools.Client;
    using AzureTools.Repository;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

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
            .ConfigureFunctionsWebApplication()
			.ConfigureServices((context, service) =>
			{
                service.AddGraphClient(context.Configuration);
                service.AddSingleton<GraphCollector>();
                service.AddKustoRepository(context.Configuration);
                service.AddSingleton<IMessageFactory, KafkaMessageFactory>();
                service.AddLogging();
            })
            .Build();

            await host.RunAsync();
        }
    }
}