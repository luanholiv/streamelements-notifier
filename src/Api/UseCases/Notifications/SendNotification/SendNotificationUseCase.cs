using Api.Entities;

using Microsoft.Extensions.Options;

using static Api.UseCases.Notifications.SendNotification.NotificationTemplate;

namespace Api.UseCases.Notifications.SendNotification;

public sealed class SendNotificationUseCase(
    ILogger<SendNotificationUseCase> logger,
    IEmailApiClient emailApiClient,
    IOptions<MailingOptions> mailingOptions) : ISendNotificationUseCase
{
    private readonly ILogger<SendNotificationUseCase> logger = logger;
    private readonly IEmailApiClient emailApiClient = emailApiClient;
    private readonly MailingOptions mailingOptions = mailingOptions.Value;
    private const string HTML_EMAIL_SUBJECT = "Notificação das lojinhas da StreamElements";

    public async Task ExecuteAsync(
        IEnumerable<Store> payload,
        CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        if (!payload.Any())
            return;

        var listItems = payload.ToHtmlListItem();

        var emailBody = string.Format(HTML_EMAIL_TEMPLATE, listItems);

        var response = await emailApiClient.SendAsync(
            mailingOptions.ApiKey,
            HTML_EMAIL_SUBJECT,
            mailingOptions.SenderAddress,
            mailingOptions.ReceiverAddress,
            emailBody);

        if (!response.IsSuccessStatusCode)
            logger.LogWarning("Error while sending e-mail. Mailing service response status: {status}", response.StatusCode);
    }
}
