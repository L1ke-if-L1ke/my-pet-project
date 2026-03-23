using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;
using YourProject.Domain.Interfaces;

namespace UseCases.Projects.RenameProject;

public sealed class RenameProjectCommandHandler
{
    private readonly IProjectRepository _repository;

    public RenameProjectCommandHandler(IProjectRepository repository) =>
        _repository = repository;

    public async Task<Project> Handle(RenameProjectCommand command, CancellationToken ct)
    {
        var project = await _repository.GetByIdAsync(command.Id, ct)
            ?? throw new KeyNotFoundException($"Project with id {command.Id} not found");

        // Доменная валидация через Value Object
        var newName = ProjectName.Create(command.NewName);

        // Создаём новый агрегат с обновлённым именем (иммутабельность)
        var renamed = new Project(
            id: project.Id,
            lifeTime: project.LifeTime,
            description: project.Description,
            name: newName,
            tasks: project.Tasks
        );

        await _repository.UpdateAsync(renamed, ct);
        return renamed;
    }
}