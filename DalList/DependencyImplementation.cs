namespace Dal;
using DalApi;
using DO;
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
        var getDependencyById = DataSource.Dependencies.Where(dependency => dependency.Id == id);
        return getDependencyById;
    }
    /// <summary>
    /// returns all dependency from dependency list.
    /// </summary>
    /// <returns></returns>
    public List<Dependency> ReadAll()
    {
        var allDependencies =
            from Dependency in DataSource.Dependencies
            select Dependency;
        return allDependencies;
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
