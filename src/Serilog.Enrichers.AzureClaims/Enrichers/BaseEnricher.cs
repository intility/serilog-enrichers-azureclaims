using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;
using System.Security.Claims;

namespace Serilog.Enrichers.AzureClaims;

/// <summary>
/// Represents a base class for log event enrichers.
/// </summary>
internal abstract class BaseEnricher : ILogEventEnricher
{
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
    /// Initializes a new instance of the <see cref="BaseEnricher"/> class with the specified item key and property name.
    /// </summary>
    /// <param name="itemKey">The key used for storing the log event property in the HTTP context items.</param>
    /// <param name="propertyName">The name of the property to be added to the log event.</param>
    protected BaseEnricher(string itemKey, string propertyName)
    {
        _contextAccessor = new HttpContextAccessor();
        _itemKey = itemKey;
        _propertyName = propertyName;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEnricher"/> class with the specified HTTP context accessor, item key, and property name.
    /// </summary>
    /// <param name="contextAccessor">The HTTP context accessor used for retrieving the HTTP context.</param>
    /// <param name="itemKey">The key used for storing the log event property in the HTTP context items.</param>
    /// <param name="propertyName">The name of the property to be added to the log event.</param>
    protected BaseEnricher(IHttpContextAccessor contextAccessor, string itemKey, string propertyName)
    {
        _contextAccessor = contextAccessor;
        _itemKey = itemKey;
        _propertyName = propertyName;
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
    /// Gets the property value to be added to the log event from the specified claims principal.
    /// </summary>
    /// <param name="user">The claims principal representing the user.</param>
    /// <returns>The property value to be added to the log event, or <c>null</c> if it cannot be determined.</returns>
    protected abstract string? GetPropertyValue(ClaimsPrincipal user);
}