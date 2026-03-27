namespace Domain.ProjectContexts.Entities
{
    public sealed record ProjectTaskSchedule
    {
        public DateTime Created { get; }
        public DateTime? Closed { get; }
        public ProjectTaskSchedule(DateTime created, DateTime? closed)
        {
            Created = created;
            Closed = closed;
        }
        private ProjectTaskSchedule() { } // Для EF
    }
}
