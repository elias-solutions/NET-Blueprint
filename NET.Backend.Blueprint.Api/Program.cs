using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Api.Authentication;
using NET.Backend.Blueprint.Api.Authorization;
using NET.Backend.Blueprint.Api.Converters;
using NET.Backend.Blueprint.Api.DataAccess;
using NET.Backend.Blueprint.Api.ErrorHandling;
using NET.Backend.Blueprint.Api.Extensions;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

const string rateLimitsPolicyName = "fixed";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ErrorHandlingMiddleware>();
builder.Services.AddDbContextFactory<BlueprintDbContext>(options => options.UseInMemoryDatabase("Database"));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped(typeof(Repository<>));
builder.Services.AddCqrs();

builder.Services.AddAuthentication("Authentication").AddScheme<AuthenticationSchemeOptions, AuthenticationHandler>("Authentication", null);
builder.Services.AddAuthorization();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new DateOnlyNullableJsonConverter());
}); 

builder.Services.AddVersioning();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()));
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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers().RequireRateLimiting(rateLimitsPolicyName);
await app.RunAsync();
//app.UseEndpoints(endpoints => endpoints.MapControllers().RequireRateLimiting(rateLimitsPolicyName));


namespace NET.Backend.Blueprint.Api
{ 
    public partial class Program { }
}
