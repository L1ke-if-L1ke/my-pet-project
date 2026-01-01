namespace Domain.ProjectContexts.Entities
{
    public sealed class ProjectMember
    {
        /// <summary>
        /// Проект в котором состоит участник
        /// </summary>
        public Project Project { get; }

        /// <summary>
        /// Идентификатор участника проекта
        /// </summary>
        public ProjectMemberId Id { get; }

        /// <summary>
        /// Логин участника проекта
        /// </summary>
        public ProjectMemberLogin Login { get; private set; }

        /// <summary>
        /// Статус участника проекта
        /// </summary>
        public ProjectMemberStatus Status { get; private set; }

        public ProjectMember(
            ProjectMemberId id,
            ProjectMemberLogin login,
            ProjectMemberStatus status,
            Project project
            )
        {
            Id = id;
            Login = login;
            Status = status;
            Project = project;
        }
    }
}
