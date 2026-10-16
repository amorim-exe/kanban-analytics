using Revo.Domain.Common;

namespace Revo.Domain.Entities;

public class TaskAttachment : BaseEntity
{
    public string FileName { get; private set; } = string.Empty;
    public string FileUrl { get; private set; } = string.Empty;
    public string FileType { get; private set; } = string.Empty;
    public long FileSize { get; private set; }
    public Guid TaskId { get; private set; }
    public Guid UploadedById { get; private set; }
    public string UploadedByName { get; private set; } = string.Empty;
    public virtual TaskItem Task { get; private set; } = null!;

    private TaskAttachment() { }

    public TaskAttachment(string fileName, string fileUrl, string fileType, long fileSize, 
        Guid taskId, Guid uploadedById, string uploadedByName)
    {
        FileName = fileName;
        FileUrl = fileUrl;
        FileType = fileType;
        FileSize = fileSize;
        TaskId = taskId;
        UploadedById = uploadedById;
        UploadedByName = uploadedByName;
    }
}
