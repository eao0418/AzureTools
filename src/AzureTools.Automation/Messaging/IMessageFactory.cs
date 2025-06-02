namespace AzureTools.Automation.Messaging
{
    using Confluent.Kafka;

    public interface IMessageFactory
    {
        void Dispose();
        IProducer<string, string> GetProducerClient(string topicName);
        Task SendMessageAsync(string topic, string key, string value);
    }
}