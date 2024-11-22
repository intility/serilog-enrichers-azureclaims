using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Serilog.Enrichers.AzureClaims;

/// <summary>
/// Enriches log events with a custom property from the user's claims.
/// </summary>
internal class CustomClaimEnricher : BaseEnricher
{
    private readonly string _customClaimType;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomClaimEnricher"/> class.
    /// </summary>
    public CustomClaimEnricher(string claimType, string? customPropertyName = null) : base($"Serilog_{customPropertyName ?? claimType}", customPropertyName ?? claimType)
    {
        _customClaimType = claimType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomClaimEnricher"/> class with the specified HTTP context accessor.
    /// </summary>
    /// <param name="contextAccessor">The HTTP context accessor to use for retrieving the user's claims.</param>
    /// <param name="claimType">The custom claimType to be used to find the claim</param>
    /// <param name="customPropertyName">The custom property name to be used in the enriched logs.</param>
    internal CustomClaimEnricher(IHttpContextAccessor contextAccessor, string claimType, string? customPropertyName = null) : base(contextAccessor, $"Serilog_{customPropertyName ?? claimType}", customPropertyName ?? claimType)
    {
        _customClaimType = claimType;
    }

    /// <summary>
    /// Gets the Custom Claim property value from the specified claims principal.
    /// </summary>
    /// <param name="user">The claims principal representing the user.</param>
    /// <returns>The Custom Claim property value, or <c>null</c> if it cannot be found.</returns>
    protected override string? GetPropertyValue(ClaimsPrincipal user)
    {
        return user.FindFirstValue(_customClaimType);
    }
}