using Microsoft.EntityFrameworkCore;
using Revo.Application.Interfaces;
using Revo.Domain.Entities;
using Revo.Infrastructure.Data;
using TaskStatus = Revo.Domain.Entities.TaskStatus;

namespace Revo.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly RevoDbContext _context;

    public TaskRepository(RevoDbContext context)
    {
        _context = context;
    }

    public async System.Threading.Tasks.Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await _context.Tasks
            .Include(t => t.Project)
            .Include(t => t.Comments)
            .Include(t => t.Attachments)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskItem>> GetByProjectIdAsync(Guid projectId)
    {
        return await _context.Tasks
            .Where(t => t.ProjectId == projectId)
            .OrderBy(t => t.Order)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskItem>> GetByStatusAsync(TaskStatus status)
    {
        return await _context.Tasks
            .Where(t => t.Status == status)
            .OrderBy(t => t.Order)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskItem>> GetOverdueTasksAsync()
    {
        return await _context.Tasks
            .Where(t => t.DueDate.HasValue && t.DueDate < DateTime.UtcNow && t.Status != TaskStatus.Done)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }

    public async System.Threading.Tasks.Task<TaskItem> CreateAsync(TaskItem task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async System.Threading.Tasks.Task<TaskItem> UpdateAsync(TaskItem task)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async System.Threading.Tasks.Task DeleteAsync(Guid id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }

    public async System.Threading.Tasks.Task<IEnumerable<TaskItem>> GetAssignedToUserAsync(Guid userId)
    {
        return await _context.Tasks
            .Where(t => t.AssignedToId == userId)
            .OrderBy(t => t.Priority)
            .ToListAsync();
    }
}
