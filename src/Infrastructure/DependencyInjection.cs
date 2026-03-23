using Microsoft.Extensions.DependencyInjection;
using YourProject.Domain.Interfaces;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Репозиторий
        services.AddSingleton<IProjectRepository, ProjectsStorage>();


        return services;
    }
}