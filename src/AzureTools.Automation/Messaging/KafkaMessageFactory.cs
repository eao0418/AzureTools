// KafkaMessageFactory.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Automation.Messaging
{
    using Confluent.Kafka;
    using System.Collections.Concurrent;

    public sealed class KafkaMessageFactory : IDisposable, IMessageFactory
    {
        private readonly ConcurrentDictionary<string, IProducer<string, string>> _producers = new();

        public KafkaMessageFactory()
        {
            Init();
        }

        private void Init()
        {
            var bootstrapServers = Environment.GetEnvironmentVariable("BrokerList");

            // TO-DO: make this cleaner and built off of configuration.
            var producer = new ProducerBuilder<string, string>(
                new ProducerConfig
                {
                    BootstrapServers = bootstrapServers,
                    SecurityProtocol = SecurityProtocol.Plaintext,
                    SaslUsername = "",
                    SaslPassword = ""
                }).Build();

            _producers.TryAdd(MessageTopics.ObjectEnumerationTopic, producer);

            var groupProducer = new ProducerBuilder<string, string>(
                new ProducerConfig
                {
                    BootstrapServers = bootstrapServers,
                    SecurityProtocol = SecurityProtocol.Plaintext,
                    SaslUsername = "",
                    SaslPassword = ""
                }).Build();

            _producers.TryAdd(MessageTopics.GroupMembershipTopic, groupProducer);

            var appOwnerProducer = new ProducerBuilder<string, string>(
                new ProducerConfig
                {
                    BootstrapServers = bootstrapServers,
                    SecurityProtocol = SecurityProtocol.Plaintext,
                    SaslUsername = "",
                    SaslPassword = ""
                }).Build();

            _producers.TryAdd(MessageTopics.ApplicationRegistrationOwnersTopic, appOwnerProducer);

            var resourcePropertiesProducer = new ProducerBuilder<string, string>(
                new ProducerConfig
                {
                    BootstrapServers = bootstrapServers,
                    SecurityProtocol = SecurityProtocol.Plaintext,
                    SaslUsername = "",
                    SaslPassword = ""
                }).Build();

            _producers.TryAdd(MessageTopics.ResourcePropertiesTopic, resourcePropertiesProducer);

            var subscriptionProducer = new ProducerBuilder<string, string>(
                new ProducerConfig
                {
                    BootstrapServers = bootstrapServers,
                    SecurityProtocol = SecurityProtocol.Plaintext,
                    SaslUsername = "",
                    SaslPassword = ""
                }).Build();

            _producers.TryAdd(MessageTopics.SubscriptionEnumerationTopic, subscriptionProducer);
        }

        public void Dispose()
        {
            foreach (var producer in _producers.Values)
            {
                producer.Dispose();
            }
            _producers.Clear();
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task SendMessageAsync<T>(string topic, string key, T value)
        {
            if (_producers.TryGetValue(topic, out var producer))
            {
                var jsonValue = JsonUtil.Serialize(value);

                await producer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = jsonValue });
            }
            else
            {
                throw new InvalidOperationException($"Producer for topic '{topic}' not found.");
            }

        }
    }
}
