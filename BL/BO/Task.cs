namespace BO;
/// <summary>
/// 
/// <param name="Id">Personal unique ID of engineer (as in national id card)</param>
/// <param name="Name">engineer's name</param>
/// <param name="Email">engineer's email address</param>
/// <param name="Level">engineer's level can either be a manager or engineer</param>
/// <param name="Cost">salary per hour</param>
/// </summary>
public class Task
{
    public int Id { get; init; }
    public string? Description { get; set; }
    public string? Alias { get; set; }
    public DateTime CreatedAtDate { get; init; }
    public Status Status { get; set; }
    public TaskInList DependenciesList { get; set; }
    public MilestoneInTask Milestone { get; set; }
    public DateTime ScheduledStartDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? ForecastAtDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompletedAtDate { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public EngineerInTask? Engineer { get; set; }
    public EngineerExperience ComplexityLevel { get; set; } 
}