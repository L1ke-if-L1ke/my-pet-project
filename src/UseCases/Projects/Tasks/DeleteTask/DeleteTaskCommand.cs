public sealed record DeleteTaskCommand(
    Guid ProjectId,
    Guid TaskId
);