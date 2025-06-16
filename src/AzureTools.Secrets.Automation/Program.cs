using AzureTools.Client;
using AzureTools.Repository;
using AzureTools.Secrets.Automation;
using AzureTools.Secrets.Automation.Service;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
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
            service.Configure<SecretAutomationSettings>(context.Configuration.GetSection(SecretAutomationSettings.ConfigurationSectionName));
            service.AddGraphClient(context.Configuration);
            service.AddKustoReader(context.Configuration);
            service.AddLogging();
            service.AddSingleton<ApplicationCredentialManager>();
        })
        .Build();

        await host.RunAsync();
    }
}