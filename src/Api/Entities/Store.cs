namespace Api.Entities;

public sealed class Store : Entity
{
    public required string ExternalId { get; set; }
    public required string StreamerName { get; set; }
    public required string Uri { get; set; }
    public IList<StoreItem> AvailableItems { get; set; } = [];
}
