namespace Domain.ProjectContexts.Entities
{
    public readonly record struct ProjectMemberId
    {
        public Guid Value { get; }

        public ProjectMemberId() => Value = Guid.NewGuid();

        public ProjectMemberId(Guid value) => Value = value;

        public static ProjectMemberId Create(Guid value) =>
            value == Guid.Empty
            ? throw new ArgumentException("Идентификатор участника проекта некорректный.")
            : new ProjectMemberId(value);
    }
}
