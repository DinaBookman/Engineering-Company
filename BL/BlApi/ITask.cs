namespace BlApi;

public interface ITask
{
    public IEnumerable<BO.Task?> ReadAll();
    public BO.Task? Read(int id);
    public int Create(BO.Task task);
    public void Update(BO.Task item);
    public void Delete(int id);
}