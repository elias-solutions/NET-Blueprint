using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using NET.Backend.Blueprint.Api.Authorization;

namespace NET.Backend.Blueprint.Api.Authentication;

public class AuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUserService _userService;

    public AuthenticationHandler(
        IUserService userService,
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _userService = userService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var user = _userService.GetCurrentUser();
        if (user == null)
        {
            return await Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new[] { 
            new Claim(ClaimTypes.NameIdentifier, user!.UserName),
            new Claim(ClaimTypes.Name, user!.UserName),
            new Claim(ClaimTypes.Role, user!.UserName)
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return await Task.FromResult(AuthenticateResult.Success(ticket));
    }
}