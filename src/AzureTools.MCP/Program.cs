// Program.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.MCP
{
    using Microsoft.Azure.Functions.Worker.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using AzureTools.Kusto;
    using Microsoft.Extensions.Configuration;

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await new HostBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("config/local.config.json", optional: true, reloadOnChange: true);
                config.AddEnvironmentVariables();
            })
            .ConfigureFunctionsWebApplication()
            .
            .ConfigureServices((context, service) =>
            {
                service.AddKustoReader(context.Configuration);
                service.AddLogging();
            })
            .Build().RunAsync();
        }
    }
}