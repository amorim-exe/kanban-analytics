namespace Revo.Application.DTOs;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = "#3b82f6";
    public bool IsArchived { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int TaskCount { get; set; }
    public int CompletedTaskCount { get; set; }
}

public class CreateProjectDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = "#3b82f6";
}

public class UpdateProjectDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = "#3b82f6";
}
