using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using System.Security.Claims;

namespace Serilog.Enrichers.Claims;

/// <summary>
/// Enriches log events with a custom property from the user's claims.
/// </summary>
internal class ClaimEnricher : ILogEventEnricher
{
    /// <summary>
    /// The custom claimType to be used to find the claim
    /// </summary>
    private readonly string _claimType;

    /// <summary>
    /// The unknown value to be used when the property value is not available.
    /// </summary>
    protected const string UnknownValue = "unknown";

    /// <summary>
    /// The HTTP context accessor used for retrieving the HTTP context.
    /// </summary>
    protected IHttpContextAccessor _contextAccessor;

    /// <summary>
    /// The key used for storing the log event property in the HTTP context items.
    /// </summary>
    protected string _itemKey;

    /// <summary>
    /// The name of the property to be added to the log event.
    /// </summary>
    protected string _propertyName;


    /// <summary>
    /// Initializes a new instance of the <see cref="ClaimEnricher"/> class.
    /// </summary>
    public ClaimEnricher(string claimType, string? customPropertyName = null)
    {
        var propertyName = customPropertyName ?? claimType;

        _contextAccessor = new HttpContextAccessor();
        _itemKey = $"Serilog_{propertyName}";

        _propertyName = propertyName;
        _claimType = claimType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ClaimEnricher"/> class with the specified HTTP context accessor.
    /// </summary>
    /// <param name="contextAccessor">The HTTP context accessor to use for retrieving the user's claims.</param>
    /// <param name="claimType">The custom claimType to be used to find the claim</param>
    /// <param name="customPropertyName">The custom property name to be used in the enriched logs.</param>
    internal ClaimEnricher(IHttpContextAccessor contextAccessor, string claimType, string? customPropertyName = null)
    {
        var propertyName = customPropertyName ?? claimType;

        _contextAccessor = contextAccessor;
        _itemKey = $"Serilog_{propertyName}";

        _propertyName = propertyName;
        _claimType = claimType;
    }

    /// <summary>
    /// Enriches the specified log event with additional properties.
    /// </summary>
    /// <param name="logEvent">The log event to enrich.</param>
    /// <param name="propertyFactory">The factory used to create log event properties.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = _contextAccessor.HttpContext;
        if (httpContext is null)
            return;

        if (httpContext.User.Identity?.IsAuthenticated != true)
            return;

        if (httpContext.Items[_itemKey] is LogEventProperty logEventProperty)
        {
            logEvent.AddPropertyIfAbsent(logEventProperty);
            return;
        }

        var propertyValue = GetPropertyValue(httpContext.User);
        if (string.IsNullOrEmpty(propertyValue))
            propertyValue = UnknownValue;

        var evtProperty = new LogEventProperty(_propertyName, new ScalarValue(propertyValue));
        httpContext.Items.Add(_itemKey, evtProperty);

        logEvent.AddPropertyIfAbsent(evtProperty);
    }

    /// <summary>
    /// Gets the Custom Claim property value from the specified claims principal.
    /// </summary>
    /// <param name="user">The claims principal representing the user.</param>
    /// <returns>The Custom Claim property value, or <c>null</c> if it cannot be found.</returns>
    protected string? GetPropertyValue(ClaimsPrincipal user)
    {
        return user.FindFirstValue(_claimType);
    }
}