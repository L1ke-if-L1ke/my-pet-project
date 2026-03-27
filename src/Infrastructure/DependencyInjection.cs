using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using YourProject.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Npgsql.EntityFrameworkCore.PostgreSQL;


namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config)
    {
        // Регистрируем опции БД из секции "Database"
        services.Configure<DatabaseConnectionOptions>(config.GetSection("Database"));

        // Регистрируем DbContext — он получит IOptions<> через DI
        services.AddDbContext<ApplicationDbContext>();

        services.AddScoped<IProjectRepository, EfProjectRepository>();

        return services;
    }
}