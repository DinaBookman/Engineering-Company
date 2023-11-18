namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Reflection.Emit;
using System.Xml.Linq;

internal class EngineerImplementation : IEngineer
{
    const string s_engineers = "engineer";
    static Engineer? GetEngineer(XElement e) =>
        e.ToIntNullable("Id") is null ? null : new Engineer()
        {
            Id = (int)e.Element("Id")!,
            Name = (string)e.Element("Name")!,
            Email = (string)e.Element("Email")!,
            Level = (EngineerExperience)e.Element("Level")!,
            Cost = (double)e.Element("Cost")!
             
        };
    static IEnumerable<XElement> CreateEngineerElement(Engineer engineer)
    {

        yield return new XElement("ID", engineer.Id);
        if (engineer.Name is not null)
            yield return new XElement("Name", engineer.Name);
        if (engineer.Email is not null)
            yield return new XElement("Email", engineer.Email);
        yield return new XElement("engineerLevel", engineer.Level);
        yield return new XElement("Cost", engineer.Cost);

    }
    public Engineer Read(int id) =>
        (Engineer)GetEngineer(XMLTools.LoadListFromXMLElement(s_engineers)?.Elements()
        .FirstOrDefault(st => st.ToIntNullable("ID") == id)
        // fix to: throw new DalMissingIdException(id);
        ?? throw new Exception("missing id"))!;

  public Engineer Read(Func<Engineer, bool> filter) =>
 (Engineer)GetEngineer(XMLTools.LoadListFromXMLElement(s_engineers)?.Elements()
 .Where(filter!).FirstOrDefault())
// fix to: throw new DalMissingIdException(id);
?? throw new Exception("missing id")!;



    public IEnumerable<Engineer?>ReadAll(Func<Engineer, bool>? filter = null) =>
        filter is null
        ? XMLTools.LoadListFromXMLElement(s_engineers).Elements().Select(s => GetEngineer(s))
        : XMLTools.LoadListFromXMLElement(s_engineers).Elements().Select(s => GetEngineer(s)).Where(filter!);

    public int Create(Engineer engineer)
    {
        XElement  engineersRootElem = XMLTools.LoadListFromXMLElement(s_engineers);

        if (XMLTools.LoadListFromXMLElement(s_engineers)?.Elements()
            .FirstOrDefault(st => st.ToIntNullable("ID") == engineer.Id) is not null)
            // fix to: throw new DalMissingIdException(id);;
            throw new Exception("id already exist");

        engineersRootElem.Add(new XElement("Student", CreateEngineerElement(engineer)));
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
  