using Domain.ProjectContexts.Entities;

namespace Domain.ProjectContexts
{
    /// <summary>
    /// Проект
    /// </summary>
    public sealed class Project
    {
        /// <summary>
        /// Задачи проекта
        /// </summary>
        private readonly List<ProjectTask> _tasks = [];

        /// <summary>
        /// Идентификатор проекта
        /// </summary>
        public ProjectId Id { get; }

        /// <summary>
        /// Жизненный цикл проекта
        /// </summary>
        public ProjectLifeTime LifeTime { get; }

        /// <summary> 
        /// Описание проекта
        /// </summary>
        public ProjectDescription Description { get; }

        /// <summary> 
        /// Название проекта
        /// </summary>
        public ProjectName Name { get; }

        /// <summary>
        /// Задачи проекта
        /// </summary>
        public IReadOnlyList<ProjectTask> Tasks => _tasks;

        // для EF Core
        protected Project()
        {
            
        }
        public Project(
            ProjectId id,
            ProjectLifeTime lifeTime,
            ProjectDescription description,
            ProjectName name,
            IEnumerable<ProjectTask> tasks
            )
        {
            _tasks = tasks.ToList();
            Id = id;
            LifeTime = lifeTime;
            Description = description;
            Name = name;
        }
        // -----------------------------
        // Методы для управления задачами
        // -----------------------------

        public void AddTask(string description, int membersLimit)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Task description cannot be empty", nameof(description));

            if (membersLimit <= 0)
                throw new ArgumentException("Members limit must be positive", nameof(membersLimit));

            var task = new ProjectTask(
                id: ProjectTaskId.Create(Guid.NewGuid()),
                limit: ProjectTaskMembersLimit.Create(membersLimit),
                statusInfo: ProjectTaskStatusInfo.Create("Created", "created"),
                information: ProjectTaskInfo.Create(description),
                project: this,
                taskMembers: Array.Empty<ProjectTaskMemberInfo>()
            );

            _tasks.Add(task);
        }

        public void AddMemberToTask(
             Guid taskId,
             string email,
             string login
            )
        {
            var task = _tasks.FirstOrDefault(t => t.Id.Value == taskId)
                ?? throw new KeyNotFoundException($"Task {taskId} not found");

            var member = ProjectTaskMemberInfo.Create(
                ProjectMemberId.Create(Guid.NewGuid()),
                email,
                login
            );

            task.AddMember(member);
        }

        public void RemoveMemberFromTask(Guid taskId, ProjectMemberId memberId)
        {
            var task = _tasks.FirstOrDefault(t => t.Id.Value == taskId)
                ?? throw new KeyNotFoundException($"Task {taskId} not found");

            task.RemoveMember(memberId);
        }

        public void ChangeTaskInfo(Guid taskId, string description)
        {
            var task = _tasks.FirstOrDefault(t => t.Id.Value == taskId)
                ?? throw new KeyNotFoundException($"Task {taskId} not found");

            var info = ProjectTaskInfo.Create(description);
            task.ChangeInformation(info);
        }
        public void RemoveTask(Guid taskId)
        {
            var task = _tasks.FirstOrDefault(t => t.Id.Value == taskId)
                ?? throw new KeyNotFoundException($"Task {taskId} not found");

            _tasks.Remove(task);
        }

    }
}
