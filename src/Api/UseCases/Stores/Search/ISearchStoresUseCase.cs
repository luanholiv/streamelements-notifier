namespace Api.UseCases.Stores.Search;

public interface ISearchStoresUseCase : IHttpUseCase<PagedRequest, PagedResponse<SearchStoresResponse>> { }