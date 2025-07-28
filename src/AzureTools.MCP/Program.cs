// Program.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.MCP
{
    using Microsoft.Azure.Functions.Worker.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using AzureTools.Kusto;
    using Microsoft.Extensions.Configuration;
    using AzureTools.MCP.Services;

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = FunctionsApplication.CreateBuilder(args);

            builder.Configuration
                .AddJsonFile("config/local.config.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            builder.ConfigureFunctionsWebApplication()
                .EnableMcpToolMetadata();

            builder.Services
                .AddLogging()
                .AddSingleton<ArmResourceService>()
                .AddSingleton<GraphObjectService>()
                .AddKustoReader(builder.Configuration);

            await builder.Build().RunAsync();
        }
    }
}