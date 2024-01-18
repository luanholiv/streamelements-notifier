using Api.Entities;

namespace Api.UseCases.Notifications.SendNotification;

public static class StoreItemCollectionExtensions
{
    public static string ToHtmlListItem(
        this IEnumerable<StoreItem> storeItems
    )
    {
        var items = storeItems.Select(item => $"<li><strong>{item.Name}</strong>: {item.Cost} pontos</li>");
        return string.Join('\n', items);
    }
}