using Microsoft.EntityFrameworkCore;
using Revo.Application.Interfaces;
using Revo.Domain.Entities;
using Revo.Infrastructure.Data;

namespace Revo.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly RevoDbContext _context;

    public ProjectRepository(RevoDbContext context)
    {
        _context = context;
    }

    public async Task<Project?> GetByIdAsync(Guid id)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .OrderBy(p => p.Order)
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetActiveAsync()
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .Where(p => !p.IsArchived)
            .OrderBy(p => p.Order)
            .ToListAsync();
    }

    public async Task<Project> CreateAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<Project> UpdateAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task DeleteAsync(Guid id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }
}
