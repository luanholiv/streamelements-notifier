using System.Text.Json.Serialization;

namespace Api.Entities;

public sealed record class ExternalStoreItem(
    Bot Bot,
    Cooldown Cooldown,
    Quantity Quantity,
    AccessCodes AccessCodes,
    Alert Alert,
    bool SubscriberOnly,
    IEnumerable<string> UserInput,
    bool Enabled,
    bool Featured,
    string Name,
    string Description,
    string Type,
    int Cost,
    bool Public,
    string Channel)
{
    [JsonPropertyName("_id")]
    public required string Id { get; set; }

    public bool IsAvailable =>
        Enabled && (Quantity.IsPositive || Quantity.IsInfinite);
}

[JsonSerializable(typeof(ExternalStoreItem))]
public partial class ExternalStoreItemContext : JsonSerializerContext { }

public sealed record class Bot(bool Enabled, string Identifier);

[JsonSerializable(typeof(Bot))]
public partial class BotContext : JsonSerializerContext { }

public sealed record class Cooldown(int User, int Global);

[JsonSerializable(typeof(Cooldown))]
public partial class CooldownContext : JsonSerializerContext { }

public sealed record class Quantity(int Total, int Current)
{
    public bool IsPositive => Current > 0;

    public bool IsInfinite => Current == -1;
};

[JsonSerializable(typeof(Quantity))]
public partial class QuantityContext : JsonSerializerContext { }

public sealed record class AccessCodes(IEnumerable<string> Keys, string Mode, bool Random);

[JsonSerializable(typeof(AccessCodes))]
public partial class AccessCodesContext : JsonSerializerContext { }

public sealed record class Alert(Graphics Graphics, Audio Audio, bool Enabled);

[JsonSerializable(typeof(Alert))]
public partial class AlertContext : JsonSerializerContext { }

public sealed record class Graphics(int Duration);

[JsonSerializable(typeof(Graphics))]
public partial class GraphicsContext : JsonSerializerContext { }

public sealed record class Audio(string? Src, float Volume);

[JsonSerializable(typeof(Audio))]
public partial class AudioContext : JsonSerializerContext { }