using NET.Backend.Blueprint.Api.Authorization;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;

public class IntegrationTestFixture : TestBase
{
    public async Task<HttpResponseMessage> SendAnonymousAsync(string route, HttpMethod method)
    {
        var message = CreateHttpRequestMessage(null, route, method);

        return await Client.SendAsync(message);
    }

    public async Task<HttpResponseMessage> SendAsync(User user, string route, HttpMethod method)
    {
        var message = CreateHttpRequestMessage(user, route, method);
        return await Client.SendAsync(message);
    }

    public async Task<HttpResponseMessage> GetAsync(User user, string route)
    {
        UserService.GetCurrentUser().Returns(user);
        return await Client.GetAsync(route);
    }
    
    public async Task<HttpResponseMessage> PostAsync(User user, string route, StringContent content)
    {
        UserService.GetCurrentUser().Returns(user);
        return await Client.PostAsync(route, content);
    }

    public async Task<HttpResponseMessage> PutAsync(User user, string route, StringContent content)
    {
        UserService.GetCurrentUser().Returns(user);
        return await Client.PutAsync(route, content);
    }
    
    public async Task<HttpResponseMessage> DeleteAsync(User user, string route)
    {
        UserService.GetCurrentUser().Returns(user);
        return await Client.DeleteAsync(route);
    }

    private HttpRequestMessage CreateHttpRequestMessage(User? user, string route, HttpMethod method)
    {
        UserService.GetCurrentUser().Returns(user);
        var message = new HttpRequestMessage { Method = method, RequestUri = new Uri(route, UriKind.Relative) };
        return message;
    }
}