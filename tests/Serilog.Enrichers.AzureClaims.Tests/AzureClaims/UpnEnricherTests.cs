using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog.Enrichers.AzureClaims.Tests.Helpers;
using Serilog.Events;
using System.Security.Claims;
using Xunit;

namespace Serilog.Enrichers.AzureClaims.Tests.AzureClaims;

public class UpnEnricherTests
{
    [Fact]
    public void LogEvent_DoesNotContainUPNWhenUserIsNotLoggedIn()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext());

        var upnEnricher = new UpnEnricher(httpContextAccessorMock);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(upnEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"UPN property is not set when the user is not logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.False(evt.Properties.ContainsKey("UserPrincipalName"));
    }

    [Fact]
    public void LogEvent_ContainsUnknownUPNWhenUserIsLoggedInButUPNIsNotFound()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        var user = new ClaimsPrincipal(TestClaimsProvider.NotValidClaims().GetClaimsPrincipal());

        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext
        {
            User = user
        });

        var upnEnricher = new UpnEnricher(httpContextAccessorMock);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(upnEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"UserPrincipalName property is set to unknown when the user is logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.True(evt.Properties.ContainsKey("UserPrincipalName"));
        Assert.Equal("unknown", evt.Properties["UserPrincipalName"].LiteralValue().ToString());
    }

    [Fact]
    public void LogEvent_ContainUPNWhenUserIsLoggedIn()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        var user = new ClaimsPrincipal(TestClaimsProvider.ValidClaims().GetClaimsPrincipal());

        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext
        {
            User = user
        });

        var upnEnricher = new UpnEnricher(httpContextAccessorMock);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(upnEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"UserPrincipalName property is set when the user is logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.True(evt.Properties.ContainsKey("UserPrincipalName"));
        Assert.Equal(TestConstants.UPN, evt.Properties["UserPrincipalName"].LiteralValue().ToString());
    }
}
