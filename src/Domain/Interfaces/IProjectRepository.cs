using Domain.ProjectContexts;

namespace YourProject.Domain.Interfaces;

/// <summary>
/// Интерфейс репозитория для работы с проектами
/// </summary>
public interface IProjectRepository
{
    Task<List<Project>> GetAllAsync(CancellationToken ct = default);
    Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Project project, CancellationToken ct = default);
    Task<bool> UpdateAsync(Project project, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}