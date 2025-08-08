using System;
using System.Threading.Tasks;
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

    [Fact]
    public void ApplyBusinessChange_RollsBack_WhenPackagingUnavailable()
    {
        var packagingService = new PackagingService(isAvailable: false);
        var applicationService = new ApplicationService(packagingService);

        var result = applicationService.TryApplyBusinessChange(5);

        Assert.False(result);
        Assert.Equal(0, applicationService.BusinessState);
    }

    private class FailingApplicationService : IApplicationService
    {
        public bool Ping() => false;
        public Task<bool> PingAsync() => Task.FromResult(false);
    }

    [Fact]
    public void ChangeSubscription_RollsBack_WhenApplicationUnavailable()
    {
        var packagingService = new PackagingService();
        var failingApp = new FailingApplicationService();
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var newInfo = new AccessInfo("VIP", 20);

        var result = packagingService.TryChangeSubscription(userId, newInfo, failingApp);

        Assert.False(result);
        var access = packagingService.GetAccessInfo(userId);
        Assert.Equal("Basic", access.SubscriptionLevel);
        Assert.Equal(3, access.AllowedFeatures);
    }

    [Fact]
    public async Task VerifyPackagingConnectionAsync_ReturnsTrue()
    {
        var packagingService = new PackagingService();
        var applicationService = new ApplicationService(packagingService);

        Assert.True(await applicationService.VerifyPackagingConnectionAsync());
    }

    [Fact]
    public async Task RequestAccessInfoAsync_ReturnsExpectedSubscriptionLevel()
    {
        var packagingService = new PackagingService();
        var applicationService = new ApplicationService(packagingService);

        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var access = await applicationService.RequestAccessInfoAsync(userId);

        Assert.Equal("Basic", access.SubscriptionLevel);
        Assert.Equal(3, access.AllowedFeatures);
    }

    [Fact]
    public async Task ApplyBusinessChangeAsync_RollsBack_WhenPackagingUnavailable()
    {
        var packagingService = new PackagingService(isAvailable: false);
        var applicationService = new ApplicationService(packagingService);

        var result = await applicationService.TryApplyBusinessChangeAsync(5);

        Assert.False(result);
        Assert.Equal(0, applicationService.BusinessState);
    }

    [Fact]
    public async Task ChangeSubscriptionAsync_RollsBack_WhenApplicationUnavailable()
    {
        var packagingService = new PackagingService();
        var failingApp = new FailingApplicationService();
        var userId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var newInfo = new AccessInfo("VIP", 20);

        var result = await packagingService.TryChangeSubscriptionAsync(userId, newInfo, failingApp);

        Assert.False(result);
        var access = await packagingService.GetAccessInfoAsync(userId);
        Assert.Equal("Basic", access.SubscriptionLevel);
        Assert.Equal(3, access.AllowedFeatures);
    }
}
