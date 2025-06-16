

namespace AzureTools.Secrets.Settings
{
    using AzureTools.Authentication.Settings;
    using AzureTools.Utility.Settings;
    using System;

    public class SecretProviderSettings : SettingsBase
    {
        public new const string ConfigurationSectionName = "secretProviderSettings";

        public Uri? KeyVaultUri { get; init; } = null;

        public AuthenticationSettings? AuthenticationSettings { get; init; } = null;
    }
}
