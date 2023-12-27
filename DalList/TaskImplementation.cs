namespace Dal;
using DalApi;
using DO;
using static Dal.DataSource;
//using System.Collections.Generic;
internal class TaskImplementation : ITask
{
     /// <summary>
     /// creates a task and adds it to tasks list.
     /// </summary>
     /// <param name="task"></param>
     /// <returns></returns>
    public int Create(Task task)
    {
        int id = Config.NextTaskId;
        Task copy = task with { Id = id };
        Tasks.Add(copy);
        return id;
    }
   /// <summary>
   /// deletes task from task list.
   /// </summary>
   /// <param name="id"></param>
   /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        Task? toRemove = Read(id);
        if (toRemove is null)
            throw new DalDoesNotExistException($"Task with ID={id} does not exist");
        Tasks.Remove(toRemove);
    }
    /// <summary>
    /// finds a task by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task? Read(int id)
    {
        return Tasks.FirstOrDefault(task => task.Id == id) ?? null;
    }
    /// <summary>
    /// returns a task by some attribute.
    /// </summary>
    /// <param name="filter">The attribute on which to search</param>
    /// <returns></returns>
    public Task? Read(Func<Task, bool> filter)
    {
        return Tasks.Where(filter).FirstOrDefault() ?? null;
    }
    /// <summary>
    /// returns all tasks in tasks list.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Task>? ReadAll(Func<Task, bool>? filter = null)
    {
        return filter == null ? Tasks.Select(item => item) : Tasks.Where(filter);
    }
    /// <summary>
    /// updates a task from task list.
    /// </summary>
    /// <param name="task"></param>
    public void Update(Task task)
    {
       Delete(task.Id);
       Create(task);
    }
}