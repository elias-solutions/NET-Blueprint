using System.Net;
using System.Net.Http.Json;
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

    protected async Task<HttpResponseMessage> GetAsync(User? user, string route)
    {
        UserService.GetCurrentUser().Returns(user);
        return await Client.GetAsync(route);
    }

    protected async Task<HttpResponseMessage> PostAsync(User? user, string route)
    {
        UserService.GetCurrentUser().Returns(user);
        return await Client.PostAsync(route, new StringContent(string.Empty));
    }

    protected async Task<HttpResponseMessage> PostAsync(User user, string route, object request)
    {
        UserService.GetCurrentUser().Returns(user);
        return await Client.PostAsync(route, request.ToStringContent());
    }

    protected async Task<HttpResponseMessage> PutAsync(User user, string route, object request)
    {
        UserService.GetCurrentUser().Returns(user);
        return await Client.PutAsJsonAsync(route, request);
    }

    protected async Task<HttpResponseMessage> DeleteAsync(User? user, string route)
    {
        UserService.GetCurrentUser().Returns(user);
        return await Client.DeleteAsync(route);
    }
}