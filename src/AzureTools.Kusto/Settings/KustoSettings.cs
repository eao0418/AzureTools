// KustStreamWriter.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Kusto.Settings
{
    using AzureTools.Kusto.Authentication;
    using AzureTools.Utility.Settings;
    using System.Collections.Generic;

    public sealed class KustoSettings: SettingsBase
    {

        public new const string ConfigurationSectionName = "kustoSettings";

        public List<KustoAuthenticationSettings> AuthenticationSettings { get; set; } = new();
    }
}
