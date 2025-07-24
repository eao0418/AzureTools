namespace AzureTools.Client
{
    using System.Text.Json.Serialization;
    using AzureTools.Client.Model;
    using AzureTools.Client.Model.Resources;

    [JsonSerializable(typeof(Subscription))]
    [JsonSerializable(typeof(ARMResponse<Subscription>))]
    [JsonSerializable(typeof(ARMResponse<Tenant>))]
    [JsonSerializable(typeof(Tenant))]
    [JsonSerializable(typeof(Resource))]
    [JsonSerializable(typeof(ARMResponse<Resource>))]
    [JsonSerializable(typeof(Model.Resources.Identity))]
    [JsonSerializable(typeof(Plan))]
    [JsonSerializable(typeof(Sku))]
    [JsonSerializable(typeof(ExtendedLocation))]
    [JsonSerializable(typeof(UserAssignedIdentity))]
    [JsonSerializable(typeof(ProviderResourceType))]
    [JsonSerializable(typeof(Provider))]
    [JsonSerializable(typeof(ModelBase))]
    [JsonSerializable(typeof(User))]
    [JsonSourceGenerationOptions(WriteIndented = true)]
    public partial class SerializableContext : JsonSerializerContext
    {
    }
}
