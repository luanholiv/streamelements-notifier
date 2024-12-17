using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.UseCases;

public record PagedResponse<T>(int TotalRecords, IEnumerable<T> Data);

[JsonSerializable(typeof(PagedResponse<>))]
[JsonSerializable(typeof(PagedResponse<object>))]
public partial class PagedResponseSerializerContext : JsonSerializerContext
{
    protected new JsonSerializerOptions Options { get; } = new ()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public static PagedResponseSerializerContext Context => new ();

    public static string Serialize<T>(PagedResponse<T> response)
    {
        return JsonSerializer.Serialize(response, typeof(PagedResponse<T>), Context);
    }

    public static PagedResponse<T>? Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<PagedResponse<T>>(json);
    }
}