using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;
using YourProject.Domain.Interfaces;

namespace UseCases.Projects.CreateProject;

public sealed class CreateProjectCommandHandler
{
    private readonly IProjectRepository _repository;

    public CreateProjectCommandHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<Project> Handle(CreateProjectCommand command, CancellationToken ct)
    {
        // Валидация домена
        var name = ProjectName.Create(command.Name);
        var description = ProjectDescription.Create(command.Description);

        var project = new Project(
            id: ProjectId.Create(Guid.NewGuid()),
            lifeTime: ProjectLifeTime.Create(DateOnly.FromDateTime(DateTime.UtcNow), null),
            description: description,
            name: name,
            tasks: Array.Empty<ProjectTask>()
        );

        await _repository.AddAsync(project, ct);

        return project;
    }
}