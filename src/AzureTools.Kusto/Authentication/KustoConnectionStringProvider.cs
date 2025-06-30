// KustoConnectionStringProvider.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Kusto.Authentication
{
    using AzureTools.Authentication.Provider;
    using global::Kusto.Data;

    public sealed class KustoConnectionStringProvider(ITokenCredentialProvider tokenCredentialProvider) : IKustoConnectionStringProvider
    {
        private readonly ITokenCredentialProvider _tokenCredentialProvider = tokenCredentialProvider ?? throw new ArgumentNullException(nameof(tokenCredentialProvider));

        // return connection strings for all of the kusto clusters.

        public KustoConnectionStringBuilder GetConnectionString(KustoAuthenticationSettings authSettings)
        {
            var tokenCredential = _tokenCredentialProvider.GetCredential(authSettings) ??
                throw new Exception($"Failed to get a TokenCredential. Params: AuthenticationType: {authSettings.AuthenticationType}, Tenant: {authSettings.TenantId}, Audience: {authSettings.Audience}");
            var kcsb = new KustoConnectionStringBuilder()
            {
                FederatedSecurity = true,
                InitialCatalog = authSettings.DatabaseName,
                DataSource = authSettings.ClusterUrl,
            }.WithAadAzureTokenCredentialsAuthentication(tokenCredential);

            return kcsb;
        }
    }
}
