namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using static Dal.DataSource;

//using System.Collections.Generic;

internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// creates a dependency.
    /// </summary>
    /// <param name="dependency"></param>
    /// <returns></returns>
    public int Create(Dependency dependency)
    {
        int id = Config.NextDependencyId;
        Dependency copy = dependency with { Id = id };
        Dependencies.Add(copy);
        return id;
    }
    /// <summary>
    /// deletes a dependency from list.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        Dependency? toRemove = Read(id);
        if (toRemove is null)
            throw new DalDoesNotExistException($"Dependency with ID={id} does not exist");
        Dependencies.Remove(toRemove);
    }
    /// <summary>
    /// returns a dependency by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {
        return Dependencies.FirstOrDefault(dependency => dependency.Id == id) ?? null;
    }
    /// <summary>
    /// returns a dependency by some attribute.
    /// </summary>
    /// <param name="filter">The attribute on which to search</param>
    /// <returns></returns>
    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return Dependencies.Where(filter).FirstOrDefault() ?? null;
    }
    /// <summary>
    /// returns all dependency from dependency list.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null)
    {
       return filter == null ? Dependencies.Select(item => item) : Dependencies.Where(filter);
    }
    /// <summary>
    /// update a dependency.
    /// </summary>
    /// <param name="dependency"></param>
    public void Update(Dependency dependency)
    {
        Delete(dependency.Id);
        Create(dependency);
    }
}