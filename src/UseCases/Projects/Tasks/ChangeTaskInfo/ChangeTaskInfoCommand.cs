public sealed record ChangeTaskInfoCommand(
    Guid ProjectId,
    Guid TaskId,
    string Description
);