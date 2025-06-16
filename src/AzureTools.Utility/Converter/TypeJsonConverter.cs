// TypeJsonConverter.cs Copyright (c) Aaron Randolph. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

namespace AzureTools.Utility.Converter
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    /// <summary>
    /// JsonConverter for converting Type objects to and from JSON.
    /// </summary>
    public class TypeJsonConverter : JsonConverter<Type>
    {
        /// <summary>
        /// Reads a JSON string and converts it to a Type object.
        /// </summary>
        /// <param name="reader">The json reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">Json serializer options being used.</param>
        /// <returns>The deserialized type value.</returns>
        /// <exception cref="FormatException">If the value to convert is null or whitespace.</exception>
        /// <exception cref="InvalidOperationException">If the type cannot be converted.</exception>
        public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {

            var typeFullName = reader.GetString();
            if (string.IsNullOrWhiteSpace(typeFullName))
            {
                throw new FormatException("value could not be converted.");
            }

            var convertedType = Type.GetType(typeFullName);

            if (convertedType == null)
            {
                throw new InvalidOperationException($"Type '{typeFullName}' could not be converted.");
            }

            return convertedType;
        }

        /// <summary>
        /// Writes a Type object as a JSON string representation of its assembly qualified name.
        /// </summary>
        /// <param name="writer">The json writer.</param>
        /// <param name="typeValue">The value to convert to a string.</param>
        /// <param name="options">The Json serialization options.</param>
        public override void Write(Utf8JsonWriter writer, Type typeValue, JsonSerializerOptions options)
        {
            writer.WriteStringValue(typeValue.AssemblyQualifiedName);
        }
    }
}
