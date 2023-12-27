namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using static Dal.DataSource;
//using System.Collections.Generic;

internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// creates an engineer and adds it to the engineer list.
    /// </summary>
    /// <param name="engineer"></param>
    /// <returns></returns>
    /// <exception cref="DalAlreadyExistsException"></exception>
    public int Create(Engineer engineer)
    {
        if (Read(engineer.Id) is not null)
            throw new DalAlreadyExistsException($"Engineer with ID={engineer.Id} already exists");
        Engineers.Add(engineer);
        return engineer.Id;
    }
    /// <summary>
    /// deletes engineer from list.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        Engineer? toRemove = Read(id);
        if (toRemove is null)
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");
        Engineers.Remove(toRemove);
    }
    /// <summary>
    /// finds engineer by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
        return Engineers.FirstOrDefault(engineer => engineer.Id == id) ?? null;
    }
    /// <summary>
    /// returns a engineer by some attribute.
    /// </summary>
    /// <param name="filter">The attribute on which to search</param>
    /// <returns></returns>
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return Engineers.Where(filter).FirstOrDefault() ?? null;
    }
    /// <summary>
    /// returns all engineer from list.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Engineer>? ReadAll(Func<Engineer, bool>? filter = null)
    {
        return filter == null ? Engineers.Select(item => item) : Engineers.Where(filter);
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