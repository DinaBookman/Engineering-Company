namespace BO;
public class Task
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public DateTime CreatedAtDate { get; init; }
    public Status Status { get; set; }
    public IEnumerable<TaskInList>? DependenciesList { get; set; }
    public MilestoneInTask? Milestone { get; set; }
    public DateTime? ScheduledStartDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? ForecastAtDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompletedAtDate { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public EngineerInTask? Engineer { get; set; }
    public EngineerExperience ComplexityLevel { get; set; } 
}