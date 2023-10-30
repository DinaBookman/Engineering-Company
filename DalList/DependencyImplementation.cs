namespace Dal;
using DalApi;
using DO;
using static Dal.DataSource;

//using System.Collections.Generic;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency dependency)
    {
        int id = DataSource.Config.NextDependencyId;
        Dependency copy = dependency with { Id = id };
        DataSource.Dependencies.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        Dependency? toRemove = Read(id);
        if (toRemove is null)
            throw new Exception($"Dependency with ID={id} does not exist");
        DataSource.Dependencies.Remove(toRemove);
    }

    public Dependency? Read(int id)
    {
        Dependency? foundDependency = Array.Find(Dependencies, item => item.Id == id));
        return foundDependency;
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies);
    }

    public void Update(Dependency dependency)
    {
        Delete(dependency.Id);
        Create(dependency);
    }
}
