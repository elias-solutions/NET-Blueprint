using System.Net.Http.Headers;
using System.Net.Mime;
using Xunit;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.SQlite.Environment;

public abstract class TestBase : WebApplicationFactory<Startup>, IAsyncLifetime
{
    public IUserService UserService { get; }
    public DatabaseResetProvider DatabaseResetProvider { get; }
    protected HttpClient Client { get; }
    
    protected TestBase()
    {
        UserService = Substitute.For<IUserService>();
        Client = CreateClient();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        DatabaseResetProvider = new DatabaseResetProvider();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {            
        builder
            .ConfigureTestServices(services =>
            {
                services.AddScoped(_ => UserService);
                services.AddAuthentication("TestAuthentication")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>("TestAuthentication", null);
            });

        builder.UseEnvironment("IntegrationTestPostgres");
    }

    public async Task InitializeAsync() => await DatabaseResetProvider.InitializeAsync();

    async Task IAsyncLifetime.DisposeAsync() => await DatabaseResetProvider.DisposeDbConnectionAsync();
}