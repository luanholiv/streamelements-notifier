using Api.Entities;
using Refit;

namespace Api.UseCases.Stores.PollStoreItems;

public interface IStoreApiClient
{
    [Get("/kappa/v2/store/{channel}/items?source=website")]
    public Task<ApiResponse<IEnumerable<ExternalStoreItem>>> GetByChannel(
        [AliasAs("channel")] string channelId);
}
