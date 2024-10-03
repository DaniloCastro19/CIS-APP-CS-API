using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using cis_api_legacy_integration_phase_2.Src.Data.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;

namespace cis_api_legacy_integration_phase_2.Src.Core.Auth;

public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly UsersAPIClient _usersAPIClient = new();
    public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder
    ): base(options, logger, encoder)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if(!Request.Headers.ContainsKey("Authorization")) return AuthenticateResult.Fail("Unauthorized. Request doesn't have Auth Key.");

        string authHeader = Request.Headers["Authorization"];
        if(string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("basic ", StringComparison.OrdinalIgnoreCase)) return AuthenticateResult.Fail("Unauthorized. Empty or unvalid Header.");
        
        var token = authHeader.Substring(6);
        var credentialAsString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
        var credentials = credentialAsString.Split(':');
        if(credentials?.Length != 2) return AuthenticateResult.Fail("Unauthorized. Failed Credentials construction.");
        string username = credentials[0];
        string password = credentials[1];

        LoginDTO loginData = new LoginDTO{
            Login = username,
            Password = password,
        };
        //var loginDataToJson = JsonSerializer.Serialize(loginData);

        //var requestBody = new StringContent(loginDataToJson, Encoding.UTF8, "application/json");
        var (successLogin, userId) = await _usersAPIClient.Login(username,password);
         
        if (!successLogin) return AuthenticateResult.Fail("Unauthorized. User not found.");
        
        var claims = new [] {new Claim(ClaimTypes.NameIdentifier, username)};
        var identity = new ClaimsIdentity(claims, "Basic");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
    }
}
