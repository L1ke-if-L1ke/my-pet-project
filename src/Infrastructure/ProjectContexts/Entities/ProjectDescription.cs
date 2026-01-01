namespace Infrastructure.ProjectContexts.Entities
{
    public sealed record ProjectDescription
    {
        public const int MAX_PROJECT_DESCRIPTION_LENGTH = 150;
        public string Value { get; }
        private ProjectDescription(string value)
        {
            Value = value;
        }
        public static ProjectDescription Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Описание проекта было пустым.");
            if (value.Length > MAX_PROJECT_DESCRIPTION_LENGTH)
                throw new ArgumentException(
                    $"Описание проекта превышает максимальную длину в {MAX_PROJECT_DESCRIPTION_LENGTH} символов"
                    );
            return new ProjectDescription(value);
        }
    }
}
