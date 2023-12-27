using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Authentication;
using NET.Backend.Blueprint.Api.Authorization;
using NET.Backend.Blueprint.Api.DataAccess;
using NET.Backend.Blueprint.Api.ErrorHandling;
using NET.Backend.Blueprint.Api.Repository;

namespace NET.Backend.Blueprint.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ErrorHandlingMiddleware>();

        services.AddDbContextFactory<BlueprintDbContext>(options => options.UseNpgsql(_configuration.GetConnectionString("Database")));
        services.AddScoped<IUserService, UserService>(); 

        services.AddMediatR(options => options.RegisterServicesFromAssemblyContaining<Program>());
        services.AddScoped(typeof(Repository<>));

        services
            .AddAuthentication("Authentication")
            .AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>("Authentication", null);
        services.AddAuthorization();
        services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options => options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            //app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}