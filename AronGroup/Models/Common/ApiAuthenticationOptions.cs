using Microsoft.AspNetCore.Authentication;

namespace AronGroup.Models.Common;

public class ApiAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string DefaultScheme = "Key";
    public string TokenHeaderName { get; set; } = "ApiKey";
}
