public sealed record CreateTaskCommand(
    Guid ProjectId,
    string Description,
    int MembersLimit
);