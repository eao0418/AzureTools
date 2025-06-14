namespace AzureTools.Automation.Messaging
{
    /// <summary>
    /// Represents a message for group membership operations in Azure Automation.
    /// </summary>
    public class GroupMembershipMessage : GraphObjectMessage
    {
        public string GroupId { get; set; } = string.Empty;
    }
}
