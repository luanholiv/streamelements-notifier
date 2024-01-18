using Api.Entities;

namespace Api.UseCases.Notifications.SendNotification;

public interface ISendNotificationUseCase : IWorkerUseCaseWithoutResponse<IEnumerable<Store>> { }