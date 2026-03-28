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
        public ProjectId ProjectId { get; private set; }

        /// <summary>
        /// Участники задачи
        /// </summary>
        private readonly List<ProjectTaskMemberInfo> _taskMembers = [];

        /// <summary>
        /// Идентификатор задачи
        /// </summary>
        public ProjectTaskId Id { get; private set; }

        /// <summary> 
        /// Лимит участников задачи
        /// </summary>
        public ProjectTaskMembersLimit Limit { get; private set; }

        /// <summary>
        /// Статус задачи
        /// </summary>
        public ProjectTaskStatusInfo StatusInfo { get; private set; }

        /// <summary>
        /// Информация о задаче
        /// </summary>
        public ProjectTaskInfo Information { get; private set; }

        /// <summary>
        /// Участники задачи (коллекция только для чтения)
        /// </summary>
        public IReadOnlyList<ProjectTaskMemberInfo> TaskMembers => _taskMembers;

        // Для EF Core
        protected ProjectTask()
        {

        }

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
        // -----------------------------
        // Методы управления задачей
        // -----------------------------

        public void ChangeInformation(ProjectTaskInfo newInfo)
        {
            if (newInfo == null)
                throw new ArgumentNullException(nameof(newInfo));

            Information = newInfo;
        }

        public void AddMember(ProjectTaskMemberInfo member)
        {
            if (_taskMembers.Count >= Limit.Value)
                throw new InvalidOperationException("Task member limit reached");

            if (_taskMembers.Any(m => m.MemberId == member.MemberId))
                throw new ArgumentException("Member already exists in task", nameof(member));

            _taskMembers.Add(member);
        }

        public void RemoveMember(ProjectMemberId memberId)
        {
            var member = _taskMembers.FirstOrDefault(m => m.MemberId == memberId);
            if (member == null)
                throw new KeyNotFoundException($"Member {memberId.Value} not found in task");

            _taskMembers.Remove(member);
        }
    }
}
