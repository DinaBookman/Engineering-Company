namespace DO;
/// <summary>
/// A task that should be performed by a programmer.
/// </summary>
/// <param name="Id">unique ID (created automatically)</param>
/// <param name="Description">describe the task</param>
/// <param name="Alias">The alias of the task</param>
/// <param name="CreatedAtDate">Task creation date</param>
/// <param name="StartDate">the real start date</param>
/// <param name="ScheduledDate">the original date on which it should be completed</param>
/// <param name="DeadlineDate">a revised scheduled completion date</param>
/// <param name="EngineerId"></param>
/// <param name="CopmlexityLevel">task: minimum experience for engineer to assign</param>
/// <param name="IsMilestone"></param>
///<param name="Remarks">free remarks from project meetings.</param>
public record Task
(
    int Id,
    string Description,
    string Alias,
    DateTime CreatedAtDate,
    DateTime? StartDate,
    DateTime? ScheduledDate,
    DateTime? DeadlineDate,
    DateTime? ForecastAtDate,
    int? EngineerId,
    EngineerExperience CopmlexityLevel,
    bool IsMilestone,
    string? Remarks
)
{
    public Task() : this(0, "", "", DateTime.Now, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue,
                         null, EngineerExperience.AdvancedBeginner, false, null) { } //empty ctor for stage 3
    DateTime? CompleteDate;//real completion date.
    string? Deliverables;//description of deliverables for MS completion.
};