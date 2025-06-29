// GraphGenerator.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Cli.Generation
{
    using AzureTools.Kusto.Utility;
    using global::AzureTools.Client.Model;
    using System;
    using System.Threading.Tasks;

    public static class GraphGenerator
    {
        public static async Task<int> GenerateKustoTables()
        {
            var outputDirectory = Path.Combine(Environment.CurrentDirectory, "resources");
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            try
            {
                await Task.WhenAll(
                    KustoTableGenerator.GenerateKustoTableCreateScript<ApplicationOwner>(outputDirectory),
                    KustoTableGenerator.GenerateKustoTableCreateScript<ApplicationRegistration>(outputDirectory),
                    KustoTableGenerator.GenerateKustoTableCreateScript<DirectoryRole>(outputDirectory),
                    KustoTableGenerator.GenerateKustoTableCreateScript<Group>(outputDirectory),
                    KustoTableGenerator.GenerateKustoTableCreateScript<GroupMember>(outputDirectory),
                    KustoTableGenerator.GenerateKustoTableCreateScript<ServicePrincipal>(outputDirectory),
                    KustoTableGenerator.GenerateKustoTableCreateScript<User>(outputDirectory)
                );
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error generating Kusto tables: {ex.Message}");
                return 1;
            }

            Console.WriteLine($"Generated Kusto Table Create Scripts in directory {outputDirectory}");
            return 0;
        }

        public static async Task<int> GenerateKustoTableMappings()
        {
            var outputDirectory = Path.Combine(Environment.CurrentDirectory, "resources");
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            try
            {
                await Task.WhenAll(
                    KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<ApplicationOwner>(outputDirectory),
                    KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<ApplicationRegistration>(outputDirectory),
                    KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<DirectoryRole>(outputDirectory),
                    KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<Group>(outputDirectory),
                    KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<GroupMember>(outputDirectory),
                    KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<ServicePrincipal>(outputDirectory),
                    KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<User>(outputDirectory)
                );
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error generating Kusto tables: {ex.Message}");
                return 1;
            }

            Console.WriteLine($"Generated Kusto Table ingestion mapping Scripts in directory {outputDirectory}");
            return 0;
        }
    }
}

