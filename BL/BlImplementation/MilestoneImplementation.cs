namespace BlImplementation;
using BlApi;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public IEnumerable<BO.MilestoneInTask> Schedule(IEnumerable<BO.TaskInList> depend)
    {
        IEnumerable<DO.Dependency>? dependencies = _dal.Dependency.ReadAll()!;
        var dependenciesTable = from DO.Dependency dep in dependencies
                                group dep.DependsOnTask by dep.DependentTask into newGroup
                                orderby newGroup.Key
                                select newGroup;
        var filteredLists = dependenciesTable.Distinct();

        int runNum = 1;
        _dal.Task.Create(new DO.Task(0, null, "M" + "0".ToString(), DateTime.Now, null, null, null, null, null, null, 0, true, null, null));
        _ = filteredLists.Select(list => _dal.Task.Create(new DO.Task(0, null, "M" + runNum++.ToString(), DateTime.Now, null, null, null, null, null, null, 0, true, null, null)));

        runNum = 1;
        var x = from list in filteredLists
                select new { D = from item in list
                                 select new DO.Dependency(0, _dal.Task.Read(t=>t.Alias=="M"+ runNum++.ToString())!.Id, item) };
        return null!;
    }
     
    private void CreateMilestone(IGrouping<int,int> milestone)
    {
        _dal.Task.Create(_dal.Task.Read(milestone.Key)! with { Alias = "M", IsMilestone = true });
    }
    /// <summary>
    /// auxiliary function, finds a list of tasks that this task depends on.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private IEnumerable<BO.TaskInList>? GetDependenciesList(int id)
    {
        IEnumerable<DO.Dependency>? dependencies = _dal.Dependency.ReadAll(dependency => dependency.DependentTask == id)!;
        IEnumerable<int> IdList = dependencies.Select(dependent => dependent!.DependsOnTask);
        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll(task => IdList.Contains(task.Id))!;
        IEnumerable<BO.TaskInList>? dependenciesList = tasks.Select(task => new BO.TaskInList() { Id = task!.Id, Description = task.Description, Alias = task.Alias, Status = TaskImplementation.Status(task) });
        return dependenciesList;
    }
    /// <summary>
    /// auxiliary function, finds the completion percentage of the tasks related to the current milestone.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private double GetCompletionPercentage(int id)
    {
        IEnumerable<BO.TaskInList> tasksList = GetDependenciesList(id)!;
        IEnumerable<BO.TaskInList>? completedTasks = from BO.TaskInList task in tasksList
                                                     where Read(task.Id).CompletedAtDate < DateTime.Now
                                                     select task;
        return (double)(completedTasks.Count()/tasksList.Count())*100;
    }
    public BO.Milestone Read(int id)
    {
        DO.Task? doMilestone = _dal.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");
        return new BO.Milestone()
        {
            Id = doMilestone.Id,
            Description = doMilestone.Description,
            Alias = doMilestone.Alias,
            Status = TaskImplementation.GetStatus(doMilestone),
            CreatedAtDate = doMilestone.CreatedAtDate,
            StartDate = doMilestone.StartDate,
            DeadlineDate = doMilestone.DeadlineDate,
            ForecastAtDate = doMilestone.ForecastAtDate,
            CompletedAtDate = doMilestone.CompleteDate,
            CompletionPercentage = GetCompletionPercentage(id),
            Remarks = doMilestone.Remarks,
            DependenciesList = GetDependenciesList(id)
        };
    }

    public BO.Milestone Update(BO.Milestone milestone)
    {
        throw new NotImplementedException();
    }
}