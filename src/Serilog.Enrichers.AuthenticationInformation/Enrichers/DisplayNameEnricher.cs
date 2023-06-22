﻿using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;
using Serilog.Enrichers.AuthenticationInformation.Enrichers;
using System.Security.Claims;

namespace Serilog.Enrichers.AuthenticationInformation;

public class DisplayNameEnricher : BaseEnricher
{
    private const string DisplayNameItemKey = "Serilog_DisplayName";
    private const string DisplayNamePropertyName = "DisplayName";

    public DisplayNameEnricher() : base(DisplayNameItemKey, DisplayNamePropertyName) { }

    public DisplayNameEnricher(IHttpContextAccessor contextAccessor) : base(contextAccessor, DisplayNameItemKey, DisplayNamePropertyName) { }

    protected override string GetPropertyValue(ClaimsPrincipal user)
    {
        return user?.GetDisplayName() ?? UnknownValue;
    }
}
