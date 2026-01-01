namespace Infrastructure.ProjectContexts.Entities
{
    public sealed record ProjectMemberStatus
    {
        public const int MAX_PROJECT_MEMBER_STATUS = 150;
        public string Value { get; }
        private ProjectMemberStatus(string value)
        {
            Value = value;
        }
        public static ProjectMemberStatus Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Статус участника проекта был пустым.");
            if (value.Length > MAX_PROJECT_MEMBER_STATUS)
                throw new ArgumentException(
                    $"Статус участника проекта превышает максимальную длину в {MAX_PROJECT_MEMBER_STATUS} символов"
                    );
            return new ProjectMemberStatus(value);
        }
    }
}
