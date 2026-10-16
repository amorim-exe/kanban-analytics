using Revo.Domain.Common;

namespace Revo.Domain.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public TaskStatus Status { get; private set; } = TaskStatus.Todo;
    public TaskPriority Priority { get; private set; } = TaskPriority.Medium;
    public int Order { get; private set; }
    public DateTime? DueDate { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public Guid ProjectId { get; private set; }
    public Guid? AssignedToId { get; private set; }
    public int EstimatedHours { get; private set; }
    public int ActualHours { get; private set; }
    public virtual Project Project { get; private set; } = null!;
    public virtual ICollection<TaskComment> Comments { get; private set; } = new List<TaskComment>();
    public virtual ICollection<TaskAttachment> Attachments { get; private set; } = new List<TaskAttachment>();

    private TaskItem() { }

    public TaskItem(string title, string description, Guid projectId, string createdBy)
    {
        Title = title;
        Description = description;
        ProjectId = projectId;
        CreatedBy = createdBy;
        UpdatedBy = createdBy;
    }

    public void Update(string title, string description, TaskStatus status, TaskPriority priority, 
        DateTime? dueDate, int estimatedHours, string updatedBy)
    {
        Title = title;
        Description = description;
        Status = status;
        Priority = priority;
        DueDate = dueDate;
        EstimatedHours = estimatedHours;
        
        if (status == TaskStatus.Done && !CompletedAt.HasValue)
        {
            CompletedAt = DateTime.UtcNow;
        }
        else if (status != TaskStatus.Done)
        {
            CompletedAt = null;
        }
        
        UpdateAuditInfo(updatedBy);
    }

    public void MoveTo(TaskStatus newStatus, int newOrder, string updatedBy)
    {
        Status = newStatus;
        Order = newOrder;
        
        if (newStatus == TaskStatus.Done && !CompletedAt.HasValue)
        {
            CompletedAt = DateTime.UtcNow;
        }
        else if (newStatus != TaskStatus.Done)
        {
            CompletedAt = null;
        }
        
        UpdateAuditInfo(updatedBy);
    }

    public void AssignTo(Guid? userId, string updatedBy)
    {
        AssignedToId = userId;
        UpdateAuditInfo(updatedBy);
    }

    public void LogHours(int hours, string updatedBy)
    {
        ActualHours += hours;
        UpdateAuditInfo(updatedBy);
    }

    public void SetOrder(int order, string updatedBy)
    {
        Order = order;
        UpdateAuditInfo(updatedBy);
    }
}

public enum TaskStatus
{
    Todo,
    InProgress,
    Review,
    Done
}

public enum TaskPriority
{
    Low,
    Medium,
    High,
    Critical
}
