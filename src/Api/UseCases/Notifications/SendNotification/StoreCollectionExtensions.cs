using Api.Entities;

namespace Api.UseCases.Notifications.SendNotification;

public static class StoreCollectionExtensions
{
    public static string ToHtmlListItem(
        this IEnumerable<Store> stores)
    {
        var items = stores.Select(store =>
            @$"
            <li>
                <a href='{store.Uri}'>{store.StreamerName}</a>
                <ul>
                    {store.AvailableItems!.ToHtmlListItem()}
                </ul>
            </li>"
        );

        return string.Join('\n', items);
    }
}
