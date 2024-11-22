using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog.Enrichers.AzureClaims.Tests.Helpers;
using Serilog.Events;
using System.Security.Claims;
using Xunit;

namespace Serilog.Enrichers.AzureClaims.Tests.AzureClaims;

public class OIDEnricherTests
{
    [Fact]
    public void LogEvent_DoesNotContainOIDWhenUserIsNotLoggedIn()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext());

        var oidEnricher = new ObjectIdEnricher(httpContextAccessorMock);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(oidEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"ObjectId property is not set when the user is not logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.False(evt.Properties.ContainsKey("ObjectId"));
    }

    [Fact]
    public void LogEvent_ContainsUnknownOIDWhenUserIsLoggedInButOIDIsNotFound()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        var user = new ClaimsPrincipal(TestClaimsProvider.NotValidClaims().GetClaimsPrincipal());

        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext
        {
            User = user
        });

        var oidEnricher = new ObjectIdEnricher(httpContextAccessorMock);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(oidEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"ObjectId property is set to unknown when the user is logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.True(evt.Properties.ContainsKey("ObjectId"));
        Assert.Equal("unknown", evt.Properties["ObjectId"].LiteralValue().ToString());
    }

    [Fact]
    public void LogEvent_ContainOIDWhenUserIsLoggedIn()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        var user = new ClaimsPrincipal(TestClaimsProvider.ValidClaims().GetClaimsPrincipal());

        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext
        {
            User = user
        });

        var oidEnricher = new ObjectIdEnricher(httpContextAccessorMock);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(oidEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"ObjectId property is set when the user is logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.True(evt.Properties.ContainsKey("ObjectId"));
        Assert.Equal(TestConstants.OID, evt.Properties["ObjectId"].LiteralValue().ToString());
    }
}
