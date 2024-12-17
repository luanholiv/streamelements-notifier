namespace Api.Database.Stores;

internal static class SqlQueries
{
    internal static class Store
    {
        public const string AddStore =
            @"INSERT INTO stores 
            VALUES (@Id, @CreatedAt, @ExternalId, @StreamerName, @Uri);";

        public const string GetAllStores =
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
        
        public const string GetPagedStores =
            @"SELECT 
                id AS Id, 
                created_at AS CreatedAt, 
                external_id AS ExternalId, 
                streamer_name AS StreamerName, 
                uri AS Uri
            FROM stores
            ORDER BY streamer_name
            LIMIT @Limit
            OFFSET @Offset;";
        
        public const string CountStores = "SELECT COUNT(id) FROM stores;";

        public const string GetIdByExternalId =
            "SELECT id AS Id FROM stores WHERE external_id = @ExternalId;";

        public const string DeleteStore =
            @"DELETE FROM stores WHERE id = @Id;";
    }

    internal static class StoreItems
    {
        public const string AddStoreItems =
            @"INSERT INTO store_items 
            VALUES (@Id, @CreatedAt, @ExternalId, @StoreId, @Name, @Cost);";

        public const string DeleteStoreItems =
            @"DELETE FROM store_items WHERE id = ANY(@Ids);";
    }
}
