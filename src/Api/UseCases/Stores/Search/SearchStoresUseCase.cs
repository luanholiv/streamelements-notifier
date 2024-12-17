using Api.Database.Stores;
using System.Net;

namespace Api.UseCases.Stores.Search;

public sealed class SearchStoresUseCase(IStoreRepository storeDatabase)
    : ISearchStoresUseCase
{
    public async Task<HttpUseCaseResponse<PagedResponse<SearchStoresResponse>>>
        ExecuteAsync(
            PagedRequest request)
    {
        try
        {
            var result = await storeDatabase
                .GetAsync(request.PageNumber, request.PageSize);

            if (result.TotalRecords == 0)
            {
                return new HttpUseCaseResponse<
                    PagedResponse<SearchStoresResponse>>(
                    HttpStatusCode.NoContent,
                    result,
                    []);
            }

            return new HttpUseCaseResponse<PagedResponse<SearchStoresResponse>>(
                HttpStatusCode.OK,
                result,
                []);
        }
        catch (Exception ex)
        {
            return new HttpUseCaseResponse<PagedResponse<SearchStoresResponse>>(
                HttpStatusCode.InternalServerError,
                new PagedResponse<SearchStoresResponse>(0,[]),
                [new(ex.Message)]);
        }
    }
}