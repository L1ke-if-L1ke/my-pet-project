namespace Infrastructure.ProjectContexts.Entities
{
    public sealed record ProjectLifeTime
    {
        public DateOnly CreatedAt { get; }
        public DateOnly? FinishedAt { get; }
    }
}
