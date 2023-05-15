using System.Text.Json.Serialization;
using BIT.NET.Backend.Blueprint.Authorization;
using BIT.NET.Backend.Blueprint.DataAccess;
using BIT.NET.Backend.Blueprint.Repository.Base;
using BIT.NET.Backend.Blueprint.Service;
using Microsoft.EntityFrameworkCore;

namespace BIT.NET.Backend.Blueprint;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContextFactory<BlueprintDbContext>(options => options.UseInMemoryDatabase("GenericDatabase"), ServiceLifetime.Scoped);
        services.AddScoped<IUserService, UserService>();
        services.AddScoped(typeof(PersonService));
        services.AddScoped(typeof(Repository<>));
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