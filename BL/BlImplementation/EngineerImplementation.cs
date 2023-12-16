using BlApi;
using BO;

namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public int Create(BO.Engineer boEngineer)
    {   if (boEngineer.Id > 0 && boEngineer.Name != "" && boEngineer.Email != null && boEngineer.Cost > 0)
          DO.Engineer doEngineer= new DO.Engineer(boEngineer.Id, boEngineer.Name, boEngineer.Email,boEngineer.Level, boEngineer.Cost);
        else
            throw BO.FormatException("Wrong input")
        try
        {
            int idEngineer = _dal.Engineer.Create(doEngineer);
            return idEngineer;
         }
         catch (DO.DalAlreadyExistsException ex)
        {
             throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists", ex);
         }
        
         
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public TaskInEngineer GetTaskForEngineer(int EngineerId)
    {
        return from BO.TaskInEngineer boEngineer 
                select new BO.TaskInEngineer
                {
                      Id = boEngineer.EngineerId
                };
    }

    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");

        return new BO.Engineer()
        {
            Id = id,
            Name = doEngineer.Name,
            Email= doEngineer.Email,
            Level = BO.doEngineer.Level,
            Cost = doEngineer.Cost,
            Task= TaskInEngineer
        };
    }

    public IEnumerable<BO.EngineerInTask> ReadAll()
    {
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                select new BO. EngineerInTask
                {
                    Id = doEngineer.Id,
                    Name = doEngineer.Name,
                });
    }

    public void Update(BO.Engineer boEngineer)
    {
        DO.Engineer doEngineer = new DO.Engineer(boEngineer.Id, boEngineer.Name, boEngineer.Email, boEngineer.Level, boEngineer.Cost);
        try
        {
            int idEngineer = _dal.Engineer.Create(doEngineer);
            return idEngineer;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists", ex);
        }
    }
}
/*
  public int Create(BO.Student boStudent)
{
    DO.Student doStudent = new DO.Student
        (boStudent.Id, boStudent.Name, boStudent.Alias, boStudent.IsActive, boStudent.BirthDate);
	try
	{
        int idStud = _dal.Student.Create(doStudent);
        return idStud;
	}
	catch (DO.DalAlreadyExistsException ex)
	{
        throw new BO.BlAlreadyExistsException($"Student with ID={boStudent.Id} already exists", ex);
	}
}
 
public BO.Student? Read(int id)
{
 
    DO.Student? doStudent = _dal.Student.Read(id);
	if (doStudent == null)
        throw new BO.BlDoesNotExistException($"Student with ID={id} does Not exist");
 
	return new BO.Student()
	{
        Id = id,
        Name = doStudent.Name,
        Alias = doStudent.Alias,
        IsActive = doStudent.IsActive,
        BirthDate = doStudent.BirthDate,
        RegistrationDate = doStudent.RegistrationDate,
        CurrentYear = (BO.Year)(DateTime.Now.Year - doStudent.RegistrationDate.Year)
	};
}
 
public IEnumerable<BO.StudentInList> ReadAll()
{
	return (from DO.Student doStudent in _dal.Student.ReadAll()
        select new BO.StudentInList
        {
            Id = doStudent.Id,
            Name = doStudent.Name,
            CurrentYear = (BO.Year)(DateTime.Now.Year - doStudent.RegistrationDate.Year)
        });
}
*/