using Microsoft.AspNetCore.Http;
using NSubstitute;
using Serilog.Core;
using Serilog.Enrichers.AzureClaims.Tests.Helpers;
using Serilog.Events;
using System.Reflection;
using Xunit;

namespace Serilog.Enrichers.AzureClaims.Tests.Extensions
{
    public class ExtensionsBindingTests
    {
        [Fact]
        public void AllEnrichers_AreAddedToTheBuilder()
        {
            // Arrange
            var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
            httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext());

            // Remember to add all enrichers to the builder, if not the test will fail
            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithUpn()
                .Enrich.WithAppId()
                .Enrich.WithTenantId()
                .Enrich.WithObjectId()
                .Enrich.WithDisplayName()
                .Enrich.WithCustomClaim("test")
                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            var aggregateEnricherFieldInfo = log.GetType()
                .GetField("_enricher", BindingFlags.Instance | BindingFlags.NonPublic);

            var aggregateEnricher = aggregateEnricherFieldInfo?.GetValue(log);
            var enrichers = aggregateEnricher.GetType()
                .GetField("_enrichers", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(aggregateEnricher) as IEnumerable<ILogEventEnricher>;

            // Assert that all Enrichers are added to the builder
            Assert.Equal(GetCountOfEnrichers(), enrichers.Count());
            Assert.Collection(enrichers,
                item => Assert.Equal(nameof(UpnEnricher), item.GetType().Name),
                item => Assert.Equal(nameof(AppIdEnricher), item.GetType().Name),
                item => Assert.Equal(nameof(TenantIdEnricher), item.GetType().Name),
                item => Assert.Equal(nameof(ObjectIdEnricher), item.GetType().Name),
                item => Assert.Equal(nameof(DisplayNameEnricher), item.GetType().Name),
                item => Assert.Equal(nameof(CustomClaimEnricher), item.GetType().Name));
        }

        // Method to count the number of enrichers in the project. Ignores the base enricher
        private int GetCountOfEnrichers()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var projectDirectory = Path.GetDirectoryName(executingAssembly.Location);
            var targetDirectory = Path.Combine(projectDirectory, "..", "..", "..", "..", "..", "src", "Serilog.Enrichers.AzureClaims");
            var files = Directory.GetFiles(targetDirectory, "*Enricher.cs", SearchOption.AllDirectories);

            var count = files.Count(file => !Path.GetFileName(file).Equals("BaseEnricher.cs", StringComparison.OrdinalIgnoreCase));
            return count;
        }

        [Fact]
        public void UPNEnricher_IsAddedToTheBuilder()
        {
            // Arrange
            var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
            httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext());

            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithUpn()
                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            var aggregateEnricherFieldInfo = log.GetType()
                .GetField("_enricher", BindingFlags.Instance | BindingFlags.NonPublic);

            var aggregateEnricher = aggregateEnricherFieldInfo?.GetValue(log);
            var enricher = aggregateEnricher.GetType();

            Assert.Equal(nameof(UpnEnricher), enricher.Name);
        }

        [Fact]
        public void AppIdEnricher_IsAddedToTheBuilder()
        {
            // Arrange
            var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
            httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext());

            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithAppId()
                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            var aggregateEnricherFieldInfo = log.GetType()
                .GetField("_enricher", BindingFlags.Instance | BindingFlags.NonPublic);

            var aggregateEnricher = aggregateEnricherFieldInfo?.GetValue(log);
            var enricher = aggregateEnricher.GetType();

            Assert.Equal(nameof(AppIdEnricher), enricher.Name);
        }

        [Fact]
        public void TenantIdEnricher_IsAddedToTheBuilder()
        {
            // Arrange
            var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
            httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext());

            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithTenantId()
                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            var aggregateEnricherFieldInfo = log.GetType()
                .GetField("_enricher", BindingFlags.Instance | BindingFlags.NonPublic);

            var aggregateEnricher = aggregateEnricherFieldInfo?.GetValue(log);
            var enricher = aggregateEnricher.GetType();

            Assert.Equal(nameof(TenantIdEnricher), enricher.Name);
        }

        [Fact]
        public void DisplayNameEnricher_IsAddedToTheBuilder()
        {
            // Arrange
            var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
            httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext());

            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithDisplayName()
                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            var aggregateEnricherFieldInfo = log.GetType()
                .GetField("_enricher", BindingFlags.Instance | BindingFlags.NonPublic);

            var aggregateEnricher = aggregateEnricherFieldInfo?.GetValue(log);
            var enricher = aggregateEnricher.GetType();

            Assert.Equal(nameof(DisplayNameEnricher), enricher.Name);
        }

        [Fact]
        public void OIDEnricher_IsAddedToTheBuilder()
        {
            // Arrange
            var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
            httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext());

            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithObjectId()
                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            var aggregateEnricherFieldInfo = log.GetType()
                .GetField("_enricher", BindingFlags.Instance | BindingFlags.NonPublic);

            var aggregateEnricher = aggregateEnricherFieldInfo?.GetValue(log);
            var enricher = aggregateEnricher.GetType();

            Assert.Equal(nameof(ObjectIdEnricher), enricher.Name);
        }

        [Fact]
        public void CustomClaimEnricher_IsAddedToTheBuilder()
        {
            // Arrange
            var httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();
            httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext());

            LogEvent evt = null;
            var log = new LoggerConfiguration()
                .Enrich.WithCustomClaim("test")
                .WriteTo.Sink(new DelegatingSink(e => evt = e))
                .CreateLogger();

            var aggregateEnricherFieldInfo = log.GetType()
                .GetField("_enricher", BindingFlags.Instance | BindingFlags.NonPublic);

            var aggregateEnricher = aggregateEnricherFieldInfo?.GetValue(log);
            var enricher = aggregateEnricher.GetType();

            Assert.Equal(nameof(CustomClaimEnricher), enricher.Name);
        }
    }
}
