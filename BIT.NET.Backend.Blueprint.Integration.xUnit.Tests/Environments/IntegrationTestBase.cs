using BIT.NET.Backend.Blueprint.Authorization;
using BIT.NET.Backend.Blueprint.Extensions;
using BIT.NET.Backend.Blueprint.Integration.xUnit.Tests.Extensions;
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
    
    protected async Task<HttpResponseMessage> PostAsync(User user, string route, string request = "")
    {
        if (string.IsNullOrEmpty(request))
        {
            return await Client.PostAsync(route, new StringContent(string.Empty));
        }

        UserService.GetCurrentUser().Returns(user);
        var jsonString = await request.ToJsonStringContentAsync();
        return await Client.PostAsync(route, jsonString.ToStringContent());
    }

    protected async Task<HttpResponseMessage> PutAsync(User user, string route, string request = "")
    {
        if (string.IsNullOrEmpty(request))
        {
            return await Client.PutAsync(route, new StringContent(string.Empty));
        }

        UserService.GetCurrentUser().Returns(user);
        var jsonString = await request.ToJsonStringContentAsync();
        return await Client.PutAsync(route, jsonString.ToStringContent());
    }

    protected async Task<HttpResponseMessage> PutAsync(User user, string route, object? request = null)
    {
        if (request == null)
        {
            return await Client.PutAsync(route, new StringContent(string.Empty));
        }

        UserService.GetCurrentUser().Returns(user);
        return await Client.PutAsync(route, request.ToJson().ToStringContent());
    }

    protected async Task<HttpResponseMessage> DeleteAsync(User? user, string route)
    {
        UserService.GetCurrentUser().Returns(user);
        return await Client.DeleteAsync(route);
    }

    protected async Task<T> CreateExpected<T>(string request)
    {
        if (string.IsNullOrEmpty(request))
        {
            throw new ArgumentException($"Argument '{nameof(request)}' is null or empty");
        }

        var jsonString = await request.ToJsonStringContentAsync();
        return jsonString.ReadFromJson<T>();
    }
}