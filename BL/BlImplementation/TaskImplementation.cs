using BlApi;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
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

    public void Delete(int id)
    {
        DO.Dependency? previousTask = _dal.Dependency.ReadAll(dependency => dependency.DependsOnTask == id).FirstOrDefault();
        if (previousTask is not null)
        {
            //throw new BO.BlAlreadyExistsException($"Task with ID={id} already exists", ex);
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
    private IEnumerable<BO.TaskInList>? DependenciesList(int id)
    {
        IEnumerable<DO.Dependency?> dependencies = _dal.Dependency.ReadAll(dependency => dependency.DependentTask == id);
        IEnumerable<int> IdList = dependencies.Select(dependent => dependent!.DependsOnTask);
        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll(task => IdList.Contains(task.Id));
        IEnumerable<BO.TaskInList>? dependenciesList = tasks.Select(task => new BO.TaskInList() { Id = task!.Id, Description = task.Description, Alias = task.Alias, Status = BO.Status.Unscheduled });
        return dependenciesList;
    }

    /// <summary>
    /// auxiliary function, findes the milestone of the current task.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private BO.MilestoneInTask Milestone(int id)
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
    private BO.EngineerInTask? EngineerInTask(DO.Task doTask, int id)
    {
        if (doTask.EngineerId != null)
        {
            string engineerName = _dal.Engineer.Read(id)!.Name;
            BO.EngineerInTask engineer = new() { Id = (int)doTask.EngineerId, Name = engineerName };
            return engineer;
        }
        return null;
    }

    /// <summary>
    /// auxiliary function, determines task's status.
    /// </summary>
    /// <param name="doTask"></param>
    /// <returns></returns>
    public static BO.Status Status(DO.Task doTask)
    {
        if (doTask.StartDate is null)
            return BO.Status.Unscheduled;
        else if (DateTime.Now > doTask.StartDate && DateTime.Now < doTask.DeadlineDate)
            return BO.Status.OnTrack;
        else if (DateTime.Now > doTask.DeadlineDate && (DateTime.Now < doTask.CompleteDate || doTask.CompleteDate is null))
            return BO.Status.InJeopardy;
        return BO.Status.Scheduled;
    }
    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id) ?? throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        return new BO.Task()
        {
            Id = doTask.Id,
            Description = doTask.Description,
            Alias = doTask.Alias,
            Status = Status(doTask),
            DependenciesList = DependenciesList(id),
            CreatedAtDate = doTask.CreatedAtDate,
            StartDate = doTask.StartDate,
            ScheduledStartDate = doTask.ScheduledDate,
            DeadlineDate = doTask.DeadlineDate,
            ForecastAtDate = doTask.ForecastAtDate,
            CompletedAtDate = doTask.CompleteDate,
            Milestone = Milestone(id),
            Engineer = EngineerInTask(doTask, id),
            ComplexityLevel = (BO.EngineerExperience)doTask.CopmlexityLevel,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
        };
    }
    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        return (from DO.Task doTask in _dal.Task.ReadAll()
                let task = Read(doTask.Id)
                where filter != null ? filter(task) : true
                select task);
    }

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
        catch (Exception ex) { }
    }
}
