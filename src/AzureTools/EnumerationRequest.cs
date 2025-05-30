namespace AzureTools.Automation
{
    using System;

    public class EnumerationRequest
    {
        public string Url { get; set; } = string.Empty;
        public string SettingKey { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string executionId { get; set; } = string.Empty;
        public Type? ObjectType { get; set; } = null;
    }
}
