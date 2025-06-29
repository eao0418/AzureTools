// KustoAuthenticationSettings.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Kusto.Authentication
{
    using AzureTools.Authentication.Settings;

    public sealed class KustoAuthenticationSettings : AuthenticationSettings
    {
        public new const string ConfigurationSectionName = "kustoAuthenticationSettings";

        public required string DatabaseName { get; set; }
        public required string ClusterUrl { get; set; }
    }
}
