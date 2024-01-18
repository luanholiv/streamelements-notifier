using Api.Entities;
using Dapper;
using Npgsql;

using static Api.Database.Stores.SqlQueries;

namespace Api.Database.Stores;

public sealed class StoreRepository(NpgsqlConnection connection) : IStoreRepository
{
    private readonly NpgsqlConnection _connection = connection;

    public async Task<IEnumerable<Store>> GetAsync()
    {
        var lookup = new Dictionary<Guid, Store>();

        _ = await _connection.QueryAsync<Store, StoreItem, Store>(
            GET_ALL,
            splitOn: "Id",
            map: (store, storeItem) =>
            {
                if (!lookup.TryGetValue(store.Id, out var storeEntry))
                {
                    storeEntry = store;
                    storeEntry.AvailableItems ??= [];
                    lookup.Add(storeEntry.Id, storeEntry);
                }

                if (storeItem is not null)
                    storeEntry.AvailableItems.Add(storeItem);

                return storeEntry;
            }) ?? [];

        return lookup.Values;
    }

    public async Task<Guid> GetIdAsync(string externalId) =>
        await _connection
            .QuerySingleOrDefaultAsync<Guid>(
                GET_ID_BY_EXTERNAL_ID,
                new { ExternalId = externalId });

    public async Task<bool> AddAsync(Store store)
    {
        var affectedRows = await _connection.ExecuteAsync(ADD, store);
        return affectedRows > 0;
    }

    public async Task<bool> AddItemsAsync(IEnumerable<StoreItem> items)
    {
        if (!items.Any())
            return true;

        var affectedRows = await _connection.ExecuteAsync(ADD_ITEMS, items);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteItemsAsync(Guid[] ids)
    {
        if (!ids.Any())
            return true;

        var affectedRows = await _connection.ExecuteAsync(DELETE_ITEMS, new { Ids = ids });
        return affectedRows > 0;
    }
}
