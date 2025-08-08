using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DidarTask.Application.Services;

/// <summary>
/// Simple in-memory implementation of <see cref="IPackagingService"/>.
/// In a real-world scenario this would make network requests to another service.
/// </summary>
public class PackagingService : IPackagingService
{
    private readonly Dictionary<Guid, AccessInfo> _subscriptions = new();
    private readonly bool _isAvailable;

    public PackagingService(bool isAvailable = true)
    {
        _isAvailable = isAvailable;

        // Seed with a couple of sample users and subscription info.
        var user1 = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var user2 = Guid.Parse("00000000-0000-0000-0000-000000000002");

        _subscriptions[user1] = new AccessInfo("Basic", 3);
        _subscriptions[user2] = new AccessInfo("Premium", 10);
    }

    public AccessInfo GetAccessInfo(Guid userId)
    {
        return _subscriptions.TryGetValue(userId, out var info)
            ? info
            : new AccessInfo("Free", 1);
    }

    public Task<AccessInfo> GetAccessInfoAsync(Guid userId) => Task.FromResult(GetAccessInfo(userId));

    /// <summary>
    /// Attempts to change a user's subscription. If the application service is
    /// unavailable the change is rolled back to the previous value.
    /// </summary>
    public bool TryChangeSubscription(Guid userId, AccessInfo newInfo, IApplicationService applicationService)
    {
        AccessInfo? previous = null;
        return TransactionCoordinator.Execute(
            () =>
            {
                _subscriptions.TryGetValue(userId, out previous);
                _subscriptions[userId] = newInfo;
            },
            () => applicationService.Ping(),
            () =>
            {
                if (previous is not null)
                {
                    _subscriptions[userId] = previous;
                }
                else
                {
                    _subscriptions.Remove(userId);
                }
            });
    }

    public Task<bool> TryChangeSubscriptionAsync(Guid userId, AccessInfo newInfo, IApplicationService applicationService)
    {
        AccessInfo? previous = null;
        return TransactionCoordinator.ExecuteAsync(
            () =>
            {
                _subscriptions.TryGetValue(userId, out previous);
                _subscriptions[userId] = newInfo;
                return Task.CompletedTask;
            },
            () => applicationService.PingAsync(),
            () =>
            {
                if (previous is not null)
                {
                    _subscriptions[userId] = previous;
                }
                else
                {
                    _subscriptions.Remove(userId);
                }

                return Task.CompletedTask;
            });
    }

    public bool Ping() => _isAvailable;

    public Task<bool> PingAsync() => Task.FromResult(_isAvailable);
}
