namespace Dal;
using DalApi;
using DO;
using static Dal.DataSource;
//using System.Collections.Generic;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer engineer)
    {
        if (Read(engineer.Id) is not null)
            throw new Exception($"Engineer with ID={engineer.Id} already exists");
        DataSource.Engineers.Add(engineer);
        return engineer.Id;
    }

    public void Delete(int id)
    {
        Engineer? toRemove = Read(id);
        if (toRemove is null)
            throw new Exception($"Engineer with ID={id} does not exist");
        DataSource.Engineers.Remove(toRemove);
    }

    public Engineer? Read(int id)
    {
        Engineer? foundEngineer = Engineers.Find(item => item.Id == id);
        return foundEngineer;
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer engineer)
    {
        Delete(engineer.Id);
        Create(engineer);
    }
}
