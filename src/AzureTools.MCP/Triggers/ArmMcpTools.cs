// ArmMcpTools.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.MCP.Triggers
{
    using System;
    using AzureTools.MCP.Common;
    using AzureTools.MCP.Services;
    using Microsoft.Azure.Functions.Worker;
    using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
    using Microsoft.Extensions.Logging;
    using AzureTools.Common.Model.Resources;

    public sealed class ArmMcpTools(ArmResourceService resourceService, ILogger<ArmMcpTools> logger)
    {
        private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly ArmResourceService _resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));

        [Function(nameof(GetResourcesForTenantAsync))]
        public async Task<IEnumerable<Resource>> GetResourcesForTenantAsync(
            [McpToolTrigger("get_resources_for_tenant", "Returns information about resources in a given tenant. If no tenant is specified, all tenant resources are returned.")] ToolInvocationContext context,
            [McpToolProperty("tenantId", McpToolPropertyType.StringType, "The id of the tenant to get the resources for.")] string id,
            FunctionContext functionContext)
        {
            _logger.LogInformation($"Trigger: {nameof(GetResourcesForTenantAsync)} executed for tenant: {id}");
            var result = await _resourceService.GetResourcesForTenantAsync(id, functionContext.CancellationToken);
            return result;
        }

        [Function(nameof(GetStorageAccountsWithPublicAccessAsync))]
        public async Task<IEnumerable<Resource>> GetStorageAccountsWithPublicAccessAsync(
            [McpToolTrigger("get_public_storage", "Returns any storage accounts that allow unauthenticated access")] ToolInvocationContext context,
            FunctionContext functionContext)
        {
            _logger.LogInformation($"Trigger: {nameof(GetStorageAccountsWithPublicAccessAsync)} executed");
            return await _resourceService.GetStorageAccountsWithPublicAccessAsync(functionContext.CancellationToken);
        }
    }
}