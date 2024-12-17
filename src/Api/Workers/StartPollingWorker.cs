using System.Threading.Channels;
using Api.Entities;
using Api.UseCases.Stores.PollStoreItems;

namespace Api.Workers;

public sealed class StartPollingWorker(
    IPollStoreItemsUseCase useCase,
    Channel<IEnumerable<Store>> channel,
    ILogger<StartPollingWorker> logger) : IHostedService, IDisposable
{
    private readonly IPollStoreItemsUseCase _useCase = useCase;
    private readonly Channel<IEnumerable<Store>> _channel = channel;
    private readonly ILogger<StartPollingWorker> _logger = logger;
    private Timer? _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Polling Worker is running...");

        _timer = new Timer(
            callback: DoWork,
            state: cancellationToken,
            dueTime: TimeSpan.Zero,
            period: TimeSpan.FromMinutes(1));

        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        var cancellationToken = state is not null
            ? (CancellationToken)state
            : CancellationTokenSource.CreateLinkedTokenSource().Token;

        var storesWithNewItems =
            await _useCase.ExecuteAsync(cancellationToken);

        if (!storesWithNewItems.Any())
        {
            _logger.LogInformation("There aren't new items, won't send notification.");

            return;
        }

        await _channel.Writer.WriteAsync(storesWithNewItems, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Polling Worker is stopping...");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();

        _ = _channel.Writer.TryComplete();

        GC.SuppressFinalize(this);
    }
}
