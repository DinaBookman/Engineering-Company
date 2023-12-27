using System.Text.RegularExpressions;
using BlApi;
namespace BlImplementation;
internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private static bool IsValidEmail(string email)
    {
        string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])""|" + @"([-a-z0-9!#$%&'+/=?^_`{|}~]|(?<!\.)\.))(?<!\.)" + @"@[a-z0-9][\w\.-][a-z0-9]\.[a-z][a-z\.]*[a-z]$";
        var regex = new Regex(pattern, RegexOptions.IgnoreCase);
        return regex.IsMatch(email);
    }
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
                throw BO.FormatException("Wrong input");
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists", ex);
        }
    }

    private bool NotInTask(int id)
    {
        DO.Task? taskInEngineer = _dal.Task.ReadAll(task => task.EngineerId == id).FirstOrDefault();
        if (taskInEngineer == null)
            return true;
        return false;
    }

    public void Delete(int id)
    {
        try
        {
            if (NotInTask(id))
                _dal.Engineer.Delete(id);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={id} already exists", ex);
        }
    }


    private BO.TaskInEngineer? TaskInEngineer(DO.Engineer doEngineer, int id)
    {
        BO.TaskInEngineer? task = (BO.TaskInEngineer?)(from DO.Task doTask in _dal.Task.ReadAll(task => task.EngineerId == id && task.StartDate < DateTime.Now && task.CompleteDate > DateTime.Now)
                                                       select new BO.TaskInEngineer { Id = doTask.Id, Alias = doTask.Alias });
        return task;
    }
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
            Task = TaskInEngineer(doEngineer, id)
        };

    }

    public IEnumerable<BO.Engineer>? ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                let engineer = Read(doEngineer.Id)
                where filter != null ? filter(engineer) : true
                select engineer);
    }


    public void Update(BO.Engineer boEngineer)
    {
        DO.Engineer doEngineer = new DO.Engineer(boEngineer.Id, boEngineer.Name!, boEngineer.Email!, (DO.EngineerExperience?)boEngineer.Level, boEngineer.Cost);
        if (boEngineer.Id > 0 && boEngineer.Name != "" && IsValidEmail(boEngineer.Email!) && boEngineer.Cost > 0) throw new ArgumentNullException(nameof(boEngineer));
        try
        {
            int id = Create(boEngineer);
            DO.Engineer? toUpdate = _dal.Engineer.Read(id);
            if (toUpdate is not null)
            {
                _dal.Engineer.Update(toUpdate!);
            }
            else throw new ArgumentNullException(nameof(boEngineer));


        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists", ex);
        }
    }


}

 