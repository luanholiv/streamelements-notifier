using System.Text.Json.Serialization;

namespace Api.UseCases.Stores.Add;

public sealed record class AddStoreRequest(
    string ExternalId,
    string StreamerName,
    string Uri)
{
    public Guid Id = Guid.NewGuid();
}

[JsonSerializable(typeof(AddStoreRequest))]
public partial class AddStoreRequestSerializerContext : JsonSerializerContext { }
