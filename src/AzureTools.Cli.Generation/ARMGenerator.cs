namespace AzureTools.Cli.Generation
{
    using AzureTools.Client.Model.Resources;
    using AzureTools.Kusto.Utility;
    using System;
    using System.Threading.Tasks;

    public static class ARMGenerator
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
                    KustoTableGenerator.GenerateKustoTableCreateScript<Resource>(outputDirectory),
                    KustoTableGenerator.GenerateKustoTableCreateScript<Subscription>(outputDirectory),
                    KustoTableGenerator.GenerateKustoTableCreateScript<Tenant>(outputDirectory)
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
                    KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<Resource>(outputDirectory),
                    KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<Subscription>(outputDirectory),
                    KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<Tenant>(outputDirectory)
                );
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error generating Kusto tables: {ex.Message}");
                return 1;
            }

            Console.WriteLine($"Generated Kusto Table Ingestion Mapping Scripts in directory {outputDirectory}");
            return 0;
        }
    }
}
