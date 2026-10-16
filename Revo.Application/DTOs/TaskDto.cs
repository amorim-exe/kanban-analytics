using Revo.Domain.Entities;
using TaskStatus = Revo.Domain.Entities.TaskStatus;
using TaskPriority = Revo.Domain.Entities.TaskPriority;

namespace Revo.Application.DTOs;

public class TaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public int Order { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? AssignedToId { get; set; }
    public int EstimatedHours { get; set; }
    public int ActualHours { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int CommentCount { get; set; }
    public int AttachmentCount { get; set; }
}

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime? DueDate { get; set; }
    public int EstimatedHours { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? AssignedToId { get; set; }
}

public class UpdateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public int EstimatedHours { get; set; }
}

public class MoveTaskDto
{
    public TaskStatus NewStatus { get; set; }
    public int NewOrder { get; set; }
}

public class LogHoursDto
{
    public int Hours { get; set; }
}
