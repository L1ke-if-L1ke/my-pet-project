namespace Presenters.DTOs
{
    public sealed record AddTaskMemberRequest(
        string Email,
        string Login
    );
}
