using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;
using YourProject.Domain.Interfaces;

namespace UseCases.Projects.ChangeDescription;

public sealed class ChangeProjectDescriptionCommandHandler
{
    private readonly IProjectRepository _repository;

    public ChangeProjectDescriptionCommandHandler(IProjectRepository repository) =>
        _repository = repository;

    public async Task<Project> Handle(ChangeProjectDescriptionCommand command, CancellationToken ct)
    {
        var project = await _repository.GetByIdAsync(command.Id, ct)
            ?? throw new KeyNotFoundException($"Project with id {command.Id} not found");

        var newDescription = ProjectDescription.Create(command.NewDescription);

        var updated = new Project(
            id: project.Id,
            lifeTime: project.LifeTime,
            description: newDescription,
            name: project.Name,
            tasks: project.Tasks
        );

        await _repository.UpdateAsync(updated, ct);
        return updated;
    }
}