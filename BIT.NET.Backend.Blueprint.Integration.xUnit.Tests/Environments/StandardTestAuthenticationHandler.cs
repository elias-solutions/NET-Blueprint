using System.Text.Encodings.Web;
using BIT.NET.Backend.Blueprint.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;

public class StandardTestAuthenticationHandler : TestAuthenticationHandler
{
    public StandardTestAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(Roles.Standard, options, logger, encoder, clock)
    {
    }
}