namespace Infrastructure.ProjectContexts.Entities
{
    public sealed record ProjectTaskStatusInfo
    {
        public ProjectTaskStatus Status { get; }
        public ProjectTaskSchedule Schedule { get; }
    }
}
