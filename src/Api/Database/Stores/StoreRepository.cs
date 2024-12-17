using Api.Entities;
using Api.UseCases;
using Api.UseCases.Stores.Search;
using Dapper;
using Npgsql;
using static Api.Database.Stores.SqlQueries.Store;
using static Api.Database.Stores.SqlQueries.StoreItems;

namespace Api.Database.Stores;

public sealed class StoreRepository(NpgsqlConnection connection)
    : IStoreRepository
{
    #region Store

    public async Task<bool> AddAsync(Store store)
    {
        var affectedRows = await connection.ExecuteAsync(AddStore, store);
        return affectedRows > 0;
    }

    public async Task<IEnumerable<Store>> GetAsync()
    {
        var lookup = new Dictionary<Guid, Store>();

        _ = await connection.QueryAsync<Store, StoreItem, Store>(
            GetAllStores,
            splitOn: "Id",
            map: (store, storeItem) =>
            {
                if (!lookup.TryGetValue(store.Id, out var storeEntry))
                {
                    storeEntry = store;
                    lookup.Add(storeEntry.Id, storeEntry);
                }

                storeEntry.AvailableItems.Add(storeItem);

                return storeEntry;
            });

        return lookup.Values;
    }

    public async Task<PagedResponse<SearchStoresResponse>> GetAsync(
        int pageNumber,
        int pageSize)
    {
        var param = new
        {
            Offset = (pageNumber - 1) * pageSize, 
            Limit = pageSize
        };

        var multipleReader = await connection.QueryMultipleAsync(
            sql: GetPagedStores + CountStores,
            param: param);

        var stores = await multipleReader.ReadAsync<SearchStoresResponse>();
        var count = await multipleReader.ReadSingleAsync<int>();

        return new PagedResponse<SearchStoresResponse>(
            count,
            stores);
    }

    public async Task<Guid> GetIdAsync(string externalId) =>
        await connection
            .QuerySingleOrDefaultAsync<Guid>(
                GetIdByExternalId,
                new { ExternalId = externalId });

    public async Task<bool> DeleteAsync(Guid id)
    {
        var affectedRows =
            await connection.ExecuteAsync(DeleteStore, new { Id = id });
        return affectedRows > 0;
    }

    #endregion

    #region Store Items

    public async Task<bool> AddItemsAsync(StoreItem[] items)
    {
        if (items.Length == 0)
        {
            return true;
        }

        var affectedRows = await connection.ExecuteAsync(AddStoreItems, items);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteItemsAsync(Guid[] ids)
    {
        if (ids.Length == 0)
        {
            return true;
        }

        var affectedRows =
            await connection.ExecuteAsync(DeleteStoreItems, new { Ids = ids });
        return affectedRows > 0;
    }

    #endregion
}