using YourProject.Domain.Interfaces;

namespace UseCases.Projects.TaskMembers.AddMember;

public sealed class AddTaskMemberCommandHandler
{
    private readonly IProjectRepository _repository;

    public AddTaskMemberCommandHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(AddTaskMemberCommand command, CancellationToken ct)
    {
        var project = await _repository.GetByIdAsync(command.ProjectId, ct);

        if (project is null)
            throw new KeyNotFoundException($"Project {command.ProjectId} not found");

        project.AddMemberToTask(
            taskId: command.TaskId,
            email: command.Email,
            login: command.Login
        );

        await _repository.UpdateAsync(project, ct);
    }
}