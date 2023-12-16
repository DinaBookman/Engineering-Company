namespace BlImplementation;
using BlApi;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public BO.Milestone Read(int id)
    {
        try
        {
            DO.Task? doTask = _dal.Task.Read(id);
        }
        catch { }
    }

    public IEnumerable<BO.MilestoneInTask> Schedule(IEnumerable<BO.TaskInList> dependencies)
    {
        throw new NotImplementedException();
    }

    public BO.Milestone Update(BO.Milestone item)
    {
        throw new NotImplementedException();
    }
}