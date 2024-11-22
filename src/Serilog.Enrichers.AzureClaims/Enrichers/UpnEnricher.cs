using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Serilog.Enrichers.AzureClaims;

/// <summary>
/// Enriches log events with the User Principal Name (UPN) property from the user's claims.
/// </summary>
internal class UpnEnricher : BaseEnricher
{
    private const string UPNItemKey = "Serilog_UPN";
    private const string UPNPropertyName = "UserPrincipalName";

    /// <summary>
    /// Initializes a new instance of the <see cref="UpnEnricher"/> class.
    /// </summary>
    public UpnEnricher() : base(UPNItemKey, UPNPropertyName) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="UpnEnricher"/> class with the specified HTTP context accessor.
    /// </summary>
    /// <param name="contextAccessor">The HTTP context accessor to use for retrieving the user's claims.</param>
    internal UpnEnricher(IHttpContextAccessor contextAccessor) : base(contextAccessor, UPNItemKey, UPNPropertyName) { }

    /// <summary>
    /// Gets the user principal name (UPN) property value from the specified claims principal.
    /// </summary>
    /// <param name="user">The claims principal representing the user.</param>
    /// <returns>The user principal name (UPN) property value, or <c>null</c> if it cannot be found.</returns>
    protected override string? GetPropertyValue(ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Upn);
    }
}

