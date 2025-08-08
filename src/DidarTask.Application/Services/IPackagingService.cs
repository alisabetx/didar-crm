using System;

namespace DidarTask.Application.Services;

/// <summary>
/// Interface for the Packaging service providing subscription information
/// and a simple connectivity check.
/// </summary>
public interface IPackagingService
{
    /// <summary>
    /// Returns access information for a specific user.
    /// </summary>
    AccessInfo GetAccessInfo(Guid userId);

    /// <summary>
    /// Attempts to change a user's subscription. Returns false and rolls back
    /// when the Application service cannot be reached.
    /// </summary>
    bool TryChangeSubscription(Guid userId, AccessInfo newInfo, IApplicationService applicationService);

    /// <summary>
    /// Returns true when the Packaging service is reachable.
    /// </summary>
    bool Ping();
}
