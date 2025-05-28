// Program.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools
{
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
    using AzureTools.Client;

    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults()
			.ConfigureServices(service =>
			{
				service.AddHttpClient<GraphClient>(client =>
                {
                    client.BaseAddress = new Uri("https://graph.microsoft.com/");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });
                service.AddHttpClient<GraphClient>();
            })
            .Build();

            await host.RunAsync();
        }
    }
}