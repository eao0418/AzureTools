namespace AzureTools.Repository.Settings
{
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RepositoryType
    {
        LocalFile,
        Kusto
    }
}
