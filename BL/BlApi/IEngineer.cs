namespace BlApi;

public interface IEngineer
{
    public int Create(BO.Engineer item);
    public BO.Engineer? Read(int id);
    public IEnumerable<BO.EngineerInTask> ReadAll();
    public void Update(BO.Engineer item);
    public void Delete(int id);


}
