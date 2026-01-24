using Supplus.Infrastructure;
using Supplus.Infrastructure.Data.Migrations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Adiciona DI do projeto de Infraestrutura
builder.Services.AdicionaInrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

AplicarMigration();

app.Run();

void AplicarMigration()
{
    using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    ControleMigration.AplicarMigration(builder.Configuration.GetConnectionString("PostgreSql"), scope.ServiceProvider);
}