namespace Serilog.Enrichers.AzureClaims.Tests.Helpers;

public static class TestConstants
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static string NAME = "Test DisplayName";
    public static string OID = "0f1eddfd-cec1-4444-88f3-07476b7e69ee";
    public static string UPN = "test.upn@example.com";
    public static string TENANTID = "79ce530d-c819-40ed-9a25-eebabd34edfe";
    public static string EMAIL = "test@example.com";
#pragma warning restore CA2211 // Non-constant fields should not be visible
}
