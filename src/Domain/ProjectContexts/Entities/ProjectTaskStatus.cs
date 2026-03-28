namespace Domain.ProjectContexts.Entities
{
    public sealed record ProjectTaskStatus
    {
        public string Name { get; }
        public string Value { get; }
        public ProjectTaskStatus(string name, string value) 
        {
            Name = name;
            Value = value;
        }

        private ProjectTaskStatus() { } // Для EF
    }
}
