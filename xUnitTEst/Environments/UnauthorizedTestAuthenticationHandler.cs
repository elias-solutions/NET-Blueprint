using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BIT.NET.Backend.Blueprint.Integration.Tests.Environments;

public class UnauthorizedTestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public UnauthorizedTestAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        return await Task.FromResult(AuthenticateResult.NoResult());
    }
}