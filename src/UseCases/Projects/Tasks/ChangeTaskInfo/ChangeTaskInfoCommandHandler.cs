using YourProject.Domain.Interfaces;

namespace UseCases.Projects.Tasks.ChangeTaskInfo;

public sealed class ChangeTaskInfoCommandHandler
{
    private readonly IProjectRepository _repository;

    public ChangeTaskInfoCommandHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(ChangeTaskInfoCommand command, CancellationToken ct)
    {
        var project = await _repository.GetByIdAsync(command.ProjectId, ct);

        if (project is null)
            throw new KeyNotFoundException($"Project {command.ProjectId} not found");

        project.ChangeTaskInfo(
            taskId: command.TaskId,
            description: command.Description
        );
    }
}