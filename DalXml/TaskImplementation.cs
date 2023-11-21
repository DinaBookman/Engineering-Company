namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization; 
internal class TaskImplementation : ITask
{
    const string TASKENTITY = "tasks";
    /// <summary>
    /// creates a task and adds it to tasks list.
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    public int Create(DO.Task task)
    {
        int id = Config.NextTaskId;
        DO.Task copy = task with { Id = id };

        //Bringing the list of tasks from the file.
        List<DO.Task> tasksList = XMLTools.LoadListFromXMLSerializer<DO.Task>(TASKENTITY);

        //Adding a new task to the list.
        tasksList?.Add(copy);

        //Returning the list to the xml file.
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasksList!, TASKENTITY);
       
        return id;
    }

    /// <summary>
    /// deletes task from task list.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        DO.Task? toRemove = Read(id);
        if (toRemove is null)
            throw new DalDoesNotExistException($"Task with ID={id} does not exist");

        //Bringing the list of tasks from the file.
        List<DO.Task> tasksList = XMLTools.LoadListFromXMLSerializer<DO.Task>(TASKENTITY);

        //Removing the task from the list.
        tasksList!.Remove(toRemove);

        //Returning the list to the xml file.
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasksList, TASKENTITY);
    }

    /// <summary>
    /// finds a task by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public DO.Task? Read(int id)
    {
        //Bringing the list of tasks from the file.
        List<DO.Task> tasksList = XMLTools.LoadListFromXMLSerializer<DO.Task>(TASKENTITY);

        //searching the task in the list.
        if (tasksList == null)
            return null;
            //throw new DalDoesNotExistException("There is no tasks.");
        DO.Task? foundTask = tasksList.FirstOrDefault(task => task!.Id == id) ?? null;
        if(foundTask == null)
            return null;
            //throw new DalDoesNotExistException("The task didn't found.");

        return foundTask;  
    }
    /// <summary>
    /// returns a task by some attribute.
    /// </summary>
    /// <param name="filter">The attribute on which to search</param>
    /// <returns></returns>
    public DO.Task? Read(Func<DO.Task, bool> filter)
    {
        //Bringing the list of tasks from the file.
        List<DO.Task> tasksList = XMLTools.LoadListFromXMLSerializer<DO.Task>(TASKENTITY);

        //searching the task in the list.
        if (tasksList == null)
            return null;
           // throw new DalDoesNotExistException("There is no tasks.");
        DO.Task? foundTask = tasksList.Where(filter!).FirstOrDefault() ?? null;
        if (foundTask == null)
            return null;
            //throw new DalDoesNotExistException("The task didn't found.");

        return foundTask;
    }

    /// <summary>
    /// returns all tasks in tasks list.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<DO.Task> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        //Bringing the list of tasks from the file.
        List<DO.Task> tasksList = XMLTools.LoadListFromXMLSerializer<DO.Task>(TASKENTITY);

        //checks if the list is empty.
        if (tasksList == null)
            throw new DalDoesNotExistException("There is no tasks.");

        //returns all the list if there is no filter, and the items that the given func returns true for them, if there is a filter.
        return (filter == null ? tasksList.Select(item => item) : tasksList.Where(filter!))!;
    }

    public void Update(DO.Task task)
    {
        Delete(task.Id);
        Create(task);
    }
}