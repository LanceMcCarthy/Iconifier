using CommonHelpers.Common;
using System.Text.Json.Serialization;

namespace Iconifier.Models;

public class IconDefinition : BindableBase
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }
}