using Serilog.Configuration;
using Serilog.Enrichers.Claims;

namespace Serilog;

/// <summary>
///     Extension methods for setting up azure auth enrichers <see cref="LoggerEnrichmentConfiguration"/>.
/// </summary>
public static class AuthenticationInformationLoggerConfigurationExtensions
{
    /// <summary>
    /// Adds a custom claim enrichment to the logger configuration.
    /// </summary>
    /// <param name="enrichmentConfiguration">The logger enrichment configuration.</param>
    /// <param name="claimType">The type of the claim to be used for enrichment.</param>
    /// <param name="propertyName">The name visible in the enriched logs</param>
    /// <returns>The logger configuration with the custom claim enrichment added.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="enrichmentConfiguration"/> or <paramref name="claimType"/> is null.</exception>
    public static LoggerConfiguration WithCustomClaim(this LoggerEnrichmentConfiguration enrichmentConfiguration, string claimType, string? propertyName = null)
    {
        ArgumentNullException.ThrowIfNull(enrichmentConfiguration, nameof(enrichmentConfiguration));

#if (NET6_0)
        if (string.IsNullOrWhiteSpace(claimType))
        {
            throw new ArgumentNullException(nameof(claimType));
        }
#else
        ArgumentNullException.ThrowIfNullOrEmpty(claimType, nameof(claimType));
#endif

        return enrichmentConfiguration.With(new ClaimEnricher(claimType, propertyName));
    }
}
