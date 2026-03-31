using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using YourProject.Domain.Interfaces;

namespace Infrastructure.Repositories;

public sealed class EfProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public EfProjectRepository(ApplicationDbContext context) => _context = context;


    public async Task<List<Project>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Projects
            .AsSplitQuery()
            .Include(p => p.Tasks) // Загружаем задачи агрегата
            .ThenInclude(t => t.TaskMembers) // И участников задач
            .ToListAsync(ct);
    }

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        // Создаём Value Object для сравнения
        var projectId = ProjectId.Create(id);

        return await _context.Projects
            .AsSplitQuery()
            .Include(p => p.Tasks)
            .ThenInclude(t => t.TaskMembers)
            .FirstOrDefaultAsync(p => p.Id == projectId, ct);
    }

    public async Task AddAsync(Project project, CancellationToken ct = default) =>
        await _context.Projects.AddAsync(project, ct);
    // без SaveChanges


    public Task DeleteAsync(Project project, CancellationToken ct = default)
    {
        _context.Projects.Remove(project);
        return Task.CompletedTask;
    }
    // без SaveChanges

    // Метод для пессимистической блокировки
    public async Task<Project?> GetByIdWithLockAsync(Guid id, CancellationToken ct = default)
    {
        var projectId = ProjectId.Create(id);
        // FOR UPDATE — блокировка строки на уровне БД
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"SELECT 1 FROM projects WHERE id = {id} FOR UPDATE",
            ct
        );

        return await GetByIdAsync(id, ct); // После блокировки загружаем агрегат
    }

}