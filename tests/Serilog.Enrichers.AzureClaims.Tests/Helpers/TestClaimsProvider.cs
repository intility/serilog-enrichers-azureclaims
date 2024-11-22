using Microsoft.Identity.Web;
using System.Security.Claims;

namespace Serilog.Enrichers.AzureClaims.Tests.Helpers;

public class TestClaimsProvider
{
    private List<Claim> Claims { get; } = [];

    public ClaimsPrincipal GetClaimsPrincipal()
    {
        var identity = new ClaimsIdentity(Claims, "TestAuthentication");
        return new ClaimsPrincipal(identity);
    }

    public TestClaimsProvider AddClaim(string type, string value)
    {
        Claims.Add(new Claim(type, value));
        return this;
    }

    public static TestClaimsProvider NotValidClaims()
    {
        return new TestClaimsProvider();
    }

    public static TestClaimsProvider ValidClaims()
    {
        return new TestClaimsProvider()
            .AddClaim(ClaimTypes.Name, TestConstants.NAME)
            .AddClaim(ClaimConstants.ObjectId, TestConstants.OID)
            .AddClaim(ClaimTypes.Upn, TestConstants.UPN)
            .AddClaim(ClaimConstants.Tid, TestConstants.TENANTID)
            .AddClaim(ClaimTypes.Email, TestConstants.EMAIL);
    }
}
