using System.Text.RegularExpressions;
using BlApi;
namespace BlImplementation;
internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// auxiliary function, checks if the email address is valid.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    private static bool IsValidEmail(string email)
    {
        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);
        return regex.IsMatch(email);
    }
    /// <summary>
    /// Function for creating new Engineer.
    /// </summary>
    /// <param name="boEngineer"></param>
    /// <returns></returns>
    /// <exception cref="BO.FormatException"></exception>
    /// <exception cref="BO.BlAlreadyExistsException"></exception>
    public int Create(BO.Engineer boEngineer)
    {
        DO.Engineer doEngineer = new(boEngineer.Id, boEngineer.Name!, boEngineer.Email!, (DO.EngineerExperience?)boEngineer.Level, boEngineer.Cost);
        try
        {
            if (boEngineer.Id > 0 && boEngineer.Name != "" && IsValidEmail(boEngineer.Email!) && boEngineer.Cost > 0)
            {
                int idEngineer = _dal.Engineer.Create(doEngineer);
                return idEngineer;
            }
            else
                throw new BO.FormatException("Wrong input");
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists", ex);
        }
    }
    /// <summary>
    /// auxiliary function, checks if the current engineer belongs to any task.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private bool NotInTask(int id)
    {
        DO.Task? taskInEngineer = _dal.Task.ReadAll(task => task.EngineerId == id)!.FirstOrDefault();
        if (taskInEngineer == null)
            return true;
        return false;
    }
    /// <summary>
    /// A function for delete an engineer.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Delete(int id)
    {
        try
        {
            if (NotInTask(id))
                _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} already exists", ex);
        }
    }
    /// <summary>
    ///  auxiliary function, looking for a current assignment for the engineer.
    /// </summary>
    /// <param name="doEngineer"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private BO.TaskInEngineer? TaskInEngineer(int id)
    {
        BO.TaskInEngineer? task = (from DO.Task doTask in _dal.Task.ReadAll(task => task.EngineerId == id && task.StartDate < DateTime.Now && task.CompleteDate > DateTime.Now)!
                                                       select new BO.TaskInEngineer { Id = doTask.Id, Alias = doTask.Alias }).FirstOrDefault();
        return task;
    }
    /// <summary>
    /// A function for reading an engineer by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");

        return new BO.Engineer()
        {
            Id = id,
            Name = doEngineer!.Name,
            Email = doEngineer.Email,
            Level = (BO.EngineerExperience?)doEngineer.Level,
            Cost = doEngineer.Cost,
            Task = TaskInEngineer(id)
        };
    }
    /// <summary>
    /// A function for reading all engineers / read by condition.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public IEnumerable<BO.Engineer>? ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()!
                let engineer = Read(doEngineer.Id)
                where filter != null ? filter(engineer) : true
                select engineer);
    }
    /// <summary>
    /// A function for updating an engineer.
    /// </summary>
    /// <param name="boEngineer"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.Engineer boEngineer)
    {
        DO.Engineer doEngineer = new DO.Engineer(boEngineer.Id, boEngineer.Name!, boEngineer.Email!, (DO.EngineerExperience?)boEngineer.Level, boEngineer.Cost);
        if (boEngineer.Id > 0 && boEngineer.Name != "" && IsValidEmail(boEngineer.Email!) && boEngineer.Cost > 0) 
        try
        {
            DO.Engineer? toUpdate = _dal.Engineer.Read(boEngineer.Id);
            if (toUpdate is not null)
            {
                _dal.Engineer.Update(doEngineer);
            }
            else throw new ArgumentNullException(nameof(boEngineer));
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={boEngineer.Id} already exists", ex);
        }  
        else throw new ArgumentNullException(nameof(boEngineer));
    }
}