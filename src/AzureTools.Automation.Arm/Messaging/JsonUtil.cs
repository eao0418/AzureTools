namespace AzureTools.Automation.Arm.Messaging
{
    using System.Text.Json;
    public static class JsonUtil
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        /// <summary>
        /// Serializes an object to a JSON string using the specified options.
        /// </summary>
        /// <typeparam name="T">The Type.</typeparam>
        /// <param name="value">The class instance to serialize.</param>
        /// <returns>The JSON string.</returns>
        public static string Serialize<T>(T value) => JsonSerializer.Serialize(value, _options);

        /// <summary>
        /// Deserializes a JSON string to an object of type T using the specified options.
        /// </summary>
        /// <typeparam name="T">The Type of object</typeparam>
        /// <param name="json">The JSON string.</param>
        /// <returns>The deserialized object.</returns>
        public static T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, _options);
    }
}
