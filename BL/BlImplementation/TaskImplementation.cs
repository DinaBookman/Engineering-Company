using BlApi;
using System.Collections.Generic;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Task boTask)
    {
        if (boTask.Id <= 0) throw new ArgumentNullException(nameof(boTask));
        if (boTask.Description == "") throw new ArgumentNullException(nameof(boTask));

        //var Dependencies = from task in boTask.DependenciesList
        //                       select new { Dependencies = newDependency with {DependentTask = boTask.Id }};

        var itemList = boTask.DependenciesList!.Select(task => new DO.Dependency() { Id=0000, DependentTask = boTask.Id, DependsOnTask = task.Id });
        _= itemList.Select(dependency => _dal.Dependency.Create(dependency));
        
        DO.Task doTask = new DO.Task(boTask.Id, boTask.Description!, boTask.Alias!, boTask.CreatedAtDate, null, null,
                                     null, null, null, (DO.EngineerExperience)boTask.ComplexityLevel, false, boTask.Remarks);
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
        // בדיקה שהמשימה לא קודמת למשימות אחרות
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={id} already exists", ex);
        }
    }

    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        //finds a list of tasks that this task depends on.
        IEnumerable<DO.Dependency?> dependencies = _dal.Dependency.ReadAll(dependency=> dependency.DependentTask == id);
        IEnumerable<int> IdList = dependencies.Select(dependent => dependent!.DependsOnTask) ;
        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll(task => IdList.Contains(task.Id));
        IEnumerable<BO.TaskInList> dependenciesList = tasks.Select(task => new BO.TaskInList() { Id = task!.Id, Description = task.Description, Alias = task.Alias, Status = BO.Status.Unscheduled });
        //findes the milestone of the current task.

        IEnumerable<DO.Task> MileStones = _dal.Task.ReadAll(task => task.IsMilestone==true)!;
        IEnumerable<int> MileStoneIds = MileStones.Select(m => m!.Id);
        DO.Dependency milstone = _dal.Dependency.Read(dependency => dependency.DependsOnTask == id && MileStoneIds.Contains(dependency.DependentTask))!;
        string? MAlias = MileStones.Where(m => m!.Id == milstone.DependentTask).FirstOrDefault()!.Alias;
        BO.MilestoneInTask Milestone = new() {Id= milstone.DependentTask, Alias= MAlias };

        //findes the engineer that is incharge of this Task.
        BO.EngineerInTask? engineerIntask;
        if (doTask.EngineerId != null)
        {
            string engineerName = _dal.Engineer.Read(id)!.Name;
            engineerIntask = new() { Id = (int)doTask.EngineerId, Name = engineerName };
        }
        else
            engineerIntask = null;
        
       
        return new BO.Task()
        {
            Id = doTask.Id,
            Description = doTask.Description,
            Alias = doTask.Alias,
            Status = BO.Status.Unscheduled,//צריך לשאול לפי מה מחליטים מה הסטטוס!
            DependenciesList = dependenciesList,
            CreatedAtDate = (DateTime.Now),
            StartDate = doTask.StartDate,
            ScheduledStartDate = doTask.ScheduledDate,
            DeadlineDate = doTask.DeadlineDate,
            ForecastAtDate = doTask.ForecastAtDate,
            Milestone= Milestone,
            Engineer = engineerIntask,
            ComplexityLevel = (BO.EngineerExperience)doTask.CopmlexityLevel,
            Remarks = doTask.Remarks,            
        };
    }

    public IEnumerable<BO.TaskInList?> ReadAll()
    {
        return (from DO.Task doTask in _dal.Task.ReadAll()
                select new BO.TaskInList
                {
                    Id = doTask.Id,
                    Description = doTask.Description,
                    Alias = doTask.Alias,
                });
    }
    
    public void Update(BO.Task boTask)
    {
        if (boTask.Id <= 0) throw new ArgumentNullException(nameof(boTask));
        if (boTask.Description == "") throw new ArgumentNullException(nameof(boTask));
        try
        {
            int id = Create(boTask);
            DO.Task? toUpdate = _dal.Task.Read(id);
            if (toUpdate!=null)
            {
                _dal.Task.Update(toUpdate!);
            }
            else throw new ArgumentNullException(nameof(boTask));
        }
        catch (Exception ex) { }
    }
}
