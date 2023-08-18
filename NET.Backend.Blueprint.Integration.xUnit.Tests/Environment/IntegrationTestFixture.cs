using NET.Backend.Blueprint.Api.Authorization;
using NET.Backend.Blueprint.Extensions;
using NET.Backend.Blueprint.Integration.xUnit.Tests.Extensions;
using NSubstitute;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;

public class IntegrationTestFixture : TestBase
{
    public async Task<HttpResponseMessage> GetAsync(User? user, string route)
    {
        UserService.GetCurrentUser().Returns(user);
        return await Client.GetAsync(route);
    }
    
    public async Task<HttpResponseMessage> PostAsync(User? user, string route)
    {
        UserService.GetCurrentUser().Returns(user);
        return await Client.PostAsync(route, null);
    }
    
    public async Task<HttpResponseMessage> PostAsync(User user, string route, string jsonFileRequest)
    {
        if (string.IsNullOrEmpty(jsonFileRequest))
        {
            return await Client.PostAsync(route, null);
        }

        UserService.GetCurrentUser().Returns(user);
        var jsonString = await jsonFileRequest.ToJsonStringContentAsync();
        return await Client.PostAsync(route, jsonString.ToStringContent());
    }

    public async Task<HttpResponseMessage> PutAsync(User user, string route, string request = "")
    {
        if (string.IsNullOrEmpty(request))
        {
            return await Client.PutAsync(route, new StringContent(string.Empty));
        }

        UserService.GetCurrentUser().Returns(user);
        var jsonString = await request.ToJsonStringContentAsync();
        return await Client.PutAsync(route, jsonString.ToStringContent());
    }

    public async Task<HttpResponseMessage> PutAsync(User user, string route, object? request = null)
    {
        if (request == null)
        {
            return await Client.PutAsync(route, new StringContent(string.Empty));
        }

        UserService.GetCurrentUser().Returns(user);
        return await Client.PutAsync(route, request.ToJson().ToStringContent());
    }

    public async Task<HttpResponseMessage> DeleteAsync(User? user, string route)
    {
        UserService.GetCurrentUser().Returns(user);
        return await Client.DeleteAsync(route);
    }

    public async Task<T> CreateExpected<T>(string request)
    {
        if (string.IsNullOrEmpty(request))
        {
            throw new ArgumentException($"Argument '{nameof(request)}' is null or empty");
        }

        var jsonString = await request.ToJsonStringContentAsync();
        return jsonString.ReadFromJson<T>();
    }
}