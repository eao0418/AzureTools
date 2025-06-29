namespace AzureTools.Automation.Functions.Triggers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Extensions.Logging;
    using AzureTools.Client.Model;
    using AzureTools.Kusto.Utility;

    public class HTTPTriggers
    {
        private readonly ILogger<HTTPTriggers> _logger;

        public HTTPTriggers(ILogger<HTTPTriggers> logger)
        {
            _logger = logger;
        }

        [Function("GenerateGraphKustoTables")]
        public async Task<IActionResult> GenerateGraphKustoTables([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var outputDirectory = Path.Combine(Environment.CurrentDirectory, "resources");

            await Task.WhenAll(
                KustoTableGenerator.GenerateKustoTableCreateScript<ApplicationOwner>(outputDirectory),
                KustoTableGenerator.GenerateKustoTableCreateScript<ApplicationRegistration>(outputDirectory),
                KustoTableGenerator.GenerateKustoTableCreateScript<DirectoryRole>(outputDirectory),
                KustoTableGenerator.GenerateKustoTableCreateScript<Group>(outputDirectory),
                KustoTableGenerator.GenerateKustoTableCreateScript<GroupMember>(outputDirectory),
                KustoTableGenerator.GenerateKustoTableCreateScript<ServicePrincipal>(outputDirectory),
                KustoTableGenerator.GenerateKustoTableCreateScript<User>(outputDirectory)
            );

            return new OkObjectResult("Done");
        }

        [Function("GenerateGraphKustoTableMappings")]
        public async Task<IActionResult> GenerateGraphKustoTableMappings([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var outputDirectory = Path.Combine(Environment.CurrentDirectory, "resources");

            await Task.WhenAll(
                KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<ApplicationOwner>(outputDirectory),
                KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<ApplicationRegistration>(outputDirectory),
                KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<DirectoryRole>(outputDirectory),
                KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<Group>(outputDirectory),
                KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<GroupMember>(outputDirectory),
                KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<ServicePrincipal>(outputDirectory),
                KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<User>(outputDirectory)
            );

            return new OkObjectResult("Done");
        }
    }
}