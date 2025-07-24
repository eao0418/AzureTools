namespace AzureTools.Automation.Messaging
{
    /// <summary>
    /// The base class for all messages related to Graph object operations in Azure Automation.
    /// </summary>
    public class GraphObjectMessage : ObjectMessage
    {
        public string ODataNextLink { get; set; } = string.Empty;
    }
}
