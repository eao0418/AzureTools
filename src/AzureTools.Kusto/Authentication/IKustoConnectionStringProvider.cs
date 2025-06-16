// KustStreamWriter.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Kusto.Authentication
{
    public interface IKustoConnectionStringProvider
    {
        global::Kusto.Data.KustoConnectionStringBuilder GetConnectionString(KustoAuthenticationSettings authSettings);
    }
}