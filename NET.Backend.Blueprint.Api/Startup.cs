using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Authentication;
using NET.Backend.Blueprint.Api.Authorization;
using NET.Backend.Blueprint.Api.DataAccess;
using NET.Backend.Blueprint.Api.ErrorHandling;
using NET.Backend.Blueprint.Api.Extensions;

namespace NET.Backend.Blueprint.Api;

public class Startup(IConfiguration configuration)
{
    private const string RateLimitsPolicyName = "fixed";

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ErrorHandlingMiddleware>();
        services.AddDbContextFactory<BlueprintDbContext>((sp, options) =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Database"))
                .AddInterceptors(new CommandInterceptor(sp));
        });
        services.AddScoped<IUserService, UserService>();

        services.AddScoped(typeof(Repository<>));
        services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining<Program>());

        services
            .AddAuthentication("Authentication")
            .AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>("Authentication", null);
        services.AddAuthorization();
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        services.AddVersioning();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options => options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()));
        services.AddRateLimiter(options => options.AddFixedWindowLimiter(policyName: RateLimitsPolicyName, opt =>
        {
            opt.PermitLimit = 4;
            opt.Window = TimeSpan.FromSeconds(5);
            opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            opt.QueueLimit = 2;
        }));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRateLimiter();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers().RequireRateLimiting(RateLimitsPolicyName));
    }
}