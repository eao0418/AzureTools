// Program.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Cli.Generation
{
    using System.CommandLine;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    public class Program
    {
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
                })
                .Build();

            RootCommand rootCommand = new RootCommand("AzureTools Cli");

            Command generateCommand = new("generate", "Lists the results for the given option.");
            Command kustoCommand = new("kusto", "Kusto generation commands")
            {
                ArmGenerationCommands(),
                GraphGenerationCommands()
            };
            generateCommand.Add(kustoCommand);

            rootCommand.Add(generateCommand);


            return await rootCommand.Parse(args).InvokeAsync();
        }

        private static Command ArmGenerationCommands()
        {
            Command armGenerationCommand = new("arm", "Azure Resource Manager (ARM) generation commands");
            armGenerationCommand.SetAction(async (context) =>
            {
                Console.WriteLine("Generating ARM Kusto scripts.");
                await Task.WhenAll(
                    ARMGenerator.GenerateKustoTables(),
                    ARMGenerator.GenerateKustoTableMappings()
                );
            });

            return armGenerationCommand;
        }

        private static Command GraphGenerationCommands()
        {
            Command graphGenerationCommand = new("graph", "Azure Graph generation commands");
            graphGenerationCommand.SetAction(async (context) =>
            {
                Console.WriteLine("Generating Graph Kusto scripts.");
                await GraphGenerator.GenerateKustoTables();
                await GraphGenerator.GenerateKustoTableMappings();
            });

            return graphGenerationCommand;
        }
    }
}