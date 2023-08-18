using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Authentication;
using NET.Backend.Blueprint.Authorization;
using NET.Backend.Blueprint.DataAccess;
using NET.Backend.Blueprint.ErrorHandling;
using NET.Backend.Blueprint.Mapper;
using NET.Backend.Blueprint.Repository.Base;
using NET.Backend.Blueprint.Service;

namespace NET.Backend.Blueprint;

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

        services.AddDbContextFactory<BlueprintDbContext>(options => options.UseNpgsql(_configuration.GetConnectionString("Database")), ServiceLifetime.Scoped);
        services.AddScoped<IUserService, UserService>(); 
        
        services.AddScoped<PersonToDtoMapper>();
        services.AddScoped<PersonToEntityMapper>();
        services.AddScoped<AddressToDtoMapper>();
        services.AddScoped<AddressToEntityMapper>();

        services.AddScoped(typeof(PersonService));
        services.AddScoped(typeof(AddressService));
        services.AddScoped(typeof(Repository<>));
        services
            .AddAuthentication("Authentication")
            .AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>("Authentication", null);
        services.AddAuthorization();
        services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
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