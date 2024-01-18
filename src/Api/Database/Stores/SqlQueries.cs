namespace Api.Database.Stores;

public static class SqlQueries
{
    public const string ADD =
        "INSERT INTO stores VALUES (@Id, @CreatedAt, @ExternalId, @StreamerName, @Uri);";

    public const string ADD_ITEMS =
        "INSERT INTO store_items VALUES (@Id, @CreatedAt, @ExternalId, @StoreId, @Name, @Cost);";

    public const string DELETE_ITEMS = "DELETE FROM store_items WHERE id = ANY(@Ids)";

    public const string GET_ALL =
        @"SELECT 
            stores.id AS Id, 
            stores.created_at AS CreatedAt, 
            stores.external_id AS ExternalId, 
            stores.streamer_name AS StreamerName, 
            stores.uri AS Uri, 
            store_items.id AS Id, 
            store_items.created_at AS CreatedAt, 
            store_items.external_id AS ExternalId,
            store_items.store_id AS StoreId, 
            store_items.name AS Name, 
            store_items.cost AS Cost
        FROM stores LEFT JOIN store_items ON stores.Id = store_items.store_id
        ORDER BY stores.streamer_name;";

    public const string GET_ID_BY_EXTERNAL_ID =
        "SELECT id AS Id FROM stores WHERE external_id = @ExternalId";
}
