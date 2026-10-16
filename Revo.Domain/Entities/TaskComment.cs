using Revo.Domain.Common;

namespace Revo.Domain.Entities;

public class TaskComment : BaseEntity
{
    public string Content { get; private set; } = string.Empty;
    public Guid TaskId { get; private set; }
    public Guid AuthorId { get; private set; }
    public string AuthorName { get; private set; } = string.Empty;
    public virtual TaskItem Task { get; private set; } = null!;

    private TaskComment() { }

    public TaskComment(string content, Guid taskId, Guid authorId, string authorName)
    {
        Content = content;
        TaskId = taskId;
        AuthorId = authorId;
        AuthorName = authorName;
    }

    public void Update(string content, string updatedBy)
    {
        Content = content;
        UpdateAuditInfo(updatedBy);
    }
}
