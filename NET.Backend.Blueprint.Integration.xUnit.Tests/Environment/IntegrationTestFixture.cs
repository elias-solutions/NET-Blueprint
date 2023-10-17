using NET.Backend.Blueprint.Api.Authorization;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Environment;

public class IntegrationTestFixture : TestBase
{
    public async Task<HttpResponseMessage> SendAnonymousAsync(string route, HttpMethod method)
    {
        UserService.GetCurrentUser().ReturnsNull();

        var message = new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri(route, UriKind.Relative)
        };

        return await Client.SendAsync(message);
    }

    public async Task<HttpResponseMessage> SendAsync(User user, string route, HttpMethod method)
    {
        UserService.GetCurrentUser().Returns(user);

        var message = new HttpRequestMessage
        {
            Method = method,
            RequestUri = new Uri(route, UriKind.Relative)
        }; 
        
        return await Client.SendAsync(message);
    }

    public async Task<HttpResponseMessage> SendAsync(User user, string route, HttpMethod method, HttpContent content)
    {
        UserService.GetCurrentUser().Returns(user);

        var message = new HttpRequestMessage { 
            Method = method, 
            RequestUri = new Uri(route, UriKind.Relative),
            Content = content
        };

        return await Client.SendAsync(message);
    }
}