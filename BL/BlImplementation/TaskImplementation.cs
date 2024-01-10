using BlApi;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Function for creating new Task.
    /// </summary>
    /// <param name="boTask"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public int Create(BO.Task boTask)
    {
        if (boTask.Id <= 0) throw new ArgumentNullException(nameof(boTask));
        if (boTask.Description == "") throw new ArgumentNullException(nameof(boTask));

        var itemList = boTask.DependenciesList!.Select(task => new DO.Dependency() { Id = 0000, DependentTask = boTask.Id, DependsOnTask = task.Id });
        _ = itemList.Select(dependency => _dal.Dependency.Create(dependency));

        DO.Task doTask = new DO.Task(boTask.Id, boTask.Description!, boTask.Alias!, boTask.CreatedAtDate, null, null,
                                     null, null, null, null, (DO.EngineerExperience)boTask.ComplexityLevel, false, boTask.Remarks, boTask.Deliverables);
        try
        {
            int idTask = _dal.Task.Create(doTask);
            return idTask;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
        }
    }

    /// <summary>
    /// A function that deletes a task.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public void Delete(int id)
    {
        DO.Dependency? previousTask = _dal.Dependency.ReadAll(dependency => dependency.DependsOnTask == id)!.FirstOrDefault();
        if (previousTask is not null)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={id} already exists");
        }
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={id} already exists", ex);
        }
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
        IEnumerable<BO.TaskInList>? dependenciesList = tasks.Select(task => new BO.TaskInList() { Id = task!.Id, Description = task.Description, Alias = task.Alias, Status =GetStatus(task) });
        return dependenciesList;
    }

    /// <summary>
    /// auxiliary function, finds the milestone of the current task.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private BO.MilestoneInTask GetMilestone(int id)
    {
        IEnumerable<DO.Task> milestones = _dal.Task.ReadAll(task => task.IsMilestone == true)!;
        IEnumerable<int> milestoneIds = milestones.Select(m => m!.Id);
        DO.Dependency milstone = _dal.Dependency.Read(dependency => dependency.DependsOnTask == id && milestoneIds.Contains(dependency.DependentTask))!;
        string? mAlias = milestones.Where(m => m!.Id == milstone.DependentTask).FirstOrDefault()!.Alias;
        BO.MilestoneInTask currentMilestone = new() { Id = milstone.DependentTask, Alias = mAlias };
        return currentMilestone;
    }

    /// <summary>
    /// auxiliary function, finds the engineer that is in charge of this Task.
    /// </summary>
    /// <param name="doTask"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private BO.EngineerInList? GetEngineerInTask(DO.Task doTask, int id)
    {
        if (doTask.EngineerId != null)
        {
            string engineerName = _dal.Engineer.Read(id)!.Name;
            BO.EngineerInList engineer = new() { Id = (int)doTask.EngineerId, Name = engineerName };
            return engineer;
        }
        return null;
    }

    /// <summary>
    /// auxiliary function, determines task's status.
    /// </summary>
    /// <param name="doTask"></param>
    /// <returns></returns>
    public static BO.Status GetStatus(DO.Task doTask)
    {
        if (doTask.StartDate is null)
            return BO.Status.Unscheduled;
        else if (DateTime.Now > doTask.StartDate && DateTime.Now < doTask.DeadlineDate)
            return BO.Status.OnTrack;
        else if (DateTime.Now > doTask.DeadlineDate && (DateTime.Now < doTask.CompleteDate || doTask.CompleteDate is null))
            return BO.Status.InJeopardy;
        return BO.Status.Scheduled;
    }
    /// <summary>
    /// A function for reading a task by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        return new BO.Task()
        {
            Id = doTask.Id,
            Description = doTask.Description,
            Alias = doTask.Alias,
            Status = GetStatus(doTask),
            DependenciesList = GetDependenciesList(id),
            CreatedAtDate = doTask.CreatedAtDate,
            StartDate = doTask.StartDate,
            ScheduledStartDate = doTask.ScheduledDate,
            DeadlineDate = doTask.DeadlineDate,
            ForecastAtDate = doTask.ForecastAtDate,
            CompletedAtDate = doTask.CompleteDate,
            Milestone = GetMilestone(id),
            Engineer = GetEngineerInTask(doTask, id),
            ComplexityLevel = (BO.EngineerExperience)doTask.CopmlexityLevel,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
        };
    }
    /// <summary>
    /// A function for reading all tasks / read by condition.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        return (from DO.Task doTask in _dal.Task.ReadAll()!
                let task = Read(doTask.Id)
                where filter != null ? filter(task) : true
                select task);
    }
    /// <summary>
    /// A function for updating a task.
    /// </summary>
    /// <param name="boTask"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.Task boTask)
    {
        if (boTask.Id <= 0) throw new ArgumentNullException(nameof(boTask));
        if (boTask.Description == "") throw new ArgumentNullException(nameof(boTask));
        try
        {
            int id = Create(boTask);
            DO.Task? toUpdate = _dal.Task.Read(id);
            if (toUpdate is not null)
            {
                _dal.Task.Update(toUpdate!);
            }
            else throw new ArgumentNullException(nameof(boTask));
        }
        catch (DO.DalDoesNotExistException ex) { throw new BO.BlDoesNotExistException($"Task with ID={boTask.Id} already exists", ex); }
    }
}
  