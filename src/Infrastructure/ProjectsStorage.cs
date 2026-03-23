using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;
using YourProject.Domain.Interfaces;

namespace Infrastructure;

/// <summary>
/// In-memory хранилище проектов на основе статического словаря.
/// Реализует интерфейс IProjectRepository для лабораторной работы.
/// </summary>
public class ProjectsStorage : IProjectRepository
{
    /// <summary>
    /// Статический словарь: ключ — ProjectId, значение — Project
    /// </summary>
    private static readonly Dictionary<ProjectId, Project> _store = new();

    /// <summary>
    /// Semaphore для потокобезопасного доступа
    /// </summary>
    private static readonly SemaphoreSlim _lock = new(1, 1);

    /// <summary>
    /// Статический конструктор: пустой (тестовые данные добавляются через API)
    /// </summary>
    static ProjectsStorage()
    {
        // Тестовые данные не добавляем из-за проблемы с ProjectLifeTime
        // Проекты можно будет создать через POST /api/projects
    }

    /// <summary>
    /// Возвращает список всех проектов
    /// </summary>
    public Task<List<Project>> GetAllAsync(CancellationToken ct = default) =>
        Task.FromResult(_store.Values.ToList());

    /// <summary>
    /// Возвращает проект по Guid
    /// </summary>
    public Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var projectId = new ProjectId(id);
        _store.TryGetValue(projectId, out var project);
        return Task.FromResult(project);
    }

    /// <summary>
    /// Добавляет новый проект в хранилище
    /// </summary>
    public async Task AddAsync(Project project, CancellationToken ct = default)
    {
        if (project is null)
            throw new ArgumentNullException(nameof(project));

        await _lock.WaitAsync(ct);
        try
        {
            _store[project.Id] = project;
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Обновляет проект
    /// </summary>
    public async Task<bool> UpdateAsync(Project project, CancellationToken ct = default)
    {
        if (project is null)
            throw new ArgumentNullException(nameof(project));

        await _lock.WaitAsync(ct);
        try
        {
            if (!_store.ContainsKey(project.Id))
                return false;

            _store[project.Id] = project;
            return true;
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>
    /// Удаляет проект по Guid
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        await _lock.WaitAsync(ct);
        try
        {
            var projectId = new ProjectId(id);
            return _store.Remove(projectId);
        }
        finally
        {
            _lock.Release();
        }
    }
}