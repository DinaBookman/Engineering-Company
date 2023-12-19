namespace DO;
/// <summary>
/// Dependencies between tasks, when the execution of one task depends on the completion of another task.
/// </summary>
/// <param name="Id">unique ID (created automatically)</param>
/// <param name="DependentTask">ID of Dependent task</param>
/// <param name="DependsOnTask">ID of the task that has to be completed before DependentTask</param>
public record Dependency
(
   int Id,
   int DependentTask,
   int DependsOnTask
)
{
    public Dependency() : this(0,0,0) {} //empty ctor for stage 3
};