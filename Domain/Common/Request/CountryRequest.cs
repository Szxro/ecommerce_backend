using System.Text.Json.Serialization;

namespace Domain.Common.Request;

public partial class CountryRequest
{
    [JsonPropertyName("name")]
    public Name Name { get; set; } = new();
}

public partial class Name
{
    [JsonPropertyName("common")]
    public string Common { get; set; } = string.Empty;

    [JsonPropertyName("official")]
    public string Official { get; set; } = string.Empty;

    [JsonPropertyName("nativeName")]
    public Dictionary<string, NativeName> NativeName { get; set; } = new();
}

public partial class NativeName
{
    [JsonPropertyName("official")]
    public string Official { get; set; } = string.Empty;

    [JsonPropertyName("common")]
    public string Common { get; set; } = string.Empty;
}