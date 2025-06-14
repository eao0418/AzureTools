// KustStreamWriter.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Kusto.Authentication
{
    using AzureTools.Authentication.Provider;
    using global::Kusto.Data;

    public sealed class KustoConnectionStringProvider : IKustoConnectionStringProvider
    {
        private readonly ITokenCredentialProvider _tokenCredentialProvider;

        public KustoConnectionStringProvider(ITokenCredentialProvider tokenCredentialProvider)
        {
            _tokenCredentialProvider = tokenCredentialProvider ?? throw new ArgumentNullException(nameof(tokenCredentialProvider));
        }

        // return connection strings for all of the kusto clusters.

        public KustoConnectionStringBuilder GetConnectionString(KustoAuthenticationSettings authSettings)
        {
            var tokenCredential = _tokenCredentialProvider.GetCredential(authSettings);

            if (tokenCredential == null)
            {
                throw new ArgumentNullException(nameof(tokenCredential), "Token credential cannot be null.");
            }

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
