namespace Api.UseCases;

public interface IWorkerUseCaseWithResponse<T>
{
    Task<T> ExecuteAsync(CancellationToken cancellationToken);
}
