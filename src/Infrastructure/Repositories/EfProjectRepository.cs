using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using YourProject.Domain.Interfaces;

namespace Infrastructure.Repositories;

public sealed class EfProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public EfProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Projects
            .AsSplitQuery()
            .ToListAsync(ct);
    }

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        // Создаём Value Object для сравнения
        var projectId = ProjectId.Create(id);

        return await _context.Projects
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == projectId, ct);
    }

    public async Task AddAsync(Project project, CancellationToken ct = default)
    {
        await _context.Projects.AddAsync(project, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> UpdateAsync(Project project, CancellationToken ct = default)
    {
        _context.Projects.Update(project);
        var affected = await _context.SaveChangesAsync(ct);
        return affected > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var project = await GetByIdAsync(id, ct);
        if (project is not null)
        {
            _context.Projects.Remove(project);
            var affected = await _context.SaveChangesAsync(ct);
            return affected > 0;
        }
        return false;
    }
}