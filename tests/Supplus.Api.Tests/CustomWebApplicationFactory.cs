using FluentMigrator.Runner;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Supplus.Domain.Entities;
using Supplus.Domain.Enums;
using Supplus.Infrastructure.Data.Context;
using Supplus.Infrastructure.Data.Migrations;
using System.Reflection;

namespace Supplus.Api.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly ContainersFixture _containerFixture;

    public CustomWebApplicationFactory(ContainersFixture containersFixture)
    {
        _containerFixture = containersFixture;
    }

    public async Task InitializeAsync() => await _containerFixture.InitializeAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        builder.ConfigureServices(services =>
        {
            var descriptorDbContext = services
            .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<SupplusDbContext>));

            if(descriptorDbContext is not null)
                services.Remove(descriptorDbContext);

            var connectionString = _containerFixture.Container.GetConnectionString();

            services.AddDbContext<SupplusDbContext>(options =>
            {
                options.UseNpgsql(_containerFixture.Container.GetConnectionString());
            });

            services.AddFluentMigratorCore().ConfigureRunner(opt =>
            {
                opt.AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(Assembly.Load("Supplus.Infrastructure"))
                    .For
                    .All();
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SupplusDbContext>();
            var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Usuario>>();                      

            ControleMigration.AplicarMigration(connectionString, scope.ServiceProvider);

            if (!dbContext.Usuarios.Any())
            {
                var usuario = new Usuario(email: "admin@admin.com", nome: "Admin", sobrenome: "Admin", 1, role: Role.Administrador);
                usuario.AtribuirSenha(hasher.HashPassword(usuario, password: "SenhaForte123!"));

                dbContext.Usuarios.Add(usuario);
                dbContext.SaveChanges();
            }            
        });            
    }

    async Task IAsyncLifetime.DisposeAsync() => await _containerFixture.DisposeAsync();
}
