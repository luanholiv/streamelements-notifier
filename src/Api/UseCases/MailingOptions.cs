namespace Api.UseCases;

public class MailingOptions
{
    public string ApiKey { get; set; } = string.Empty;
    public string SenderAddress { get; set; } = string.Empty;
    public string ReceiverAddress { get; set; } = string.Empty;
}
