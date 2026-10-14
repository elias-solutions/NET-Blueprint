using Microsoft.EntityFrameworkCore;
using NET.Backend.Blueprint.Application.Extensions;
using NET.Backend.Blueprint.Infrastructure.Extensions;
using NET.Backend.Blueprint.Minimal.Api.Endpoints;
using NET.Backend.Blueprint.Minimal.Api.Extensions;
using NET.Backend.Blueprint.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddPersistence(options => options.UseInMemoryDatabase("Database"))
    .AddCqrs()
    .AddBlueprintSecurity()
    .AddBlueprintProblemDetails()
    .AddValidation()
    .AddBlueprintJson()
    .AddFixedWindowRateLimiting()
    .AddBlueprintVersioning()
    .AddVersionedSwagger(".NET 10 Minimal API");

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseVersionedSwagger();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.MapBlueprintEndpoints();

await app.RunAsync();
