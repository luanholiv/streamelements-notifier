using System.Text.Json.Serialization;

namespace Api.UseCases.Stores.Search;

public sealed record SearchStoresResponse(
    Guid Id,
    DateTime CreatedAt,
    string ExternalId,
    string StreamerName,
    string Uri
);

[JsonSerializable(typeof(SearchStoresResponse))]
public partial class SearchStoresResponseSerializerContext
    : JsonSerializerContext
{
}