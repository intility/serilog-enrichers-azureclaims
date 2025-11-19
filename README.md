# Serilog.Enrichers.AzureClaims and Serilog.Enrichers.Claims
Enriches Serilog events with information from the ClaimsPrincipal.

[![Build_and_Test](https://github.com/Intility/serilog-enrichers-azureclaims/actions/workflows/build_and_test.yaml/badge.svg)](https://github.com/Intility/serilog-enrichers-azureclaims/actions/workflows/build_and_test.yaml)
[![Publish](https://github.com/Intility/serilog-enrichers-azureclaims/actions/workflows/publish.yaml/badge.svg)](https://github.com/Intility/serilog-enrichers-azureclaims/actions/workflows/publish.yaml)

![Nuget](https://img.shields.io/nuget/v/Serilog.Enrichers.AzureClaims?label=Serilog.Enrichers.AzureClaims)
![Nuget](https://img.shields.io/nuget/dt/Serilog.Enrichers.AzureClaims?logo=nuget&label=Downloads)

![Nuget](https://img.shields.io/nuget/v/Serilog.Enrichers.Claims?label=Serilog.Enrichers.Claims)
![Nuget](https://img.shields.io/nuget/dt/Serilog.Enrichers.Claims?logo=nuget&label=Downloads)

Install the _Serilog.Enrichers.AzureClaims_ [NuGet package](https://www.nuget.org/packages/Serilog.Enrichers.AzureClaims/)  
Install the _Serilog.Enrichers.Claims_ [NuGet package](https://www.nuget.org/packages/Serilog.Enrichers.Claims/)

```powershell
Install-Package Serilog.Enrichers.AzureClaims
Install-Package Serilog.Enrichers.Claims
```

Then, apply the enricher to your `LoggerConfiguration`:

```csharp
Log.Logger = new LoggerConfiguration()
    .Enrich.WithUpn()
    .Enrich.WithDisplayName()
    .Enrich.WithTenantId()
    .Enrich.WithObjectId()
    .Enrich.WithAppId()
    .Enrich.WithCustomClaim("AnyExistingClaim") // Available from Serilog.Enrichers.Claims
    // ...other configuration...
    .CreateLogger();
```


### Included enrichers

#### Available from Serilog.Enrichers.AzureClaims

 * `WithUpn()` - adds `UserPrincipalName` based on the ClaimType `http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn`
 * `WithDisplayName()` - adds `DisplayName` based on the ClaimType `http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name` or `name` or `preferred_username`
 * `WithTenantId()` - adds `TenantId` based on the ClaimType `http://schemas.microsoft.com/identity/claims/tenantid` or `tid` 
 * `WithObjectId()` - adds `ObjectId` based on the ClaimType `http://schemas.microsoft.com/identity/claims/objectidentifier` or `oid`
 * `WithAppId()` - adds `AppId` based on the CLaimType `appid` or `azp` 

#### Available from Serilog.Enrichers.Claims

 * `WithCustomClaim("AnyExistingClaim")` based on the claim you want to extract from the ClaimsPrincipal

## Installing into an ASP.NET Core Web Application
The `IHttpContextAccessor` singleton should be registered, but is not required for these nugets to run. The enrichers have access to the requests `HttpContext` to extract the data.
This is what your `Program` class should contain in order for this enricher to work as expected:

```cs
// ...
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSerilog(new LoggerConfiguration()
    .Enrich.WithUpn()
    .Enrich.WithDisplayName()
    .Enrich.WithTenantId()
    .Enrich.WithObjectId()
    .Enrich.WithAppId()
    .Enrich.WithCustomClaim("AnyExistingClaim")
    .CreateLogger());

var app = builder.Build();
app.UseSerilogRequestLogging();
// ...

```
