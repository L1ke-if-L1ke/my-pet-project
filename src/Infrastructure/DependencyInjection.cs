using Microsoft.Extensions.DependencyInjection;
using YourProject.Domain.Interfaces;
using UseCases.Projects.CreateProject;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IProjectRepository, ProjectsStorage>();
        services.AddTransient<CreateProjectCommandHandler>();
        return services;
    }
}