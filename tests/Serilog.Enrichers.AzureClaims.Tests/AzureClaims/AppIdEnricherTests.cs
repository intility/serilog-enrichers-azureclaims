using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog.Enrichers.AzureClaims.Tests.Helpers;
using Serilog.Events;
using System.Security.Claims;
using Xunit;

namespace Serilog.Enrichers.AzureClaims.Tests.AzureClaims;

public class AppIdEnricherTests
{
    [Fact]
    public void LogEvent_DoesNotContainAppIdWhenUserIsNotLoggedIn()
    {
        // Arrange
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        httpContextAccessor.HttpContext.Returns(new DefaultHttpContext());

        var appidEnricher = new AppIdEnricher(httpContextAccessor);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(appidEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"AppId property is not set when user is not logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.False(evt.Properties.ContainsKey("AppId"));
    }

    [Fact]
    public void LogEvent_ContainsUnknownAppIdWhenUserIsLoggedInButAppIdIsNotFound()
    {
        // Arrange
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        var user = new ClaimsPrincipal(TestClaimsProvider.NotValidClaims().GetClaimsPrincipal());

        httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
        {
            User = user
        });

        var appidEnricher = new AppIdEnricher(httpContextAccessor);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(appidEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"AppId property is set to unknown when user is logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.True(evt.Properties.ContainsKey("AppId"));
        Assert.Equal("unknown", evt.Properties["AppId"].LiteralValue().ToString());
    }

    [Theory]
    [InlineData("azp")]
    [InlineData("appid")]
    public void LogEvent_ContainAppIdWhenUserIsLoggedIn(string claim)
    {
        // Arrange
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        var claimValue = Guid.NewGuid().ToString();
        var user = new ClaimsPrincipal(TestClaimsProvider.ValidClaims().AddClaim(claim, claimValue).GetClaimsPrincipal());

        httpContextAccessor.HttpContext.Returns(new DefaultHttpContext
        {
            User = user
        });

        var appidEnricher = new AppIdEnricher(httpContextAccessor);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(appidEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"AppId property is set when user is logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.True(evt.Properties.ContainsKey("AppId"));
        Assert.Equal(claimValue, evt.Properties["AppId"].LiteralValue().ToString());
    }
}
