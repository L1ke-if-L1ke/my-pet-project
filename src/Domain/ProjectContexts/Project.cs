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
    }
}
