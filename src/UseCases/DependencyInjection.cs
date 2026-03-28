using Microsoft.Extensions.DependencyInjection;
using UseCases.Projects.ChangeDescription;
using UseCases.Projects.CreateProject;
using UseCases.Projects.DeleteProject;
using UseCases.Projects.GetAllProjects;
using UseCases.Projects.GetProjectById;
using UseCases.Projects.RenameProject;
using UseCases.Projects.ReplaceProject;
using UseCases.Projects.Tasks.ChangeTaskInfo;
using UseCases.Projects.Tasks.CreateTask;
using UseCases.Projects.Tasks.DeleteTask;
using UseCases.Projects.TaskMembers.AddMember;
using UseCases.Projects.TaskMembers.RemoveMember;
namespace UseCases;

/// <summary>
/// Методы расширения для регистрации зависимостей Application-слоя (UseCases)
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Хэндлеры команд
        services.AddTransient<CreateProjectCommandHandler>();
        services.AddTransient<ReplaceProjectCommandHandler>();
        services.AddTransient<RenameProjectCommandHandler>();
        services.AddTransient<ChangeProjectDescriptionCommandHandler>();
        services.AddTransient<DeleteProjectCommandHandler>();
        services.AddTransient<CreateTaskCommandHandler>();
        services.AddTransient<DeleteTaskCommandHandler>();
        services.AddTransient<ChangeTaskInfoCommandHandler>();

        services.AddTransient<AddTaskMemberCommandHandler>();
        services.AddTransient<RemoveTaskMemberCommandHandler>();

        // Хэндлеры запросов
        services.AddTransient<GetAllProjectsQueryHandler>();
        services.AddTransient<GetProjectByIdQueryHandler>();

        return services;
    }
}