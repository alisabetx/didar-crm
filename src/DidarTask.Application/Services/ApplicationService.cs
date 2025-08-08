using System;
using System.Threading.Tasks;

namespace DidarTask.Application.Services;

/// <summary>
/// Represents the business application service which requests access information
/// from the Packaging service throughout the application.
/// </summary>
public class ApplicationService : IApplicationService
{
    private readonly IPackagingService _packagingService;
    private int _businessState;

    /// <summary>
    /// Exposes the current business state so tests can verify rollback behaviour.
    /// </summary>
    public int BusinessState => _businessState;

    public ApplicationService(IPackagingService packagingService)
    {
        _packagingService = packagingService;
    }

    /// <summary>
    /// Attempts to apply a change to the application's business state. If the
    /// Packaging service cannot be reached the change is rolled back.
    /// </summary>
    public bool TryApplyBusinessChange(int delta)
    {
        return TransactionCoordinator.Execute(
            () => _businessState += delta,
            () => _packagingService.Ping(),
            () => _businessState -= delta);
    }

    public Task<bool> TryApplyBusinessChangeAsync(int delta)
    {
        return TransactionCoordinator.ExecuteAsync(
            () => { _businessState += delta; return Task.CompletedTask; },
            () => _packagingService.PingAsync(),
            () => { _businessState -= delta; return Task.CompletedTask; });
    }

    /// <summary>
    /// Retrieves the current user's subscription information.
    /// </summary>
    public AccessInfo RequestAccessInfo(Guid userId) => _packagingService.GetAccessInfo(userId);

    public Task<AccessInfo> RequestAccessInfoAsync(Guid userId) => _packagingService.GetAccessInfoAsync(userId);

    /// <summary>
    /// Verifies that the Packaging service can be reached.
    /// </summary>
    public bool VerifyPackagingConnection() => _packagingService.Ping();

    public Task<bool> VerifyPackagingConnectionAsync() => _packagingService.PingAsync();

    /// <summary>
    /// Pings the Application service. This will always return true for the
    /// in-memory implementation but allows other services to verify availability.
    /// </summary>
    public bool Ping() => true;

    public Task<bool> PingAsync() => Task.FromResult(true);
}
