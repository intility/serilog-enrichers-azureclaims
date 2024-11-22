using Serilog.Configuration;
using Xunit;

namespace Serilog.Enrichers.AzureClaims.Tests.Extensions;

public class ExtensionsExceptionsTests
{
    [Fact]
    public void WithDisplayName_EnrichmentConfigurationIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        LoggerEnrichmentConfiguration enrichmentConfiguration = null;

        var expectedExceptionMessage = "Value cannot be null. (Parameter 'enrichmentConfiguration')";

        // Act and Assert
        var exception = Assert.Throws<ArgumentNullException>(() => enrichmentConfiguration.WithDisplayName());
        Assert.Equal(expectedExceptionMessage, exception.Message);
    }

    [Fact]
    public void WithUPN_EnrichmentConfigurationIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        LoggerEnrichmentConfiguration enrichmentConfiguration = null;

        var expectedExceptionMessage = "Value cannot be null. (Parameter 'enrichmentConfiguration')";

        // Act and Assert
        var exception = Assert.Throws<ArgumentNullException>(() => enrichmentConfiguration.WithUpn());
        Assert.Equal(expectedExceptionMessage, exception.Message);
    }

    [Fact]
    public void WithTenantId_EnrichmentConfigurationIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        LoggerEnrichmentConfiguration enrichmentConfiguration = null;

        var expectedExceptionMessage = "Value cannot be null. (Parameter 'enrichmentConfiguration')";

        // Act and Assert
        var exception = Assert.Throws<ArgumentNullException>(() => enrichmentConfiguration.WithTenantId());
        Assert.Equal(expectedExceptionMessage, exception.Message);
    }

    [Fact]
    public void WithOID_EnrichmentConfigurationIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        LoggerEnrichmentConfiguration enrichmentConfiguration = null;

        var expectedExceptionMessage = "Value cannot be null. (Parameter 'enrichmentConfiguration')";

        // Act and Assert
        var exception = Assert.Throws<ArgumentNullException>(() => enrichmentConfiguration.WithObjectId());
        Assert.Equal(expectedExceptionMessage, exception.Message);
    }

    [Fact]
    public void WithAppId_EnrichmentConfigurationIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        LoggerEnrichmentConfiguration enrichmentConfiguration = null;

        var expectedExceptionMessage = "Value cannot be null. (Parameter 'enrichmentConfiguration')";

        // Act and Assert
        var exception = Assert.Throws<ArgumentNullException>(() => enrichmentConfiguration.WithAppId());
        Assert.Equal(expectedExceptionMessage, exception.Message);
    }
}
