using Api.Entities;

namespace Api.Database.Stores;

public interface IStoreRepository
{
    Task<IEnumerable<Store>> GetAsync();
    Task<Guid> GetIdAsync(string externalId);
    Task<bool> AddAsync(Store store);
    Task<bool> AddItemsAsync(IEnumerable<StoreItem> items);
    Task<bool> DeleteItemsAsync(Guid[] ids);
}
