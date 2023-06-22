﻿using Serilog.Configuration;
using Xunit;

namespace Serilog.Enrichers.AzureAuthInfo.Tests
{
    public class ExtensionsTests
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
            var exception = Assert.Throws<ArgumentNullException>(() => enrichmentConfiguration.WithUPN());
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
            var exception = Assert.Throws<ArgumentNullException>(() => enrichmentConfiguration.WithObjectIdentifier());
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }
    }
}