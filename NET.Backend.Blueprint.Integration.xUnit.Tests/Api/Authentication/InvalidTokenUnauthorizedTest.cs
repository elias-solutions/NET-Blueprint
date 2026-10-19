using System.Net;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Api.Authentication;

[Collection(nameof(SharedTestCollection))]
public class InvalidTokenUnauthorizedTest(Fixture fixture)
{
    private const string Route = "/api/v1/persons";

    [Fact]
    public Task ForeignSignature_Unauthorized() =>
        AssertUnauthorizedAsync(fixture.Tokens.IssueWithForeignKey(TestUsers.Admin));

    [Fact]
    public Task ForeignIssuer_Unauthorized() =>
        AssertUnauthorizedAsync(fixture.Tokens.Issue(TestUsers.Admin, token =>
        {
            token.Issuer = "http://localhost:8180/realms/other";
            return token;
        }));

    [Fact]
    public Task ForeignAudience_Unauthorized() =>
        AssertUnauthorizedAsync(fixture.Tokens.Issue(TestUsers.Admin, token =>
        {
            token.Audience = "account";
            return token;
        }));

    [Fact]
    public Task ExpiredToken_Unauthorized() =>
        AssertUnauthorizedAsync(fixture.Tokens.Issue(TestUsers.Admin, token =>
        {
            token.IssuedAt = token.NotBefore = DateTime.UtcNow.AddHours(-2);
            token.Expires = DateTime.UtcNow.AddHours(-1);
            return token;
        }));

    private async Task AssertUnauthorizedAsync(string accessToken)
    {
        var response = await fixture.SendWithTokenAsync(HttpMethod.Get, Route, null, accessToken);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
