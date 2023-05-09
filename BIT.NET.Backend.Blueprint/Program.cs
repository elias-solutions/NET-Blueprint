using BIT.NET.Backend.Blueprint.DataAccess;
using BIT.NET.Backend.Blueprint.Repository.Base;
using BIT.NET.Backend.Blueprint.Service;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContextFactory<BlueprintDbContext>(options => options.UseInMemoryDatabase("GenericDatabase"), ServiceLifetime.Scoped);

builder.Services.AddScoped(typeof(PersonService));
builder.Services.AddScoped(typeof(Repository<>));
builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
