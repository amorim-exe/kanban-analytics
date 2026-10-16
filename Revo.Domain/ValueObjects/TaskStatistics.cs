namespace Revo.Domain.ValueObjects;

public class TaskStatistics
{
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int InProgressTasks { get; set; }
    public int OverdueTasks { get; set; }
    public double CompletionRate => TotalTasks > 0 ? (double)CompletedTasks / TotalTasks * 100 : 0;
    public int TotalHours { get; set; }
    public int BillableHours { get; set; }
    public Dictionary<string, int> TasksByPriority { get; set; } = new();
    public Dictionary<string, int> TasksByStatus { get; set; } = new();
}

public class ProjectStatistics
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public TaskStatistics Statistics { get; set; } = new();
}
