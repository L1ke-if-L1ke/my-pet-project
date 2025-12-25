namespace Domain.ProjectContexts.Entities
{
    public readonly record struct ProjectTaskId
    {
        public Guid Value { get; }

        public ProjectTaskId() => Value = Guid.NewGuid();

        public ProjectTaskId(Guid value) => Value = value;

        public static ProjectTaskId Create(Guid value) =>
            value == Guid.Empty
            ? throw new ArgumentException("Идентификатор задачи некорректный.")
            : new ProjectTaskId(value);
    }
}
