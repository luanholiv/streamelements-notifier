using Api.Database.Stores;
using Api.Entities;
using Api.UseCases;
using static System.Net.HttpStatusCode;

namespace Api.UseCases.Stores.Add;
public sealed class AddStoreUseCase(IStoreRepository storeDatabase)
    : IAddStoreUseCase
{
    private readonly IStoreRepository _storeDatabase = storeDatabase;
    private readonly AddStoreRequestValidator _requestValidator = new();

    public async Task<HttpUseCaseResponse<Guid>> ExecuteAsync(
        AddStoreRequest request)
    {
        var validationResult = await _requestValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return new HttpUseCaseResponse<Guid>(
                UnprocessableEntity,
                Guid.Empty,
                validationResult.ToErrorMessages());
        }

        var store = new Store
        {
            ExternalId = request.ExternalId,
            StreamerName = request.StreamerName,
            Uri = request.Uri
        };

        try
        {
            var existingStoreId = await
                _storeDatabase.GetIdAsync(request.ExternalId);

            if (existingStoreId != Guid.Empty)
            {
                return new HttpUseCaseResponse<Guid>(
                    UnprocessableEntity,
                    existingStoreId,
                    [new("Loja já cadastrada")]);
            }

            _ = await _storeDatabase.AddAsync(store);

            return new HttpUseCaseResponse<Guid>(
                Created,
                store.Id,
                []);
        }
        catch (Exception ex)
        {
            return new HttpUseCaseResponse<Guid>(
                InternalServerError,
                Guid.Empty,
                [new(ex.Message)]);
        }
    }
}
