using Microsoft.Extensions.DependencyInjection;
using UseCases.Projects.ChangeDescription;
using UseCases.Projects.CreateProject;
using UseCases.Projects.DeleteProject;
using UseCases.Projects.GetAllProjects;
using UseCases.Projects.GetProjectById;
using UseCases.Projects.RenameProject;
using UseCases.Projects.ReplaceProject;
using YourProject.Domain.Interfaces;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Репозиторий
        services.AddSingleton<IProjectRepository, ProjectsStorage>();

        // Хэндлеры команд и запросов
        services.AddTransient<CreateProjectCommandHandler>();
        services.AddTransient<GetAllProjectsQueryHandler>();
        services.AddTransient<GetProjectByIdQueryHandler>();
        services.AddTransient<DeleteProjectCommandHandler>();
        services.AddTransient<ReplaceProjectCommandHandler>();
        services.AddTransient<RenameProjectCommandHandler>();
        services.AddTransient<ChangeProjectDescriptionCommandHandler>();

        // Операционные хэндлеры (PATCH)
        services.AddTransient<RenameProjectCommandHandler>();
        services.AddTransient<ChangeProjectDescriptionCommandHandler>();

        return services;
    }
}