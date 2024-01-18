using System.Collections.Concurrent;

using Api.Database.Stores;
using Api.Entities;

namespace Api.UseCases.Stores.PollStoreItems;

public sealed class PollStoreItemsUseCase(
    IStoreApiClient storeApiClient,
    IStoreRepository storeDatabase,
    ILogger<PollStoreItemsUseCase> logger) : IPollStoreItemsUseCase
{
    private readonly IStoreApiClient _storeApiClient = storeApiClient;
    private readonly IStoreRepository _storeDatabase = storeDatabase;
    private readonly ILogger<PollStoreItemsUseCase> _logger = logger;

    public async Task<IEnumerable<Store>> ExecuteAsync(
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return [];

        try
        {
            var stores = await _storeDatabase.GetAsync();

            if (!stores.Any())
            {
                _logger.LogWarning("There aren't stores in database.");
                return [];
            }

            var currentAvailableItems =
                stores.SelectMany(x => x.AvailableItems) ?? [];

            var externalAvailableItems = new ConcurrentBag<StoreItem>();

            await Parallel.ForEachAsync(stores, async (store, cancellationToken) =>
            {
                var result = await _storeApiClient.GetByChannel(store.ExternalId);

                if (!result.IsSuccessStatusCode)
                    return;

                foreach (var item in result.Content.Where(item => item.IsAvailable))
                {
                    externalAvailableItems.Add(new StoreItem
                    {
                        StoreId = store.Id,
                        ExternalId = item.Id,
                        Name = item.Name,
                        Cost = item.Cost,
                    });
                }
            });

            var newItems = externalAvailableItems
                .Except(currentAvailableItems) ?? [];

            _ = await _storeDatabase.AddItemsAsync(newItems);

            var itemsToDelete = currentAvailableItems
                .Except(externalAvailableItems) ?? [];

            if (itemsToDelete.Any())
            {
                _ = await _storeDatabase.DeleteItemsAsync(
                    itemsToDelete
                        .Where(item => item is not null)
                        .Select(item => item.Id)
                        .ToArray()
                );
            }

            foreach (var store in stores)
            {
                store.AvailableItems = new List<StoreItem>(newItems.Where(item => item.StoreId == store.Id));
            }

            var result = stores.Where(store => store.AvailableItems.Count > 0);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while fetching StereamElements data.");
            return [];
        }
    }
}
