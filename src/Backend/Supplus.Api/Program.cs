using Supplus.Api.Middlewares;
using Supplus.Application;
using Supplus.Infrastructure;
using Supplus.Infrastructure.Data.Migrations;
using Supplus.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Adiciona DI do projeto de Infraestrutura
builder.Services.AdicionaInrastructure(builder.Configuration);

// Adiciona DI do projeto de Application
builder.Services.AdicionaApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();


if(!app.Environment.IsEnvironment("Test"))
    AplicarMigration();

app.Run();

void AplicarMigration()
{
    using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    ControleMigration.AplicarMigration(builder.Configuration.ConnectionString(), scope.ServiceProvider);
}

public partial class Program { }