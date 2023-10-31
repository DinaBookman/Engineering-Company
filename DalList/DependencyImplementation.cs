namespace Dal;
using DalApi;
using DO;
using static Dal.DataSource;

//using System.Collections.Generic;

public class DependencyImplementation : IDependency
{
    /// <summary>
    /// creates a dependency.
    /// </summary>
    /// <param name="dependency"></param>
    /// <returns></returns>
    public int Create(Dependency dependency)
    {
        int id = DataSource.Config.NextDependencyId;
        Dependency copy = dependency with { Id = id };
        DataSource.Dependencies.Add(copy);
        return id;
    }
    /// <summary>
    /// deletes a dependency from list.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        Dependency? toRemove = Read(id);
        if (toRemove is null)
            throw new Exception($"Dependency with ID={id} does not exist");
        DataSource.Dependencies.Remove(toRemove);
    }
    /// <summary>
    /// returns a dependency by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {
        Dependency? foundDependency = Dependencies.Find(item => item.Id == id);
        return foundDependency;
    }
    /// <summary>
    /// returns all dependency from dependency list.
    /// </summary>
    /// <returns></returns>
    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies);
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
