namespace Api.UseCases.Notifications.SendNotification;

public static class NotificationTemplate
{
    public const string HTML_EMAIL_TEMPLATE =
        @"<html>
            <body>
                <h1>Novidades nas lojinhas da StreamElements</h1>
                <p> Tem novidade pra você! </p>
                <br/>
                <p> Veja os novos itens que foram adicionados às lojas dos streamers que você segue:</p>
                <br/>
                <ul>{0}</ul>
            </body>
        </html>";
}
