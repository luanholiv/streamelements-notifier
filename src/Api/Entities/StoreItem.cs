namespace Api.Entities;

public sealed class StoreItem : Entity
{
    public Guid StoreId { get; set; }
    public required string ExternalId { get; set; }
    public required string Name { get; set; }
    public int Cost { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not StoreItem)
            return false;

        var other = (StoreItem)obj;

        return
            ExternalId == other.ExternalId &&
            StoreId == other.StoreId;
    }

    public override int GetHashCode() =>
        HashCode.Combine(ExternalId, StoreId);

    public override string ToString() =>
        $"Item: {Name}. Valor: {Cost} pontos";
}