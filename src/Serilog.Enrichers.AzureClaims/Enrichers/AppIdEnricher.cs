using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Serilog.Enrichers.AzureClaims;

/// <summary>
/// Enriches log events with the AppId property from the user's claims.
/// </summary>
internal class AppIdEnricher : BaseEnricher
{
    private const string AzpItemKey = "Serilog_AppId";
    private const string AzpPropertyName = "AppId";

    /// <summary>
    /// Initializes a new instance of the <see cref="AppIdEnricher"/> class.
    /// </summary>
    public AppIdEnricher() : base(AzpItemKey, AzpPropertyName) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppIdEnricher"/> class with the specified HTTP context accessor.
    /// </summary>
    /// <param name="contextAccessor">The HTTP context accessor to use for retrieving the user's claims.</param>
    internal AppIdEnricher(IHttpContextAccessor contextAccessor) : base(contextAccessor, AzpItemKey, AzpPropertyName) { }

    /// <summary>
    /// Gets the AppId property value from the specified claims principal.
    /// </summary>
    /// <param name="user">The claims principal representing the user.</param>
    /// <returns>The AppId property value, or <c>null</c> if it cannot be found.</returns>
    protected override string? GetPropertyValue(ClaimsPrincipal user)
    {
        return user.FindFirstValue("appid") ?? user.FindFirstValue("azp");
    }
}
