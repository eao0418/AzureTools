// GraphMcpTools.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.MCP.Triggers
{
    using System;
    using AzureTools.MCP.Services;
    using AzureTools.Common.Model.Graph;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
    using Microsoft.Extensions.Logging;

    public sealed class GraphMcpTools(GraphObjectService graphObjectService, ILogger<ArmMcpTools> logger)
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly GraphObjectService _graphObjectService = graphObjectService ?? throw new ArgumentNullException(nameof(graphObjectService));

        [Function(nameof(GetDisabledUserAccounts))]
        public async Task<IEnumerable<User>> GetDisabledUserAccounts(
            [McpToolTrigger("get_disabled_user_accounts", "Returns any user accounts that has been disabled.")] ToolInvocationContext context,
            FunctionContext functionContext)
        {
            _logger.LogInformation($"Trigger: {nameof(GetDisabledUserAccounts)} executed.");
            return await _graphObjectService.GetDisabledUserAccounts(functionContext.CancellationToken);
        }
    }
}