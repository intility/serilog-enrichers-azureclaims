using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog.Enrichers.AzureClaims.Tests.Helpers;
using Serilog.Events;
using System.Security.Claims;
using Xunit;

namespace Serilog.Enrichers.AzureClaims.Tests
{
    public class CustomClaimEnricherTests
    {
        private const string _customClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
        private const string _customClaimPropertyName = "Email";

        [Fact]
        public void LogEvent_DoesNotContainCustomClaimWhenUserIsNotLoggedIn()
        {
            // Arrange
            var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
            httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext());

            var CustomClaimEnricher = new CustomClaimEnricher(httpContextAccessorMock, _customClaimType, _customClaimPropertyName);

            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.With(CustomClaimEnricher)
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

            var CustomClaimEnricher = new CustomClaimEnricher(httpContextAccessorMock, _customClaimType, _customClaimPropertyName);

            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.With(CustomClaimEnricher)
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

            var CustomClaimEnricher = new CustomClaimEnricher(httpContextAccessorMock, _customClaimType, _customClaimPropertyName);

            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.With(CustomClaimEnricher)
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

            var CustomClaimEnricher = new CustomClaimEnricher(httpContextAccessorMock, _customClaimType);

            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.With(CustomClaimEnricher)
                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            // Act
            log.Information(@"Email property is set when the user is logged in");

            // Assert
            Assert.NotNull(evt);
            Assert.True(evt.Properties.ContainsKey(_customClaimType));
            Assert.Equal(TestConstants.EMAIL, evt.Properties[_customClaimType].LiteralValue().ToString());
        }
    }
}
