// Function1.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.MCP.Triggers
{
    using System;
    using AzureTools.MCP.Services;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
    using Microsoft.Extensions.Logging;


    public sealed class ArmMcpTools(ArmResourceService resourceService, ILogger<ArmMcpTools> logger)
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly ArmResourceService _resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));

        [Function(nameof(GetResourcesForSubScriptionAsync))]
        public async Task<object> GetResourcesForSubScriptionAsync(
            [McpToolTrigger("get_resources_for_subscription", "Returns information about resources in a given subscription")] ToolInvocationContext context,
            [McpToolProperty("subscriptionId", "string", "The id of the subscription to get the resources for.")] string id)
        {
            return await _resourceService.GetResourcesForSubscriptionAsync(id);
        }
    }
}