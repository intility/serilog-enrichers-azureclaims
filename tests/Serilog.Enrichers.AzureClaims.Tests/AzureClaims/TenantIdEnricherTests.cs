using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog.Enrichers.AzureClaims.Tests.Helpers;
using Serilog.Events;
using System.Security.Claims;
using Xunit;

namespace Serilog.Enrichers.AzureClaims.Tests.AzureClaims;

public class TenantIdEnricherTests
{
    [Fact]
    public void LogEvent_DoesNotContainTenantIdWhenUserIsNotLoggedIn()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext());

        var tenantIdEnricher = new TenantIdEnricher(httpContextAccessorMock);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(tenantIdEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"TenantId property is not set when the user is not logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.False(evt.Properties.ContainsKey("TenantId"));
    }

    [Fact]
    public void LogEvent_ContainsUnknownTenantIdWhenUserIsLoggedInButTenantIdIsNotFound()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        var user = new ClaimsPrincipal(TestClaimsProvider.NotValidClaims().GetClaimsPrincipal());

        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext
        {
            User = user
        });

        var tenantIdEnricher = new TenantIdEnricher(httpContextAccessorMock);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(tenantIdEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"TenantId property is set to unknown when the user is logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.True(evt.Properties.ContainsKey("TenantId"));
        Assert.Equal("unknown", evt.Properties["TenantId"].LiteralValue().ToString());
    }

    [Fact]
    public void LogEvent_ContainTenantIdWhenUserIsLoggedIn()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        var user = new ClaimsPrincipal(TestClaimsProvider.ValidClaims().GetClaimsPrincipal());

        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext
        {
            User = user
        });

        var tenantIdEnricher = new TenantIdEnricher(httpContextAccessorMock);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(tenantIdEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"TenantId property is set when the user is logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.True(evt.Properties.ContainsKey("TenantId"));
        Assert.Equal(TestConstants.TENANTID, evt.Properties["TenantId"].LiteralValue().ToString());
    }
}
