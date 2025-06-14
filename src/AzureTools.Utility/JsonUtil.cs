// JsonUtil.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Utility
{
    using AzureTools.Utility.Converter;
    using System.Text.Json;
    public static class JsonUtil
    {
        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = {
                new System.Text.Json.Serialization.JsonStringEnumConverter(),
                new TypeJsonConverter()
            },
        };

        /// <summary>
        /// Serializes an object to a JSON string using the specified options.
        /// </summary>
        /// <typeparam name="T">The Type.</typeparam>
        /// <param name="value">The class instance to serialize.</param>
        /// <returns>The JSON string.</returns>
        public static string Serialize<T>(T value) => JsonSerializer.Serialize(value, _serializerOptions);

        public static async Task SerializeAsync<T>(T value, Stream stream, CancellationToken stopToken) => await JsonSerializer.SerializeAsync(
            stream,
            value,
            options: _serializerOptions,
            cancellationToken: stopToken);

        /// <summary>
        /// Deserializes a JSON string to an object of type T using the specified options.
        /// </summary>
        /// <typeparam name="T">The Type of object</typeparam>
        /// <param name="json">The JSON string.</param>
        /// <returns>The deserialized object.</returns>
        public static T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, _serializerOptions);

    }
}
