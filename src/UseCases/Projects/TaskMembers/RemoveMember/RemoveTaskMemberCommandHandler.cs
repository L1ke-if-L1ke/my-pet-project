using Domain.ProjectContexts.Entities;
using YourProject.Domain.Interfaces;

namespace UseCases.Projects.TaskMembers.RemoveMember;

public sealed class RemoveTaskMemberCommandHandler
{
    private readonly IProjectRepository _repository;

    public RemoveTaskMemberCommandHandler(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveTaskMemberCommand command, CancellationToken ct)
    {
        var project = await _repository.GetByIdAsync(command.ProjectId, ct);

        if (project is null)
            throw new KeyNotFoundException($"Project {command.ProjectId} not found");

        project.RemoveMemberFromTask(
            taskId: command.TaskId,
            memberId: ProjectMemberId.Create(command.MemberId)
        );

        await _repository.UpdateAsync(project, ct);
    }
}