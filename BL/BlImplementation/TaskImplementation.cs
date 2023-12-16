using BlApi;
namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Task boTask)
    {
        if (boTask.Id <= 0) throw new ArgumentNullException(nameof(boTask));
        if (boTask.Description == "") throw new ArgumentNullException(nameof(boTask));

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
        return new BO.Task()
        {
            Id = doTask.Id,
            Description = doTask.Description,
            Alias = doTask.Alias,
            Status = BO.Status.Unscheduled,
            CreatedAtDate = (DateTime.Now),
            StartDate = doTask.StartDate,
            ScheduledStartDate = doTask.ScheduledDate,
            DeadlineDate = doTask.DeadlineDate,
            ForecastAtDate = doTask.ForecastAtDate,
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
