using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Converters;
using NET.Backend.Blueprint.Api.ErrorHandling;
using NET.Backend.Blueprint.Api.Extensions;
using NET.Backend.Blueprint.Application.Extensions;
using NET.Backend.Blueprint.Infrastructure.Extensions;
using NET.Backend.Blueprint.Web.Extensions;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

const string rateLimitsPolicyName = "fixed";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddPersistence((services, options) =>
    options.UseNpgsql(services.GetRequiredService<IConfiguration>().GetConnectionString("Database")));
builder.Services.AddCqrs();
builder.Services.AddBlueprintSecurity();
builder.Services.AddSingleton<ErrorHandlingMiddleware>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new DateOnlyNullableJsonConverter());
});

builder.Services.AddVersioning();
builder.Services.AddVersionedSwagger(".NET 10 Web API");
builder.Services.AddRateLimiter(options => options.AddFixedWindowLimiter(policyName: rateLimitsPolicyName, opt =>
{
    opt.PermitLimit = 4;
    opt.Window = TimeSpan.FromSeconds(5);
    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    opt.QueueLimit = 2;
}));

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseVersionedSwagger();
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireRateLimiting(rateLimitsPolicyName);
await app.Services.MigrateDatabaseAsync();
await app.RunAsync();


namespace NET.Backend.Blueprint.Api
{
    public partial class Program { }
}
