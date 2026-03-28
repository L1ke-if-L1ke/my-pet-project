using YourProject.Domain.Interfaces;
using Domain.ProjectContexts;

namespace UseCases.Projects.Tasks.CreateTask;

public sealed class CreateTaskCommandHandler
{
    private readonly IProjectRepository _repository;

    public CreateTaskCommandHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(CreateTaskCommand command, CancellationToken ct)
    {
        var project = await _repository.GetByIdAsync(command.ProjectId, ct);
        if (project == null)
            throw new KeyNotFoundException($"Project {command.ProjectId} not found");

        // Добавляем задачу через доменный метод
        project.AddTask(
            description: command.Description,
            membersLimit: command.MembersLimit
        );

        // Сохраняем изменения через метод интерфейса
        await _repository.UpdateAsync(project, ct);
    }
}