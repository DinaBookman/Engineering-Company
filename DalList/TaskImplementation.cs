namespace Dal;
using DalApi;
using DO;
using static Dal.DataSource;

//using System.Collections.Generic;

public class TaskImplementation : ITask
{
    public int Create(Task task)
    {
        int id = DataSource.Config.NextTaskId;
        Task copy = task with { Id = id };
        DataSource.Tasks.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        Task? toRemove = Read(id);
        if (toRemove is null)
            throw new Exception($"Task with ID={id} does not exist");
        DataSource.Tasks.Remove(toRemove);
    }

    public Task? Read(int id)
    {
        Task? foundTask = Array.Find(Tasks, item => item.Id == id));
        return foundTask;
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task task)
    {
       Delete(task.Id);
       Create(task);
    }
}