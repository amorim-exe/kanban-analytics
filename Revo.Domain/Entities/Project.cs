using Revo.Domain.Common;

namespace Revo.Domain.Entities;

public class Project : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Color { get; private set; } = "#3b82f6";
    public bool IsArchived { get; private set; }
    public int Order { get; private set; }
    public virtual ICollection<TaskItem> Tasks { get; private set; } = new List<TaskItem>();

    private Project() { }

    public Project(string name, string description, string color, string createdBy)
    {
        Name = name;
        Description = description;
        Color = color;
        CreatedBy = createdBy;
        UpdatedBy = createdBy;
    }

    public void Update(string name, string description, string color, string updatedBy)
    {
        Name = name;
        Description = description;
        Color = color;
        UpdateAuditInfo(updatedBy);
    }

    public void Archive(string updatedBy)
    {
        IsArchived = true;
        UpdateAuditInfo(updatedBy);
    }

    public void Restore(string updatedBy)
    {
        IsArchived = false;
        UpdateAuditInfo(updatedBy);
    }

    public void SetOrder(int order, string updatedBy)
    {
        Order = order;
        UpdateAuditInfo(updatedBy);
    }
}
