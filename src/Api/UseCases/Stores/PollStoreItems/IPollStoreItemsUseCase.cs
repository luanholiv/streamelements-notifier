using Api.Entities;

namespace Api.UseCases.Stores.PollStoreItems;

public interface IPollStoreItemsUseCase : IWorkerUseCaseWithResponse<IEnumerable<Store>> { }