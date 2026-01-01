
namespace Infrastructure.ProjectContexts.Entities
{
    public sealed record ProjectTaskSchedule
    {
        public DateTime Created { get; }
        public DateTime? Closed { get; }
    }
}
