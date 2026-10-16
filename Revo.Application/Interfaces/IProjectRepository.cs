using Revo.Domain.Entities;

namespace Revo.Application.Interfaces;

public interface IProjectRepository
{
    System.Threading.Tasks.Task<Project?> GetByIdAsync(Guid id);
    System.Threading.Tasks.Task<IEnumerable<Project>> GetAllAsync();
    System.Threading.Tasks.Task<IEnumerable<Project>> GetActiveAsync();
    System.Threading.Tasks.Task<Project> CreateAsync(Project project);
    System.Threading.Tasks.Task<Project> UpdateAsync(Project project);
    System.Threading.Tasks.Task DeleteAsync(Guid id);
}
