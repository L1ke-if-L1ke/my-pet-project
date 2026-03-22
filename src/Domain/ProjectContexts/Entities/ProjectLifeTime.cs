namespace Domain.ProjectContexts.Entities
{
    public sealed record ProjectLifeTime
    {
        public DateOnly CreatedAt { get; init; }
        public DateOnly? FinishedAt { get; init; }
        public static ProjectLifeTime Create(DateOnly createdAt, DateOnly? finishedAt = null) =>
            new ProjectLifeTime { CreatedAt = createdAt, FinishedAt = finishedAt };
    }
}
