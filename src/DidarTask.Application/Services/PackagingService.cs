using System;
using System.Collections.Generic;

namespace DidarTask.Application.Services;

/// <summary>
/// Simple in-memory implementation of <see cref="IPackagingService"/>.
/// In a real-world scenario this would make network requests to another service.
/// </summary>
public class PackagingService : IPackagingService
{
    private readonly Dictionary<Guid, AccessInfo> _subscriptions = new();

    public PackagingService()
    {
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

    public bool Ping() => true;
}
