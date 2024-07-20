using CounterStrikeSharp.API.Core;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace plugin;

public class EventConfig : BasePluginConfig
{
    [JsonPropertyName("Modes")] public JsonObject? Modes { get; set; }
    [JsonPropertyName("Settings")] public JsonObject? Settings { get; set; }
    [JsonPropertyName("MapGroups")] public JsonObject? MapGroups { get; set; }
}
