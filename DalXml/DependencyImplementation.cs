namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class DependencyImplementation : IDependency
{
    const string DEPENDENCYENTITY = "dependencies";
    /// <summary>
    /// creates a Dependency and adds it to Dependencies list.
    /// </summary>
    /// <param name="dependency"></param>
    /// <returns></returns>
    public int Create(DO.Dependency dependency)
    {
        int id = Config.NextDependencyId;
        DO.Dependency copy = dependency with { Id = id };

        //Bringing the list of Dependencies from the file.
        List<DO.Dependency> dependenciesList = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(DEPENDENCYENTITY);

        //Adding a new Dependency to the list.
        dependenciesList?.Add(copy);

        //Returning the list to the xml file.
        XMLTools.SaveListToXMLSerializer<DO.Dependency>(dependenciesList!, DEPENDENCYENTITY);

        return id;
    }

    /// <summary>
    /// deletes dependency from dependencies list.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        DO.Dependency? toRemove = Read(id);
        if (toRemove is null)
            throw new DalDoesNotExistException($"Dependency with ID={id} does not exist");

        //Bringing the list of Dependencies from the file.
        List<DO.Dependency> dependenciesList = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(DEPENDENCYENTITY);

        //Removing the Dependency from the list.
        dependenciesList!.Remove(toRemove);

        //Returning the list to the xml file.
        XMLTools.SaveListToXMLSerializer<DO.Dependency>(dependenciesList, DEPENDENCYENTITY);
    }

    /// <summary>
    /// finds a dependency by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public DO.Dependency? Read(int id)
    {
        //Bringing the list of dependencies from the file.
        List<DO.Dependency> dependenciesList = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(DEPENDENCYENTITY);

        //searching the dependency in the list.
        if (dependenciesList == null)
            return null;
            //throw new DalDoesNotExistException("There is no dependencies.");
        DO.Dependency? foundDependency = dependenciesList.FirstOrDefault(dependency => dependency!.Id == id) ?? null;
        if (foundDependency == null)
            return null;
            //throw new DalDoesNotExistException("The dependency didn't found.");

        return foundDependency;
    }
    /// <summary>
    /// returns a dependency by some attribute.
    /// </summary>
    /// <param name="filter">The attribute on which to search</param>
    /// <returns></returns>
    public DO.Dependency? Read(Func<DO.Dependency, bool> filter)
    {
        //Bringing the list of dependencies from the file.
        List<DO.Dependency> dependenciesList = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(DEPENDENCYENTITY);

        //searching the dependency in the list.
        if (dependenciesList == null)
            return null;
            //throw new DalDoesNotExistException("There is no dependencies.");
        DO.Dependency? foundDependency = dependenciesList.Where(filter!).FirstOrDefault() ?? null;
        if (foundDependency == null)
            return null;
            //throw new DalDoesNotExistException("The dependency didn't found.");

        return foundDependency;
    }

    /// <summary>
    /// returns all dependencies in dependencies list.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<DO.Dependency> ReadAll(Func<DO.Dependency, bool>? filter = null)
    {
        //Bringing the list of Dependencies from the file.
        List<DO.Dependency> dependenciesList = XMLTools.LoadListFromXMLSerializer<DO.Dependency>(DEPENDENCYENTITY);

        //checks if the list is empty.
        if (dependenciesList == null)
            throw new DalDoesNotExistException("There is no dependencies.");

        //returns all the list if there is no filter, and the items that the given func returns true for them, if there is a filter.
        return (filter == null ? dependenciesList.Select(item => item) : dependenciesList.Where(filter!))!;
    }

    public void Update(DO.Dependency dependency)
    {
        Delete(dependency.Id);
        Create(dependency);
    }
}