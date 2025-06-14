namespace AzureTools.Automation.Messaging
{
    public class AppRegistrationOwnerRequest: GraphObjectMessage
    {
        public string AppId { get; set; } = string.Empty;
    }
}
