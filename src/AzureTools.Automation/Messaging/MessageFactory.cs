namespace AzureTools.Automation.Messaging
{
    using Confluent.Kafka;
    using System.Collections.Concurrent;

    public class MessageFactory : IDisposable, IMessageFactory
    {
        private ConcurrentDictionary<string, IProducer<string, string>> _producers = new();

        public MessageFactory()
        {
            Init();
        }

        private void Init()
        {
            var bootstrapServers = Environment.GetEnvironmentVariable("BrokerList");

            var producer = new ProducerBuilder<string, string>(
                new ProducerConfig
                {
                    BootstrapServers = bootstrapServers,
                    SecurityProtocol = SecurityProtocol.Plaintext,
                    SaslUsername = "",
                    SaslPassword = ""
                }).Build();

            _producers.TryAdd("ObjectEnumeration", producer);

            var groupProducer = new ProducerBuilder<string, string>(
                new ProducerConfig
                {
                    BootstrapServers = bootstrapServers,
                    SecurityProtocol = SecurityProtocol.Plaintext,
                    SaslUsername = "",
                    SaslPassword = ""
                }).Build();

            _producers.TryAdd("GroupMemberEnumeration", groupProducer);

            var appOwnerProducer = new ProducerBuilder<string, string>(
                new ProducerConfig
                {
                    BootstrapServers = bootstrapServers,
                    SecurityProtocol = SecurityProtocol.Plaintext,
                    SaslUsername = "",
                    SaslPassword = ""
                }).Build();

            _producers.TryAdd("AppOwnerEnumeration", appOwnerProducer);
        }

        public IProducer<string, string> GetProducerClient(string topicName)
        {
            return _producers.TryGetValue(topicName, out var producer)
                ? producer
                : throw new InvalidOperationException($"Producer for topic '{topicName}' not found.");
        }

        public void Dispose()
        {
            foreach (var producer in _producers.Values)
            {
                producer.Dispose();
            }
            _producers.Clear();
        }

        public async Task SendMessageAsync(string topic, string key, string value)
        {
            if (_producers.TryGetValue(topic, out var producer))
            {
                await producer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = value });
            }
            else
            {
                throw new InvalidOperationException($"Producer for topic '{topic}' not found.");
            }

        }
    }
}
