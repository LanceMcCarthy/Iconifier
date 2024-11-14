using System.Text.Json.Serialization;

namespace Iconifier.Models;

public class ConfigRoot
{
    [JsonPropertyName("icon_definitions")]
    public List<IconDefinition>? IconDefinitions { get; set; }
}