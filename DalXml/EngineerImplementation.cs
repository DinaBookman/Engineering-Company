namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Xml.Linq;

internal class EngineerImplementation : IEngineer
{
    const string s_engineers = "engineer";
    static Engineer? getEngineer(XElement e) =>
        e.ToIntNullable("Id") is null ? null : new Engineer()
        {
            Id = (int)e.Element("Id")!,
            Name = (string)e.Element("Name")!,
            Email= (string)e.Element("Email")!,
            Level = e.ToEnumNullable<EngineerExperience>("Level")!,
            Cost = (double)e.Element("Cost")!
             
        };
    static IEnumerable<XElement> createEngineerElement(Engineer engineer)
    {

        yield return new XElement("ID", engineer.Id);
        if (engineer.Name is not null)
            yield return new XElement("Name", engineer.Name);
        if (engineer.Email is not null)
            yield return new XElement("Email", engineer.Email);
        yield return new XElement("engineerLevel", engineer.Level);
        yield return new XElement("BirthDate", engineer.Cost);

    }
    public Engineer Read(int id) =>
        (Engineer)getEngineer(XMLTools.LoadListFromXMLElement(s_engineers)?.Elements()
        .FirstOrDefault(st => st.ToIntNullable("ID") == id)
        // fix to: throw new DalMissingIdException(id);
        ?? throw new Exception("missing id"))!;
     
        public  Engineer  Read(Func<Engineer, bool> filter)
    {
        (Engineer)getEngineer(XMLTools.LoadListFromXMLElement(s_engineers)?.Elements()
        .FirstOrDefault(st => st.ToIntNullable(@"filter")



       return Engineers.Where(filter).FirstOrDefault() ?? null;

    }
    //public IEnumerable< Engineer?> Read(Func<Engineer, bool>? filter) { }
       // ? XMLTools.LoadListFromXMLElement(s_students).Elements().Select(s => getStudent(s))
       // : XMLTools.LoadListFromXMLElement(s_students).Elements().Select(s => getStudent(s)).Where(filter);



    public IEnumerable<Engineer?>ReadAll(Func<Engineer?, bool>? filter = null) =>
        filter is null
        ? XMLTools.LoadListFromXMLElement(s_engineers).Elements().Select(s => getEngineer(s))
        : XMLTools.LoadListFromXMLElement(s_engineers).Elements().Select(s => getEngineer(s)).Where(filter);

    public int Create(Engineer engineer)
    {
        XElement  engineersRootElem = XMLTools.LoadListFromXMLElement(s_engineers);

        if (XMLTools.LoadListFromXMLElement(s_engineers)?.Elements()
            .FirstOrDefault(st => st.ToIntNullable("ID") == engineer.Id) is not null)
            // fix to: throw new DalMissingIdException(id);;
            throw new Exception("id already exist");

        engineersRootElem.Add(new XElement("Student", createEngineerElement(engineer)));
        XMLTools.SaveListToXMLElement(engineersRootElem, s_engineers);

        return engineer.Id; ;
    }
    public void Delete(int id)
    {
        XElement engineersRootElem = XMLTools.LoadListFromXMLElement(s_engineers);

        (engineersRootElem.Elements()
            // fix to: throw new DalMissingIdException(id);
            .FirstOrDefault(st => (int?)st.Element("ID") == id) ?? throw new Exception("missing id"))
            .Remove();

        XMLTools.SaveListToXMLElement(engineersRootElem, s_engineers);
    }
    public void Update(Engineer engineer)
    {
        Delete( engineer.Id);
        Create(engineer);
    }
}
/* /// <summary>
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
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
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
    }*/