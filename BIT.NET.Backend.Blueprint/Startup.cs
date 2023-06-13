using System.Text.Json.Serialization;
using BIT.NET.Backend.Blueprint.Authentication;
using BIT.NET.Backend.Blueprint.Authorization;
using BIT.NET.Backend.Blueprint.DataAccess;
using BIT.NET.Backend.Blueprint.Mapper;
using BIT.NET.Backend.Blueprint.Repository.Base;
using BIT.NET.Backend.Blueprint.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace BIT.NET.Backend.Blueprint;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
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
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
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