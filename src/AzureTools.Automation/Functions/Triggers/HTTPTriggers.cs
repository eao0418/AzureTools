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

            await KustoTableGenerator.GenerateKustoTableCreateScript<ApplicationOwner>(outputDirectory);
            await KustoTableGenerator.GenerateKustoTableCreateScript<ApplicationRegistration>(outputDirectory);
            await KustoTableGenerator.GenerateKustoTableCreateScript<DirectoryRole>(outputDirectory);
            await KustoTableGenerator.GenerateKustoTableCreateScript<Group>(outputDirectory);
            await KustoTableGenerator.GenerateKustoTableCreateScript<GroupMember>(outputDirectory);
            await KustoTableGenerator.GenerateKustoTableCreateScript<ServicePrincipal>(outputDirectory);
            await KustoTableGenerator.GenerateKustoTableCreateScript<User>(outputDirectory);

            return new OkObjectResult("Done");
        }

        [Function("GenerateGraphKustoTableMappings")]
        public async Task<IActionResult> GenerateGraphKustoTableMappings([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var outputDirectory = Path.Combine(Environment.CurrentDirectory, "resources");

            await KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<ApplicationOwner>(outputDirectory);
            await KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<ApplicationRegistration>(outputDirectory);
            await KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<DirectoryRole>(outputDirectory);
            await KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<Group>(outputDirectory);
            await KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<GroupMember>(outputDirectory);
            await KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<ServicePrincipal>(outputDirectory);
            await KustoTableGenerator.GenerateKustoTableIngestionMappingScriptAsync<User>(outputDirectory);

            return new OkObjectResult("Done");
        }
    }
}