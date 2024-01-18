using System.Threading.Channels;

using Api.Entities;
using Api.UseCases.Notifications.SendNotification;

namespace Api.Workers;

public sealed class SendNotificationWorker(
    ISendNotificationUseCase useCase,
    Channel<IEnumerable<Store>> channel,
    ILogger<SendNotificationWorker> logger) : BackgroundService
{
    private readonly ISendNotificationUseCase _useCase = useCase;
    private readonly Channel<IEnumerable<Store>> _channel = channel;
    private readonly ILogger<SendNotificationWorker> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Notification Worker is running...");

        while (await _channel.Reader.WaitToReadAsync(stoppingToken))
        {
            if (_channel.Reader.TryRead(out var items))
            {
                await _useCase.ExecuteAsync(items, stoppingToken);
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Notification Worker is stopping.");

        await base.StopAsync(stoppingToken);
    }
}

