namespace AzureTools.Automation.Messaging
{
    using AzureTools.Utility.Converter;
    using System;
    using System.Text.Json.Serialization;

    public class EnumerationRequest: GraphObjectMessage
    {
        [JsonConverter(typeof(TypeJsonConverter))]
        public Type? ObjectType { get; set; } = null;
    }
}
