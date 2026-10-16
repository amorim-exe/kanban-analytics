using Revo.Domain.Entities;
using TaskStatus = Revo.Domain.Entities.TaskStatus;

namespace Revo.Application.Interfaces;

public interface ITaskRepository
{
    System.Threading.Tasks.Task<TaskItem?> GetByIdAsync(Guid id);
    System.Threading.Tasks.Task<IEnumerable<TaskItem>> GetByProjectIdAsync(Guid projectId);
    System.Threading.Tasks.Task<IEnumerable<TaskItem>> GetByStatusAsync(TaskStatus status);
    System.Threading.Tasks.Task<IEnumerable<TaskItem>> GetOverdueTasksAsync();
    System.Threading.Tasks.Task<TaskItem> CreateAsync(TaskItem task);
    System.Threading.Tasks.Task<TaskItem> UpdateAsync(TaskItem task);
    System.Threading.Tasks.Task DeleteAsync(Guid id);
    System.Threading.Tasks.Task<IEnumerable<TaskItem>> GetAssignedToUserAsync(Guid userId);
}
