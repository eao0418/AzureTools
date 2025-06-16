namespace AzureTools.Automation.Messaging
{
    public class GraphObjectMessage
    {
        /// <summary>
        /// The base class for all messages related to Graph object operations in Azure Automation.
        /// </summary>
        public string TenantId { get; set; } = string.Empty;
        public string ExecutionId { get; set; } = string.Empty;
        public string AuthSettingsKey { get; set; } = string.Empty;
        
        public string ODataNextLink { get; set; } = string.Empty;
    }
}
