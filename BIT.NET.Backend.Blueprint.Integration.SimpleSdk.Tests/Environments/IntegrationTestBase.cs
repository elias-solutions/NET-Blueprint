using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BIT.NET.Backend.Blueprint.Integration.SimpleSdk.Tests.Environments;

public abstract class IntegrationTestBase<TStartup> where TStartup : class
{
    // We use assembly initialize to get better performance. But it is only possible if you change not states on the base
    // properties, otherwise we have to change to to clean up every test.
    //
    // Hint: With this 'AssemblyInitialize' the API is just started once.
    public static Task AssemblyInitializeAsync(TestContext testContext)
    {
        return AssemblyInitializeAsync(testContext, (_, _) => { });
    }

    public static Task AssemblyInitializeAsync(TestContext testContext,
        Action<IServiceCollection, IConfiguration> registerServices)
    {
        // Create this with new, is not a fault, the reason is to keep the test class more cleaner.
        WebApplicationFactory = new CustomWebApplicationFactory<TStartup>(registerServices);
        ServiceProvider = WebApplicationFactory.Services;
        Client = WebApplicationFactory.CreateClient(new WebApplicationFactoryClientOptions {AllowAutoRedirect = false});

        UniqueRunnerName = Environment.MachineName;

        return Task.CompletedTask;
    }

    public static CustomWebApplicationFactory<TStartup> WebApplicationFactory { get; set; } = null!;

    public static IServiceProvider ServiceProvider { get; private set; } = null!;

    protected static string UniqueRunnerName { get; private set; } = string.Empty;

    protected static HttpClient Client { get; private set; } = null!;

    [AssemblyCleanup]
    public static void AssemblyCleanup()
    {
        WebApplicationFactory.Dispose();
        Client.Dispose();
    }
}