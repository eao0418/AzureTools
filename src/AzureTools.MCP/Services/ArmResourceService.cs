// ArmResourceService.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.MCP.Services
{
    using AzureTools.Common.Model.Resources;
    using AzureTools.Kusto;
    using global::Kusto.Data.Common;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public sealed class ArmResourceService(
        IKustoReader kustoReader,
        ILogger<ArmResourceService> logger)
    {
        private readonly IKustoReader _kustoReader = kustoReader ?? throw new ArgumentNullException(nameof(KustoReader));
        private readonly ILogger<ArmResourceService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<IEnumerable<Resource>> GetResourcesForTenantAsync(string tenantId, CancellationToken stopToken) 
        {
            var requestProperty = new ClientRequestProperties();
            requestProperty.SetParameter("tenantId", tenantId);

            var result = await kustoReader.ExecuteQueryAsync<Resource>(
                "GraphData",
                $@"declare query_parameters (tenantId: string = '')
                    ;
                    Resource
                        | where RecordGenerateTime > ago(3d)
                            and (isempty(tenantId) or TenantId == tenantId) // ensures that an empty value still returns results
                        | summarize arg_max(RecordGenerateTime, *) by Id",
                Resource.CreateFromReader,
                stopToken,
                requestProperty);
            return result;
        }

        public async Task<IEnumerable<Resource>> GetStorageAccountsWithPublicAccessAsync(CancellationToken stopToken)
        {
            var result = await kustoReader.ExecuteQueryAsync<Resource>(
                "GraphData",
                $@"Resource
                    | where RecordGenerateTime > ago(3d) and Type =~ 'Microsoft.Storage/storageAccounts'
                    | summarize arg_max(RecordGenerateTime, *) by Id    
                    | where tobool(Properties.allowBlobPublicAccess) == true",
                Resource.CreateFromReader,
                stopToken,
                new ClientRequestProperties());
            return result;
        }
    }
}
