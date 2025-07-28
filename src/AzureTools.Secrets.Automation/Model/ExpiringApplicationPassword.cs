namespace AzureTools.Secrets.Automation.Model
{
    using AzureTools.Client.Model.Application;
    using System;
    using System.Data;

    public class ExpiringApplicationPassword
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public long DaysUntilExpiration { get; set; }

        public static ExpiringApplicationPassword CreateFromReader(IDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            return new ExpiringApplicationPassword
            {
                Id = reader["Id"] as string ?? string.Empty,
                DisplayName = reader["DisplayName"] as string ?? string.Empty,
                TenantId = reader["TenantId"] as string ?? string.Empty,
                DaysUntilExpiration = Convert.ToInt64(reader["DaysUntilExpiration"])
            };
        }
    }
}
