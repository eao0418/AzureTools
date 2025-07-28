// IKustoReader.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Kusto
{
    using global::Kusto.Data.Common;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IKustoReader
    {
        Task<IEnumerable<T>> ExecuteQueryAsync<T>(string databaseName, string query, Func<IDataReader, T> mapper, CancellationToken stopToken, ClientRequestProperties requestProperties) where T : class, new();
    }
}