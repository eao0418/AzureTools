

namespace AzureTools.Secrets.Automation
{
    using AzureTools.Authentication.Settings;
    using AzureTools.Utility.Settings;

    public class SecretAutomationSettings: SettingsBase
    {
        public new const string ConfigurationSectionName = "secretAutomationSettings";

        public AuthenticationSettings? AuthenticationSettings { get; set; }

        public string DataSourceName { get; set; } = string.Empty;
    }
}
