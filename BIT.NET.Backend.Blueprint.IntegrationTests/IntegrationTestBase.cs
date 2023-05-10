using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using BIT.NET.Backend.Blueprint.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;


namespace BIT.NET.Backend.Blueprint.IntegrationTests;

public abstract class IntegrationTestBase : WebApplicationFactory<Startup>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(_ =>
        {

        });

        base.ConfigureWebHost(builder);
    }

    protected async Task<TOutput> GetAsync<TOutput>(string route)
    {
        var client = CreateHttpClient();
        var response = await client.GetAsync(route);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadAsync<TOutput>();
        return result;
    }

    protected async Task<TOutput> PostAsync<TInput, TOutput>(string route, TInput model) where TInput : class
    {
        var response = await CreateHttpClient().PostAsync(route, CreateHttpContent(model));
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadAsync<TOutput>();
        return result;
    }

    protected async Task DeleteAsync(string route)
    {
        var response = await CreateHttpClient().DeleteAsync(route);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private HttpContent CreateHttpContent<T>(T model)
    {
        var stringPayload = JsonSerializer.Serialize(model);
        var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
        return httpContent;
    }

    private HttpClient CreateHttpClient()
    {
        var httpClient = Server.CreateClient();
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return httpClient;
    }
}