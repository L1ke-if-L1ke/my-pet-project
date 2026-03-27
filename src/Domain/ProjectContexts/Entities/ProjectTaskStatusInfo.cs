namespace Domain.ProjectContexts.Entities
{
    public sealed record ProjectTaskStatusInfo
    {
        public ProjectTaskStatus Status { get; }
        public ProjectTaskSchedule Schedule { get; }

        // Для домена
        public ProjectTaskStatusInfo(ProjectTaskStatus status, ProjectTaskSchedule schedule)
        {
            Status = status;
            Schedule = schedule;
        }

        // Для EF Core
        private ProjectTaskStatusInfo() { }
    }
}
