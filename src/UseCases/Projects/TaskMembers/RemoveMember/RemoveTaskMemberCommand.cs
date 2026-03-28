public sealed record RemoveTaskMemberCommand(
    Guid ProjectId,
    Guid TaskId,
    Guid MemberId
);