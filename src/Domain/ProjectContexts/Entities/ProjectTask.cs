namespace Domain.ProjectContexts.Entities
{
    /// <summary>
    /// Задача проекта
    /// </summary>
    public sealed class ProjectTask
    {
        /// <summary>
        /// Проект
        /// </summary>
        public Project Project { get; }

        /// <summary>
        /// Идентификатор проекта
        /// </summary>
        public ProjectId ProjectId { get; }

        /// <summary>
        /// Участники задачи
        /// </summary>
        private readonly List<ProjectTaskMemberInfo> _taskMembers = [];

        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public ProjectTaskId Id { get; }

        /// <summary> 
        /// Лимит участников задачи
        /// </summary>
        public ProjectTaskMembersLimit Limit { get; }

        /// <summary>
        /// Статус задачи
        /// </summary>
        public ProjectTaskStatusInfo StatusInfo { get; }

        /// <summary>
        /// Информация о задаче
        /// </summary>
        public ProjectTaskInfo Information { get; }

        /// <summary>
        /// Участники задачи (коллекция только для чтения)
        /// </summary>
        public IReadOnlyList<ProjectTaskMemberInfo> TaskMembers => _taskMembers;

        public ProjectTask(
            ProjectTaskId id,
            ProjectTaskMembersLimit limit,
            ProjectTaskStatusInfo statusInfo,
            ProjectTaskInfo information,
            Project project,
            IEnumerable<ProjectTaskMemberInfo> taskMembers
            )
        {
            Project = project;
            ProjectId = project.Id;
            _taskMembers = taskMembers.ToList();
            Id = id;
            Limit = limit;
            Information = information;
            StatusInfo = statusInfo;
        }
    }
}
