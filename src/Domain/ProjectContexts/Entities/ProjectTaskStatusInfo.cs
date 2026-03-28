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
        public static ProjectTaskStatusInfo Create(
            string statusName,
            string statusValue
        )
        {
            return new ProjectTaskStatusInfo(
                new ProjectTaskStatus(statusName, statusValue),
                new ProjectTaskSchedule(DateTime.UtcNow, null)
            );
        }

        // Для EF Core
        private ProjectTaskStatusInfo() { }
    }
}
