using Domain.ProjectContexts;
using Domain.ProjectContexts.Entities;
using Microsoft.EntityFrameworkCore;
using YourProject.Domain.Interfaces;

public class EfProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public EfProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var projectId = ProjectId.Create(id);

        return await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId, ct);
    }

    public async Task<List<Project>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Projects.ToListAsync(ct);
    }

    public async Task AddAsync(Project project, CancellationToken ct)
    {
        await _context.Projects.AddAsync(project, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> UpdateAsync(Project project, CancellationToken ct)
    {
        // Проверяем, не отслеживается ли уже сущность с таким Id
        var tracked = _context.Projects
            .Local
            .FirstOrDefault(p => p.Id == project.Id);

        if (tracked != null)
        {
            // Если отслеживается — отвязываем старую, чтобы прикрепить новую
            _context.Entry(tracked).State = EntityState.Detached;
        }

        // Теперь безопасно прикрепляем новую иммутабельную сущность
        _context.Projects.Update(project);
        return await _context.SaveChangesAsync(ct) > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var project = await GetByIdAsync(id, ct);
        if (project == null)
            return false;

        _context.Projects.Remove(project);
        var result = await _context.SaveChangesAsync(ct);

        return result > 0;
    }
}