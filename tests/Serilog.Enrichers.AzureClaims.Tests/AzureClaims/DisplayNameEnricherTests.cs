using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog.Enrichers.AzureClaims.Tests.Helpers;
using Serilog.Events;
using System.Security.Claims;
using Xunit;

namespace Serilog.Enrichers.AzureClaims.Tests.AzureClaims;

public class DisplayNameEnricherTests
{
    [Fact]
    public void LogEvent_DoesNotContainDisplayNameWhenUserIsNotLoggedIn()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext());

        var displayNameEnricher = new DisplayNameEnricher(httpContextAccessorMock);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(displayNameEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"DisplayName property is not set when the user is not logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.False(evt.Properties.ContainsKey("DisplayName"));
    }

    [Fact]
    public void LogEvent_ContainsUnknownDisplayNameWhenUserIsLoggedInButDisplayNameIsNotFound()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        var user = new ClaimsPrincipal(TestClaimsProvider.NotValidClaims().GetClaimsPrincipal());

        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext
        {
            User = user
        });

        var displayNameEnricher = new DisplayNameEnricher(httpContextAccessorMock);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(displayNameEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"DisplayName property is set to unknown when the user is logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.True(evt.Properties.ContainsKey("DisplayName"));
        Assert.Equal("unknown", evt.Properties["DisplayName"].LiteralValue().ToString());
    }

    [Fact]
    public void LogEvent_ContainDisplayNameWhenUserIsLoggedIn()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        var user = new ClaimsPrincipal(TestClaimsProvider.ValidClaims().GetClaimsPrincipal());

        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext
        {
            User = user
        });

        var displayNameEnricher = new DisplayNameEnricher(httpContextAccessorMock);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(displayNameEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"DisplayName property is set when the user is logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.True(evt.Properties.ContainsKey("DisplayName"));
        Assert.Equal(TestConstants.NAME, evt.Properties["DisplayName"].LiteralValue().ToString());
    }
}

