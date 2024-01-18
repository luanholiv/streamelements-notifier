using Refit;

namespace Api.UseCases.Notifications.SendNotification;

public interface IEmailApiClient
{
    [Post("/v2/email/send")]
    Task<ApiResponse<dynamic>> SendAsync(
        string apiKey,
        string subject,
        string from,
        string to,
        string bodyHtml
    );
}
