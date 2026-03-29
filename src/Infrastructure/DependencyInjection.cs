using Domain.Interfaces;
using Infrastructure.Common;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using UseCases.Interfaces;
using Infrastructure.Transactions;  
using YourProject.Domain.Interfaces;


namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.Configure<DatabaseConnectionOptions>(config.GetSection("Database"));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            // Конфигурация будет применена в OnConfiguring
        });

        services.AddScoped<IProjectRepository, EfProjectRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();           
        services.AddScoped<ITransactionFactory, TransactionFactory>(); 

        return services;
    }
}