using System;

namespace DidarTask.Application.Services;

/// <summary>
/// Represents the business application service which requests access information
/// from the Packaging service throughout the application.
/// </summary>
public class ApplicationService
{
    private readonly IPackagingService _packagingService;

    public ApplicationService(IPackagingService packagingService)
    {
        _packagingService = packagingService;
    }

    /// <summary>
    /// Retrieves the current user's subscription information.
    /// </summary>
    public AccessInfo RequestAccessInfo(Guid userId) => _packagingService.GetAccessInfo(userId);

    /// <summary>
    /// Verifies that the Packaging service can be reached.
    /// </summary>
    public bool VerifyPackagingConnection() => _packagingService.Ping();
}
