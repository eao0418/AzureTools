// KustoTableAutomater.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Kusto.Utility
{
    using System;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class KustoTableGenerator
    {
        public static string GenerateKustoTableCreateScript<T>()
        {
            var type = typeof(T);
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($".create-merge table {type.Name} (");

            var properties = type.GetProperties();
            foreach (var prop in properties)
            {
                string kustoType = prop.PropertyType.Name switch
                {
                    "Int32" => "int",
                    "Int64" => "long",
                    "Double" => "real",
                    "Decimal" => "decimal",
                    "TimeSpan" => "timespan",
                    "String" => "string",
                    "DateTime" => "datetime",
                    "Boolean" => "bool",
                    _ => "dynamic" // Default to dynamic for other types.
                };

                stringBuilder.AppendLine($"    {prop.Name}: {kustoType},");
            }

            stringBuilder.Remove(stringBuilder.Length - 3, 1); // Remove the trailing comma
            stringBuilder.AppendLine(")");

            return stringBuilder.ToString();
        }

        public static async Task GenerateKustoTableCreateScript<T>(string outputDirectory)
        {
            var fileName = $"{outputDirectory}/{typeof(T).Name}.kql";
            var commandText = GenerateKustoTableCreateScript<T>();

            if (System.IO.Directory.Exists(outputDirectory) is false)
            {
                outputDirectory = Environment.CurrentDirectory;
            }

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            var filePath = System.IO.Path.Combine(outputDirectory, fileName);

            using (var writer = new System.IO.StreamWriter(filePath))
            {
                await writer.WriteAsync(commandText);
            }
        }

        public static string GenerateKustoTableIngestionMappingScriptAsync<T>()
        {
            var type = typeof(T);
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($".create-or-alter table {type.Name} ingestion json mapping '{type.Name}Mapping'");
            stringBuilder.AppendLine("```");
            stringBuilder.AppendLine("[");
           
            var properties = type.GetProperties();
            foreach (var prop in properties)
            {
               var camelCaseName = System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(prop.Name);

                stringBuilder.AppendLine($"   {{ \"column\":\"{prop.Name}\",\"Properties\":{{ \"path\":\"$.{camelCaseName}\"}} }},");
            }

            stringBuilder.Remove(stringBuilder.Length - 3, 1); // Remove the trailing comma
            stringBuilder.AppendLine("]");
            stringBuilder.AppendLine("```");

            return stringBuilder.ToString();
        }

        public static async Task GenerateKustoTableIngestionMappingScriptAsync<T>(string outputDirectory)
        {
            var fileName = $"{outputDirectory}/{typeof(T).Name}IngestionMapping.kql";
            var commandText = GenerateKustoTableIngestionMappingScriptAsync<T>();

            if (System.IO.Directory.Exists(outputDirectory) is false)
            {
                outputDirectory = Environment.CurrentDirectory;
            }

            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            var filePath = System.IO.Path.Combine(outputDirectory, fileName);

            using (var writer = new System.IO.StreamWriter(filePath))
            {
                await writer.WriteAsync(commandText);
            }
        }
    }
}
