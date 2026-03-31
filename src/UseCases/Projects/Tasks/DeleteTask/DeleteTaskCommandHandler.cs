using YourProject.Domain.Interfaces;

namespace UseCases.Projects.Tasks.DeleteTask;

public sealed class DeleteTaskCommandHandler
{
    private readonly IProjectRepository _repository;

    public DeleteTaskCommandHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteTaskCommand command, CancellationToken ct)
    {
        var project = await _repository.GetByIdAsync(command.ProjectId, ct);

        if (project is null)
            throw new KeyNotFoundException($"Project {command.ProjectId} not found");

        project.RemoveTask(command.TaskId);
    }
}