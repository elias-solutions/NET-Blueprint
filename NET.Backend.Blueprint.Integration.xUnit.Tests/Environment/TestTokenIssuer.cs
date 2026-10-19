using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using NET.Backend.Blueprint.Web.Authorization;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;

/// <summary>
/// Stands in for Keycloak: signs access tokens like the "blueprint" realm. <see cref="Trust"/> hands its key
/// to the JWT bearer handler, which then validates the tokens exactly as in production.
/// </summary>
public sealed class TestTokenIssuer
{
    /// <summary>Must match <c>Authentication:Schemes:Bearer</c> in appsettings.json.</summary>
    public const string Issuer = "http://localhost:8180/realms/blueprint";
    public const string Audience = "blueprint-api";

    private readonly SecurityKey _signingKey = CreateKey();
    private readonly SecurityKey _foreignKey = CreateKey();
    private readonly JsonWebTokenHandler _handler = new();

    public void Trust(JwtBearerOptions options)
    {
        var configuration = new OpenIdConnectConfiguration { Issuer = Issuer };
        configuration.SigningKeys.Add(_signingKey);
        options.ConfigurationManager = new StaticConfigurationManager<OpenIdConnectConfiguration>(configuration);
    }

    public string Issue(TestUser user) => Issue(user, descriptor => descriptor);

    /// <summary>Issues a token whose descriptor the test may tamper with (issuer, audience, expiry...).</summary>
    public string Issue(TestUser user, Func<SecurityTokenDescriptor, SecurityTokenDescriptor> tamper) =>
        _handler.CreateToken(tamper(Describe(user, _signingKey)));

    public string IssueWithForeignKey(TestUser user) => _handler.CreateToken(Describe(user, _foreignKey));

    private static SecurityTokenDescriptor Describe(TestUser user, SecurityKey key)
    {
        var now = DateTime.UtcNow;
        return new SecurityTokenDescriptor
        {
            Issuer = Issuer,
            Audience = Audience,
            IssuedAt = now,
            NotBefore = now,
            Expires = now.AddMinutes(5),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256),
            Claims = new Dictionary<string, object>
            {
                [JwtRegisteredClaimNames.Sub] = user.Id.ToString(),
                [KeycloakClaims.UserName] = user.UserName,
                [KeycloakClaims.Roles] = user.Roles
            }
        };
    }

    private static RsaSecurityKey CreateKey() => new(RSA.Create(2048)) { KeyId = Guid.NewGuid().ToString() };
}
