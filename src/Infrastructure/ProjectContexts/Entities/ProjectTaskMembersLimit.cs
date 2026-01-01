namespace Infrastructure.ProjectContexts.Entities
{
    public readonly record struct ProjectTaskMembersLimit
    {
        public const int MAX_TASK_MEMBERS_LIMIT = 100;

        public int Value { get; }
        private ProjectTaskMembersLimit(int value)
        {
            Value = value;
        }
        public static ProjectTaskMembersLimit Create(int value)
        {
            if (value<=0)
                throw new ArgumentException("Лимит участников всегда больше нуля");
            if (value > MAX_TASK_MEMBERS_LIMIT)
                throw new ArgumentException(
                    $"Лимит участников превышает максимальное в {MAX_TASK_MEMBERS_LIMIT} человек"
                    );
            return new ProjectTaskMembersLimit(value);
        }
    }
}
