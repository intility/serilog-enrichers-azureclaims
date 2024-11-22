using Serilog.Configuration;
using Serilog.Enrichers.AzureClaims;

namespace Serilog;

/// <summary>
///     Extension methods for setting up azure auth enrichers <see cref="LoggerEnrichmentConfiguration"/>.
/// </summary>
public static class AuthenticationInformationLoggerConfigurationExtensions
{
    /// <summary>
    /// Adds a display name enrichment to the logger configuration.
    /// </summary>
    /// <param name="enrichmentConfiguration">The logger enrichment configuration.</param>
    /// <returns>The logger configuration with the display name enrichment added.</returns>
    public static LoggerConfiguration WithDisplayName(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        ArgumentNullException.ThrowIfNull(enrichmentConfiguration, nameof(enrichmentConfiguration));

        return enrichmentConfiguration.With<DisplayNameEnricher>();
    }

    /// <summary>
    /// Adds a user principal name (upn) enrichment to the logger configuration.
    /// </summary>
    /// <param name="enrichmentConfiguration">The logger enrichment configuration.</param>
    /// <remarks>
    /// The user principal name (upn) is an optional claim in the v2 tokens, but will always be present in v1 tokens.
    /// </remarks>
    /// <returns>The logger configuration with the Upn enrichment added.</returns>
    public static LoggerConfiguration WithUpn(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        ArgumentNullException.ThrowIfNull(enrichmentConfiguration, nameof(enrichmentConfiguration));

        return enrichmentConfiguration.With<UpnEnricher>();
    }

    /// <summary>
    /// Adds an object identifier (oid) enrichment to the logger configuration.
    /// </summary>
    /// <param name="enrichmentConfiguration">The logger enrichment configuration.</param>
    /// <returns>The logger configuration with the objectId enrichment added.</returns>
    public static LoggerConfiguration WithObjectId(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        ArgumentNullException.ThrowIfNull(enrichmentConfiguration, nameof(enrichmentConfiguration));

        return enrichmentConfiguration.With<ObjectIdEnricher>();
    }

    /// <summary>
    /// Adds a tenant ID (tid) enrichment to the logger configuration.
    /// </summary>
    /// <param name="enrichmentConfiguration">The logger enrichment configuration.</param>
    /// <returns>The logger configuration with the tenantId enrichment added.</returns>
    public static LoggerConfiguration WithTenantId(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        ArgumentNullException.ThrowIfNull(enrichmentConfiguration, nameof(enrichmentConfiguration));

        return enrichmentConfiguration.With<TenantIdEnricher>();
    }

    /// <summary>
    /// Adds a application ID enrichment to the logger configuration.
    /// </summary>
    /// <param name="enrichmentConfiguration">The logger enrichment configuration.</param>
    /// <remarks>
    /// The application ID value will be populated from appid in v1 tokens and azp in v2 tokens.
    /// </remarks>
    /// <returns>The logger configuration with the AppId enrichment added.</returns>
    public static LoggerConfiguration WithAppId(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        ArgumentNullException.ThrowIfNull(enrichmentConfiguration, nameof(enrichmentConfiguration));

        return enrichmentConfiguration.With<AppIdEnricher>();
    }
}
