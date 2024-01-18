using System.Threading.Channels;
using Api.Entities;

namespace Api.Workers;

public static class WorkersConfiguration
{
    public static IServiceCollection AddWorkers(this IServiceCollection services)
    {
        services.AddSingleton(Channel.CreateUnbounded<IEnumerable<Store>>());

        services.AddHostedService<StartPollingWorker>();
        services.AddHostedService<SendNotificationWorker>();

        return services;
    }
}
