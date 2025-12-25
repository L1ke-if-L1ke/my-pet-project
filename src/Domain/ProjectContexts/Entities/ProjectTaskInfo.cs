namespace Domain.ProjectContexts.Entities
{
    public sealed record ProjectTaskInfo
    {
        public const int MAX_DESCRIPTION_LENGTH = 150;

        public string Description { get; }

        private ProjectTaskInfo(string description)
        {
            Description = description;
        }

        public static ProjectTaskInfo Create(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Информация о задаче была пустой.");

            if (description.Length > MAX_DESCRIPTION_LENGTH)
                throw new ArgumentException(
                    $"Информация о задаче превышает максимальную длину в {MAX_DESCRIPTION_LENGTH} символов"
                );

            return new ProjectTaskInfo(description);
        }
    }
}
