using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using Supplus.Api.Handlers;
using Supplus.Api.Middlewares;
using Supplus.Api.Requirements;
using Supplus.Api.Tokens;
using Supplus.Application;
using Supplus.Domain.Services.Tokens;
using Supplus.Infrastructure;
using Supplus.Infrastructure.Data.Migrations;
using Supplus.Infrastructure.Extensions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthorizationHandler, RegistrarUsuarioHandler>();

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Config:Jwt:ChaveAssinatura"] ?? 
            throw new ArgumentNullException("É necessária a configuração da cave de assinatura do token JWT."))),
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization(options =>
{    
    options.AddPolicy(Policies.PodeRegistrarUsuario, policy =>
        policy.AddRequirements(new RegistrarUsuarioRequirement()));
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

app.UseAuthentication();
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