using Api.UseCases.Notifications.SendNotification;
using Api.UseCases.Stores.Add;
using Api.UseCases.Stores.PollStoreItems;
using Polly;
using Polly.Retry;
using Refit;

namespace Api.UseCases;

public static class UseCasesConfiguration
{
    private static AsyncRetryPolicy<HttpResponseMessage> RetryPolicy =>
        Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync([
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            ]);
    public static IServiceCollection AddUseCases(
        this IServiceCollection services)
    {
        services.AddSingleton<IAddStoreUseCase, AddStoreUseCase>();
        services.AddSingleton<IPollStoreItemsUseCase, PollStoreItemsUseCase>();
        services.AddSingleton<ISendNotificationUseCase, SendNotificationUseCase>();

        return services;
    }

    public static IServiceCollection AddMailing(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.Configure<MailingOptions>(config.GetSection("Mailing"));

        services.AddRefitClient<IEmailApiClient>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("https://api.elasticemail.com");
            })
            .AddPolicyHandler(RetryPolicy);

        return services;
    }

    public static IServiceCollection AddStreamElementsClient(
        this IServiceCollection services,
        IConfiguration config)
    {
        var accessToken = config.GetValue<string>("StreamElements:AccessToken");

        services.AddRefitClient<IStoreApiClient>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri("https://api.streamelements.com");
                client.DefaultRequestHeaders.Add("Accept", "application/json; charset=utf-8");
                client.DefaultRequestHeaders.Add("Authorization", accessToken);
            })
            .AddPolicyHandler(RetryPolicy);

        return services;
    }
}
