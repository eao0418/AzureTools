namespace AzureTools.Automation.Functions
{
    using Microsoft.Azure.Functions.Worker;

    public class KafkaOutputBinding
    {
        [KafkaOutput("%BrokerList%",
            "ObjectEnumeration",
            Protocol = BrokerProtocol.SaslSsl,
            Username = "%KafkaConnection%",
            Password = "%ConnectionString%")]

        public string Kevent { get; set; } = string.Empty;
    }
}
