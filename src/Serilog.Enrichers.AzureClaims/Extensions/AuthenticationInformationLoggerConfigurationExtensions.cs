﻿using Serilog.Configuration;
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
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="enrichmentConfiguration"/> is null.</exception>
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
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="enrichmentConfiguration"/> is null.</exception>
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
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="enrichmentConfiguration"/> is null.</exception>
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
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="enrichmentConfiguration"/> is null.</exception>
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
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="enrichmentConfiguration"/> is null.</exception>
    public static LoggerConfiguration WithAppId(this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        ArgumentNullException.ThrowIfNull(enrichmentConfiguration, nameof(enrichmentConfiguration));

        return enrichmentConfiguration.With<AppIdEnricher>();
    }

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

        return enrichmentConfiguration.With(new CustomClaimEnricher(claimType, propertyName));
    }
}
