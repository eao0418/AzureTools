// GraphObjectService.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.MCP.Services
{
    using AzureTools.Kusto;
    using global::Kusto.Data.Common;
    using AzureTools.Common.Model.Graph;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public sealed class GraphObjectService(
        IKustoReader kustoReader,
        ILogger<GraphObjectService> logger)
    {
        private readonly ILogger<GraphObjectService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IKustoReader _kustoReader = kustoReader ?? throw new ArgumentNullException(nameof(kustoReader));

        public async Task<IEnumerable<User>> GetDisabledUserAccounts(CancellationToken stopToken)
        {
            string query = @"User
                | where RecordGenerateTime > ago(1d) and AccountEnabled == false
                | summarize arg_max(RecordGenerateTime, *) by Id;
                ";
                
            var result = await _kustoReader.ExecuteQueryAsync<User>("GraphData", query, User.CreateFromReader, stopToken, new ClientRequestProperties());
            return result;
        }

//        public async Task
    }
}
