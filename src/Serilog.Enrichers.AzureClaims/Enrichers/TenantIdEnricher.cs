using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace Serilog.Enrichers.AzureClaims;

/// <summary>
/// Enriches log events with the TenantId property from the user's claims.
/// </summary>
internal class TenantIdEnricher : BaseEnricher
{
    private const string TenantIdItemKey = "Serilog_TenantId";
    private const string TenantIdPropertyName = "TenantId";

    /// <summary>
    /// Initializes a new instance of the <see cref="TenantIdEnricher"/> class.
    /// </summary>
    public TenantIdEnricher() : base(TenantIdItemKey, TenantIdPropertyName) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="TenantIdEnricher"/> class with the specified HTTP context accessor.
    /// </summary>
    /// <param name="contextAccessor">The HTTP context accessor to use for retrieving the user's claims.</param>
    internal TenantIdEnricher(IHttpContextAccessor contextAccessor) : base(contextAccessor, TenantIdItemKey, TenantIdPropertyName) { }

    /// <summary>
    /// Gets the TenantId property value from the specified claims principal.
    /// </summary>
    /// <param name="user">The claims principal representing the user.</param>
    /// <returns>The TenantId property value, or <c>null</c> if it cannot be found.</returns>
    protected override string? GetPropertyValue(ClaimsPrincipal user)
    {
        return user.GetTenantId();
    }
}

