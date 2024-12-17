using System.Collections.Concurrent;
using Api.Database.Stores;
using Api.Entities;

namespace Api.UseCases.Stores.PollStoreItems;

public sealed class PollStoreItemsUseCase(
    IStoreApiClient storeApiClient,
    IStoreRepository storeDatabase,
    ILogger<PollStoreItemsUseCase> logger) : IPollStoreItemsUseCase
{
    public async Task<IEnumerable<Store>> ExecuteAsync(
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return [];
        }

        try
        {
            var stores = await storeDatabase.GetAsync();
            stores = stores.ToArray();

            if (!stores.Any())
            {
                logger.LogWarning("There aren't stores in database.");
                return [];
            }

            var currentAvailableItems = stores
                .SelectMany(x => x.AvailableItems)
                .ToArray();

            var externalAvailableItems = new ConcurrentBag<StoreItem>();

            await Parallel.ForEachAsync(stores, cancellationToken, async (store, _) =>
            {
                var result = await storeApiClient.GetByChannel(store.ExternalId);

                if (!result.IsSuccessStatusCode)
                {
                    logger.LogWarning("There was an error getting the store items. Store: {Store}", store.Id);
                    return;
                }

                if (result.Content == null)
                {
                    logger.LogInformation("No items returned. Store: {Store}", store.Id);
                    return;
                }

                foreach (var item in result.Content.Where(item => item.IsAvailable))
                {
                    externalAvailableItems.Add(new StoreItem
                    {
                        StoreId = store.Id, ExternalId = item.Id, Name = item.Name, Cost = item.Cost,
                    });
                }
            });

            var newItems = externalAvailableItems
                .Except(currentAvailableItems)
                .ToArray();

            _ = await storeDatabase.AddItemsAsync(newItems);

            var itemsToDelete = currentAvailableItems
                .Except(externalAvailableItems)
                .ToArray();

            if (itemsToDelete.Length > 0)
            {
                _ = await storeDatabase.DeleteItemsAsync(
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
            logger.LogError(ex, "Error while fetching StreamElements data.");
            return [];
        }
    }
}