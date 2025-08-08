using System;
using DidarTask.Application.Services;
using Xunit;

namespace DidarTask.Application.Tests;

public class PackagingConnectionTests
{
    [Fact]
    public void VerifyPackagingConnection_ReturnsTrue()
    {
        var packagingService = new PackagingService();
        var applicationService = new ApplicationService(packagingService);

        Assert.True(applicationService.VerifyPackagingConnection());
    }

    [Fact]
    public void RequestAccessInfo_ReturnsExpectedSubscriptionLevel()
    {
        var packagingService = new PackagingService();
        var applicationService = new ApplicationService(packagingService);

        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var access = applicationService.RequestAccessInfo(userId);

        Assert.Equal("Basic", access.SubscriptionLevel);
        Assert.Equal(3, access.AllowedFeatures);
    }
}
