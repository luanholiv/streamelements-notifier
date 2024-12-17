using Api.Entities;
using Api.UseCases;
using Api.UseCases.Stores.Search;

namespace Api.Database.Stores;

public interface IStoreRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="store"></param>
    /// <returns></returns>
    Task<bool> AddAsync(Store store);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<Store>> GetAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    Task<PagedResponse<SearchStoresResponse>> GetAsync(
        int pageNumber,
        int pageSize);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(Guid id);

    /// <summary>
    /// Gets a Store ID based on an External ID
    /// </summary>
    /// <param name="externalId">Store's External ID</param>
    /// <returns>The store ID</returns>
    Task<Guid> GetIdAsync(string externalId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    Task<bool> AddItemsAsync(StoreItem[] items);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<bool> DeleteItemsAsync(Guid[] ids);
}