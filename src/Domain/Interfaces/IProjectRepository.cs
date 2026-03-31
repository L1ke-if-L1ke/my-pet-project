using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;

namespace YourProject.Domain.Interfaces;

/// <summary>
/// Интерфейс репозитория для работы с проектами
/// </summary>
public interface IProjectRepository
{
    Task<List<Project>> GetAllAsync(CancellationToken ct = default);
    Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Project project, CancellationToken ct = default);
    Task DeleteAsync(Project project, CancellationToken ct = default); // передаём сущность, не Guid
    Task<Project?> GetByIdWithLockAsync(Guid id, CancellationToken ct);
}