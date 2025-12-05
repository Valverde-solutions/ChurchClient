namespace ChurchSaaS.Client.Application.Churches;

public interface IChurchUnitAuthorizationService
{
    Task<bool> CanAccessUnitAsync(Guid targetUnitId, CancellationToken cancellationToken = default);
}
