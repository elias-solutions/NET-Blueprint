using System.Net;
using System.Text;
using System.Text.Json;
using BIT.NET.Backend.Blueprint.Authorization;
using BIT.NET.Backend.Blueprint.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using NSubstitute;

namespace BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Environments;

public abstract class IntegrationTestBase : TestBase
{
    protected IntegrationTestBase(WebApplicationFactory<Startup> factory) : base(factory)
    {
    }

    protected async Task AssertGetUnauthorizedAsync(string route)
    {
        var response = await Client.GetAsync(route);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    protected async Task AssertPostUnauthorizedAsync(string route)
    {
        var response = await Client.PostAsync(route, new StringContent(string.Empty));
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    protected async Task AssertDeleteUnauthorizedAsync(string route)
    {
        var response = await Client.DeleteAsync(route);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    protected async Task AssertGetHttpStatusCodeAsync(User user, string route, HttpStatusCode statusCode)
    {
        UserService.GetCurrentUser().Returns(user);
        var response = await Client.GetAsync(route);
        response.StatusCode.Should().Be(statusCode);
    }

    protected async Task AssertPostHttpStatusCodeAsync(User user, string route, HttpStatusCode statusCode)
    {
        UserService.GetCurrentUser().Returns(user);
        var response = await Client.PostAsync(route, new StringContent(string.Empty));
        response.StatusCode.Should().Be(statusCode);
    }

    protected async Task AssertDeleteHttpStatusCodeAsync(User user, string route, HttpStatusCode statusCode)
    {
        UserService.GetCurrentUser().Returns(user);
        var response = await Client.DeleteAsync(route);
        response.StatusCode.Should().Be(statusCode);
    }

    protected async Task<T> AssertGetAsync<T>(User user, string route)
    {
        UserService.GetCurrentUser().Returns(user);
        var response = await Client.GetAsync(route);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        return await response.Content.ReadAsync<T>();
    }

    protected async Task<TResponse> AssertPostAsync<TResponse>(User user, string route, object request)
    {
        UserService.GetCurrentUser().Returns(user);
        var httpContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var response = await Client.PostAsync(route, httpContent);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        return await response.Content.ReadAsync<TResponse>();
    }

    protected async Task AssertDeleteAsync(User user, string route)
    {
        UserService.GetCurrentUser().Returns(user);
        var response = await Client.DeleteAsync(route);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}