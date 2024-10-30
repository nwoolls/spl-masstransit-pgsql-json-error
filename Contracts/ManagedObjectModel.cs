using System.Text.Json;
using System.Text.Json.Serialization;

namespace spl_masstransit_pgsql_json_error.Contracts;

internal sealed class ManagedObjectModel
{
    [JsonPropertyName("foo")]
    public string? Foo { get; set; }

    [JsonExtensionData]
    public IDictionary<string, JsonElement> ExtensionData { get; init; } = new Dictionary<string, JsonElement>();
}