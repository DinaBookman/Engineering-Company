using BlApi;
namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Engineer boEngineer)
    {
        DO.Engineer doEngineer = new DO.Engineer(boEngineer.Id, boEngineer.Name!, boEngineer.Email!, (DO.EngineerExperience?)boEngineer.Level, boEngineer.Cost); ;
        try
        {
            if (boEngineer.Id > 0 && boEngineer.Name != "" && boEngineer.Email != null && boEngineer.Cost > 0)
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


    public void Delete(int id)
    {
        try
        {
            //תבדוק שהמהנדס לא בביצוע של משימה
            _dal.Engineer.Delete(id);
            
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={id} already exists", ex);
        }
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
                Cost = doEngineer.Cost
            };
        }

        public IEnumerable<BO.EngineerInTask> ReadAll()
        {
            return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                    select new BO.EngineerInTask
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
                 
            }
            catch (DO.DalAlreadyExistsException ex)
            {
                throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists", ex);
            }
        }
    }

 