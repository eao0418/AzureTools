// ArmResourceService.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.MCP.Services
{
    using AzureTools.Kusto;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class ArmResourceService(
        IKustoReader kustoReader,
        ILogger<ArmResourceService> logger)
    {
        private readonly IKustoReader _kustoReader = kustoReader ?? throw new ArgumentNullException(nameof(KustoReader));
        private readonly ILogger<ArmResourceService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<object> GetResourcesForSubscriptionAsync(string subscriptionId) 
        {
            var result = await kustoReader.ExecuteQueryAsync<Dictionary<string, object>>(
                "GraphData",
                $"Resources | where SubscriptionId == '{subscriptionId}' | project ResourceId, ResourceName, ResourceType, Location",
                default);
            return result;
        }
    }
}
