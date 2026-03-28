public sealed record AddTaskMemberCommand(
    Guid ProjectId,
    Guid TaskId,
    string Email,
    string Login
);