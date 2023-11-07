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
        int id = DataSource.Config.NextTaskId;
        Task copy = task with { Id = id };
        DataSource.Tasks.Add(copy);
        return id;
    }
   /// <summary>
   /// deletes task from task list.
   /// </summary>
   /// <param name="id"></param>
   /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        Task? toRemove = Read(id);
        if (toRemove is null)
            throw new Exception($"Task with ID={id} does not exist");
        DataSource.Tasks.Remove(toRemove);
    }
    /// <summary>
    /// finds a task by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Task? Read(int id)
    {
        var getTaskById = DataSource.Tasks.Where(task => task.Id == id);
        return getTaskById;
    }
    /// <summary>
    /// returns all tasks in tasks list.
    /// </summary>
    /// <returns></returns>
    public List<Task> ReadAll()
    {
        var allTasks =
           from Task in DataSource.Tasks
           select Task;
        return allTasks;
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