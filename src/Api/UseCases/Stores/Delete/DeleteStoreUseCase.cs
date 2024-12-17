using Api.Database.Stores;
using static System.Net.HttpStatusCode;

namespace Api.UseCases.Stores.Delete;

public sealed class DeleteStoreUseCase(IStoreRepository storeRepository)
    : IDeleteStoreUseCase
{
    public async Task<HttpUseCaseResponse<bool>> ExecuteAsync(
        DeleteStoreRequest request)
    {
        try
        {
            var success = await storeRepository.DeleteAsync(request.Id);

            return success
                ? new HttpUseCaseResponse<bool>(OK, success, [])
                : new HttpUseCaseResponse<bool>(
                    UnprocessableEntity,
                    success,
                    [new("Loja não encontrada.")]);
        }
        catch (Exception ex)
        {
            return new HttpUseCaseResponse<bool>(
                InternalServerError,
                false,
                [new(ex.Message)]);
        }
    }
}