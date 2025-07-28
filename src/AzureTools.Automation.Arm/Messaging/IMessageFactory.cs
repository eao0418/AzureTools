// IMessageFactory.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Automation.Arm.Messaging
{
    using System.Threading.Tasks;

    public interface IMessageFactory
    {
        /// <summary>
        /// Sends a message to the specified topic with the given key and value.
        /// </summary>
        /// <param name="topic">The name of the topic to send the message to.</param>
        /// <param name="key">The key to send.</param>
        /// <param name="value">The value to send. This can be a JSON-formatted string.</param>
        /// <returns>A <see cref="Task"/> representing the result of an asynchronous operation.</returns>
        public Task SendMessageAsync(string topic, string key, string value);

        /// <summary>
        /// Sends a message to the specified topic with the given key and value.
        /// </summary>
        /// <typeparam name="T">The object to send in the queue. It must be able to be serialized to JSON.</typeparam>
        /// <param name="topic">The name of the topic to send the message to.</param>
        /// <param name="key">The key to send.</param>
        /// <param name="value">The value to send.</param>
        /// <returns>A <see cref="Task"/> representing the result of an asynchronous operation.</returns>
        public Task SendMessageAsync<T>(string topic, string key, T value);
    }
}