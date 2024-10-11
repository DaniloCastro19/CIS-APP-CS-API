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
    /// <summary>//+
    /// Handles the authentication process for incoming requests.//+
    /// </summary>//+
    /// <returns>An AuthenticateResult indicating the outcome of the authentication process.</returns>//+
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Check if the request contains the 'Authorization' header.//+
        if(!Request.Headers.ContainsKey("Authorization")) return AuthenticateResult.Fail("Unauthorized. Request doesn't have Auth Key.");

        string authHeader = Request.Headers["Authorization"];

        // Check if the authorization is basic and if its not null .//+
        if(string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("basic ", StringComparison.OrdinalIgnoreCase)) return AuthenticateResult.Fail("Unauthorized. Empty or unvalid Header.");
        
        var token = authHeader.Substring(6);
        var credentialAsString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
        var credentials = credentialAsString.Split(':');

        // Check if the credentials could be formed .//+
        if(credentials?.Length != 2) return AuthenticateResult.Fail("Unauthorized. Failed Credentials construction.");
        string username = credentials[0];
        string password = credentials[1];

        var (successLogin, userId) = await _usersAPIClient.Login(username, password);
        // Check if login success .//+
        if (!successLogin) return AuthenticateResult.Fail("Unauthorized. User not found.");
        
        var claims = new [] {new Claim(ClaimTypes.NameIdentifier, userId)};
        var identity = new ClaimsIdentity(claims, "Basic");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        return AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name));
    }
}
