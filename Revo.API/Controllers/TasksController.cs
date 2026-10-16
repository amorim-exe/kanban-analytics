using Microsoft.AspNetCore.Mvc;
using Revo.Application.DTOs;
using Revo.Application.Interfaces;
using Revo.Domain.Entities;
using Revo.Infrastructure.Cache;
using TaskStatus = Revo.Domain.Entities.TaskStatus;

namespace Revo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICacheService _cacheService;

    public TasksController(
        ITaskRepository taskRepository,
        ICacheService cacheService)
    {
        _taskRepository = taskRepository;
        _cacheService = cacheService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks([FromQuery] Guid? projectId)
    {
        IEnumerable<Domain.Entities.TaskItem> tasks;

        if (projectId.HasValue)
        {
            tasks = await _taskRepository.GetByProjectIdAsync(projectId.Value);
        }
        else
        {
            tasks = await _taskRepository.GetByStatusAsync(TaskStatus.Todo);
        }

        var dtos = tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            Priority = t.Priority,
            Order = t.Order,
            DueDate = t.DueDate,
            CompletedAt = t.CompletedAt,
            ProjectId = t.ProjectId,
            AssignedToId = t.AssignedToId,
            EstimatedHours = t.EstimatedHours,
            ActualHours = t.ActualHours,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            CommentCount = t.Comments.Count,
            AttachmentCount = t.Attachments.Count
        });

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetTask(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        var dto = new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            Order = task.Order,
            DueDate = task.DueDate,
            CompletedAt = task.CompletedAt,
            ProjectId = task.ProjectId,
            AssignedToId = task.AssignedToId,
            EstimatedHours = task.EstimatedHours,
            ActualHours = task.ActualHours,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            CommentCount = task.Comments.Count,
            AttachmentCount = task.Attachments.Count
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask(CreateTaskDto dto)
    {
        var task = new Domain.Entities.TaskItem(
            dto.Title,
            dto.Description,
            dto.ProjectId,
            "system");

        task.Update(dto.Title, dto.Description, TaskStatus.Todo, dto.Priority, 
            dto.DueDate, dto.EstimatedHours, "system");
        
        if (dto.AssignedToId.HasValue)
        {
            task.AssignTo(dto.AssignedToId.Value, "system");
        }

        await _taskRepository.CreateAsync(task);
        await _cacheService.RemoveByPatternAsync("tasks:*");

        var response = new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            Order = task.Order,
            DueDate = task.DueDate,
            CompletedAt = task.CompletedAt,
            ProjectId = task.ProjectId,
            AssignedToId = task.AssignedToId,
            EstimatedHours = task.EstimatedHours,
            ActualHours = task.ActualHours,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            CommentCount = 0,
            AttachmentCount = 0
        };

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskDto>> UpdateTask(Guid id, UpdateTaskDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        task.Update(dto.Title, dto.Description, dto.Status, dto.Priority, 
            dto.DueDate, dto.EstimatedHours, "system");
        
        await _taskRepository.UpdateAsync(task);
        await _cacheService.RemoveByPatternAsync("tasks:*");

        var response = new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            Order = task.Order,
            DueDate = task.DueDate,
            CompletedAt = task.CompletedAt,
            ProjectId = task.ProjectId,
            AssignedToId = task.AssignedToId,
            EstimatedHours = task.EstimatedHours,
            ActualHours = task.ActualHours,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            CommentCount = task.Comments.Count,
            AttachmentCount = task.Attachments.Count
        };

        return Ok(response);
    }

    [HttpPost("{id}/move")]
    public async Task<ActionResult<TaskDto>> MoveTask(Guid id, MoveTaskDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        task.MoveTo(dto.NewStatus, dto.NewOrder, "system");
        await _taskRepository.UpdateAsync(task);
        await _cacheService.RemoveByPatternAsync("tasks:*");

        var response = new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            Order = task.Order,
            DueDate = task.DueDate,
            CompletedAt = task.CompletedAt,
            ProjectId = task.ProjectId,
            AssignedToId = task.AssignedToId,
            EstimatedHours = task.EstimatedHours,
            ActualHours = task.ActualHours,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            CommentCount = task.Comments.Count,
            AttachmentCount = task.Attachments.Count
        };

        return Ok(response);
    }

    [HttpPost("{id}/log-hours")]
    public async Task<ActionResult<TaskDto>> LogHours(Guid id, LogHoursDto dto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        task.LogHours(dto.Hours, "system");
        await _taskRepository.UpdateAsync(task);
        await _cacheService.RemoveByPatternAsync("tasks:*");

        var response = new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            Order = task.Order,
            DueDate = task.DueDate,
            CompletedAt = task.CompletedAt,
            ProjectId = task.ProjectId,
            AssignedToId = task.AssignedToId,
            EstimatedHours = task.EstimatedHours,
            ActualHours = task.ActualHours,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            CommentCount = task.Comments.Count,
            AttachmentCount = task.Attachments.Count
        };

        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTask(Guid id)
    {
        await _taskRepository.DeleteAsync(id);
        await _cacheService.RemoveByPatternAsync("tasks:*");

        return NoContent();
    }

    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetOverdueTasks()
    {
        var tasks = await _taskRepository.GetOverdueTasksAsync();
        var dtos = tasks.Select(t => new TaskDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            Status = t.Status,
            Priority = t.Priority,
            Order = t.Order,
            DueDate = t.DueDate,
            CompletedAt = t.CompletedAt,
            ProjectId = t.ProjectId,
            AssignedToId = t.AssignedToId,
            EstimatedHours = t.EstimatedHours,
            ActualHours = t.ActualHours,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            CommentCount = t.Comments.Count,
            AttachmentCount = t.Attachments.Count
        });

        return Ok(dtos);
    }
}
