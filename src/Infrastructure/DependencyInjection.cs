using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using YourProject.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Common;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<DatabaseOptions>(options =>
        {
            options.ConnectionString = config.GetConnectionString("Postgres")!;
        });

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var dbOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;

            options.UseNpgsql(dbOptions.ConnectionString);
        });

        services.AddScoped<IProjectRepository, EfProjectRepository>();

        return services;
    }
}