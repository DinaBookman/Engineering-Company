namespace BlImplementation;
using BlApi;
using System.Reflection.Metadata.Ecma335;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public IEnumerable<BO.MilestoneInTask> Schedule(IEnumerable<BO.TaskInList> dependencies)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// auxiliary function, finds a list of tasks that this task depends on.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private IEnumerable<BO.TaskInList>? DependenciesList(int id)
    {
        IEnumerable<DO.Dependency?> dependencies = _dal.Dependency.ReadAll(dependency => dependency.DependentTask == id);
        IEnumerable<int> IdList = dependencies.Select(dependent => dependent!.DependsOnTask);
        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll(task => IdList.Contains(task.Id));
        IEnumerable<BO.TaskInList>? dependenciesList = tasks.Select(task => new BO.TaskInList() { Id = task!.Id, Description = task.Description, Alias = task.Alias, Status = BO.Status.Unscheduled });
        return dependenciesList;
    }
    /// <summary>
    /// auxiliary function, finds the completion percentage of the tasks related to the current milestone.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private double CompletionPercentage(int id)
    {
        IEnumerable<BO.TaskInList> tasksList = DependenciesList(id)!;
        IEnumerable<BO.TaskInList>? completedTasks = from BO.TaskInList task in tasksList
                                                     where Read(task.Id).CompletedAtDate < DateTime.Now
                                                     select task;
        return (completedTasks.Count()/tasksList.Count())*100;
    }
    public BO.Milestone Read(int id)
    {
        DO.Task? doMilestone = _dal.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");
        return new BO.Milestone()
        {
            Id = doMilestone.Id,
            Description = doMilestone.Description,
            Alias = doMilestone.Alias,
            Status = TaskImplementation.Status(doMilestone),
            CreatedAtDate = doMilestone.CreatedAtDate,
            StartDate = doMilestone.StartDate,
            DeadlineDate = doMilestone.DeadlineDate,
            ForecastAtDate = doMilestone.ForecastAtDate,
            CompletedAtDate = doMilestone.CompleteDate,
            CompletionPercentage = CompletionPercentage(id),
            Remarks = doMilestone.Remarks,
            DependenciesList = DependenciesList(id)
        };
    }

    public BO.Milestone Update(BO.Milestone milestone)
    {
        throw new NotImplementedException();
    }
}