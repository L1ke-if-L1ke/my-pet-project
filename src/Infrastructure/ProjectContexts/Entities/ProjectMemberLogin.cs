namespace Infrastructure.ProjectContexts.Entities
{
    public sealed record ProjectMemberLogin
    {
        public const int MAX_MEMBER_LOGIN = 150;
        public string Value { get; }
        private ProjectMemberLogin(string value)
        {
            Value = value;
        }
        public static ProjectMemberLogin Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Информация о задаче была пустой.");
            if (value.Length > MAX_MEMBER_LOGIN)
                throw new ArgumentException(
                    $"Информация о задаче превышает максимальную длину в {MAX_MEMBER_LOGIN} символов"
                    );
            return new ProjectMemberLogin(value);
        }
    }
}
