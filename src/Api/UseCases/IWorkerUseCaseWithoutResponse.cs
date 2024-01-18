namespace Api.UseCases;

public interface IWorkerUseCaseWithoutResponse<in T>
{
    Task ExecuteAsync(T payload, CancellationToken cancellationToken);
}