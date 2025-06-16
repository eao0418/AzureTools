namespace AzureTools.Secrets.Automation.Model
{
    using AzureTools.Client.Model.Application;
    using System;

    public class ExpiringApplicationPassword
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public long DaysUntilExpiration { get; set; }
    }
}
