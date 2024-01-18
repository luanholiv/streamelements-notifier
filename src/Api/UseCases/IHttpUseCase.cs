namespace Api.UseCases;

public interface IHttpUseCase<in TRequest, TResponse>
{
    Task<HttpUseCaseResponse<TResponse>> ExecuteAsync(TRequest request);
}
