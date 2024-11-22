using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace Serilog.Enrichers.AzureClaims;

/// <summary>
/// Enriches log events with the Display Name property from the user's claims.
/// </summary>
internal class DisplayNameEnricher : BaseEnricher
{
    private const string DisplayNameItemKey = "Serilog_DisplayName";
    private const string DisplayNamePropertyName = "DisplayName";

    /// <summary>
    /// Initializes a new instance of the <see cref="DisplayNameEnricher"/> class.
    /// </summary>
    public DisplayNameEnricher() : base(DisplayNameItemKey, DisplayNamePropertyName) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DisplayNameEnricher"/> class with the specified HTTP context accessor.
    /// </summary>
    /// <param name="contextAccessor">The HTTP context accessor to use for retrieving the user's claims.</param>
    internal DisplayNameEnricher(IHttpContextAccessor contextAccessor) : base(contextAccessor, DisplayNameItemKey, DisplayNamePropertyName) { }

    /// <summary>
    /// Gets the Display Name property value from the specified claims principal.
    /// </summary>
    /// <param name="user">The claims principal representing the user.</param>
    /// <returns>The Display Name property value, or <c>null</c> if it cannot be found.</returns>
    protected override string? GetPropertyValue(ClaimsPrincipal user)
    {
        return user.GetDisplayName();
    }
}