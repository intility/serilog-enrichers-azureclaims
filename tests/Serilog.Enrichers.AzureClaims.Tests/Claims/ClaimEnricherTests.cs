using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog.Enrichers.AzureClaims.Tests.Helpers;
using Serilog.Events;
using System.Security.Claims;
using Xunit;
using Serilog.Enrichers.Claims;
using Serilog.Core;
using Serilog.Parsing;

namespace Serilog.Enrichers.AzureClaims.Tests.Claims;

public class ClaimEnricherTests
{
    private const string _customClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
    private const string _customClaimPropertyName = "Email";

    [Fact]
    public void LogEvent_DoesNotContainCustomClaimWhenUserIsNotLoggedIn()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext());

        var customClaimEnricher = new ClaimEnricher(httpContextAccessorMock, _customClaimType, _customClaimPropertyName);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(customClaimEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"Email property is not set when the user is not logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.False(evt.Properties.ContainsKey("Email"));
    }

    [Fact]
    public void LogEvent_ContainsUnknownCustomClaimWhenUserIsLoggedInButCustomClaimIsNotFound()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        var user = new ClaimsPrincipal(TestClaimsProvider.NotValidClaims().GetClaimsPrincipal());

        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext
        {
            User = user
        });

        var customClaimEnricher = new ClaimEnricher(httpContextAccessorMock, _customClaimType, _customClaimPropertyName);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(customClaimEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"Email property is set to unknown when the user is logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.True(evt.Properties.ContainsKey("Email"));
        Assert.Equal("unknown", evt.Properties["Email"].LiteralValue().ToString());
    }

    [Fact]
    public void LogEvent_ContainCustomClaimWhenUserIsLoggedIn()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        var user = new ClaimsPrincipal(TestClaimsProvider.ValidClaims().GetClaimsPrincipal());

        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext
        {
            User = user
        });

        var customClaimEnricher = new ClaimEnricher(httpContextAccessorMock, _customClaimType, _customClaimPropertyName);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(customClaimEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"Email property is set when the user is logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.True(evt.Properties.ContainsKey("Email"));
        Assert.Equal(TestConstants.EMAIL, evt.Properties["Email"].LiteralValue().ToString());
    }


    [Fact]
    public void LogEvent_ContainCustomClaimWhenUserIsLoggedIn_WithoutCustomPropertyName()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        var user = new ClaimsPrincipal(TestClaimsProvider.ValidClaims().GetClaimsPrincipal());

        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext
        {
            User = user
        });

        var customClaimEnricher = new ClaimEnricher(httpContextAccessorMock, _customClaimType);

        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.With(customClaimEnricher)
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        // Act
        log.Information(@"Email property is set when the user is logged in");

        // Assert
        Assert.NotNull(evt);
        Assert.True(evt.Properties.ContainsKey(_customClaimType));
        Assert.Equal(TestConstants.EMAIL, evt.Properties[_customClaimType].LiteralValue().ToString());
    }

    [Fact]
    public void Enrich_ShouldReturn_WhenHttpContextIsNull()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        httpContextAccessorMock.HttpContext.Returns((HttpContext?)null);

        var customClaimEnricher = new ClaimEnricher(httpContextAccessorMock, _customClaimType, _customClaimPropertyName);
        var logEvent = new LogEvent(
            DateTimeOffset.Now,
            LogEventLevel.Information, 
            null, 
            new MessageTemplate([]), []
        );

        var propertyFactory = Substitute.For<ILogEventPropertyFactory>();

        // Act
        customClaimEnricher.Enrich(logEvent, propertyFactory);

        // Assert
        Assert.Empty(logEvent.Properties);
    }

    [Fact]
    public void Enrich_ShouldAddLogEventProperty_WhenItemKeyIsLogEventProperty()
    {
        // Arrange
        var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
        var user = new ClaimsPrincipal(TestClaimsProvider.ValidClaims().GetClaimsPrincipal());

        httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext
        {
            User = user,
            Items = new Dictionary<object, object>()
        });

        var customClaimEnricher = new ClaimEnricher(httpContextAccessorMock, _customClaimType, _customClaimPropertyName);
        var logEvent = new LogEvent(
            DateTimeOffset.Now,
            LogEventLevel.Information,
            null,
            new MessageTemplate([]), []
        );

        var propertyFactory = Substitute.For<ILogEventPropertyFactory>();

        var logEventProperty = new LogEventProperty(_customClaimPropertyName, new ScalarValue("test@example.com"));
        httpContextAccessorMock.HttpContext.Items[$"Serilog_{_customClaimPropertyName}"] = logEventProperty;

        // Act
        customClaimEnricher.Enrich(logEvent, propertyFactory);

        // Assert
        Assert.Contains(logEvent.Properties, p => p.Key == _customClaimPropertyName && p.Value.Equals(new ScalarValue("test@example.com")));
    }
}
