using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;

public class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly string _role;

    public TestAuthenticationHandler(
        string role,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _role = role;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var user = _role;
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user),
            new Claim(ClaimTypes.Name, user),
            new Claim(ClaimTypes.Role, user)
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return await Task.FromResult(AuthenticateResult.Success(ticket));
    }
}