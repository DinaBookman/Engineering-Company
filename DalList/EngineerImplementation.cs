namespace Dal;
using DalApi;
using DO;
using static Dal.DataSource;
//using System.Collections.Generic;

public class EngineerImplementation : IEngineer
{  
     /// <summary>
     /// creates an engineer and adds it to the engineer list.
     /// </summary>
     /// <param name="engineer"></param>
     /// <returns></returns>
     /// <exception cref="Exception"></exception>
    public int Create(Engineer engineer)
    {
        if (Read(engineer.Id) is not null)
            throw new Exception($"Engineer with ID={engineer.Id} already exists");
        DataSource.Engineers.Add(engineer);
        return engineer.Id;
    }
    /// <summary>
    /// deletes engineer from list.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        Engineer? toRemove = Read(id);
        if (toRemove is null)
            throw new Exception($"Engineer with ID={id} does not exist");
        DataSource.Engineers.Remove(toRemove);
    }
    /// <summary>
    /// finds engineer by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
        Engineer? foundEngineer = Engineers.Find(item => item.Id == id);
        return foundEngineer;
    }
    /// <summary>
    /// returns all engineer from list.
    /// </summary>
    /// <returns></returns>
    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }
    /// <summary>
    /// update engineer.
    /// </summary>
    /// <param name="engineer"></param>
    public void Update(Engineer engineer)
    {
        Delete(engineer.Id);
        Create(engineer);
    }
}
