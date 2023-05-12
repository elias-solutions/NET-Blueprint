using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIT.NET.Backend.Blueprint.Integration.SimpleSdk.Tests.Environments;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    private readonly Action<IServiceCollection, IConfiguration> _registerServices;

    public CustomWebApplicationFactory() : this((_, _) => { })
    {
    }

    public CustomWebApplicationFactory(Action<IServiceCollection, IConfiguration> registerServices)
    {
        _registerServices = registerServices;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var testSettingsFileInfo = new FileInfo(Path.Combine(Environment.CurrentDirectory, "appsettings.test.json"));
        IConfiguration configuration = null!;

        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            if (testSettingsFileInfo.Exists)
            {
                configurationBuilder.AddJsonFile(testSettingsFileInfo.FullName);
            }

            configuration = configurationBuilder.Build();
        });


        builder.ConfigureServices(services =>
        {
            _registerServices(services, configuration);
            // if we need to switch between services we have to do it here
        });
    }
}