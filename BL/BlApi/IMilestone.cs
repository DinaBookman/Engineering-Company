using BO;

namespace BlApi;
internal interface IMilestone
{
    public IEnumerable<BO.MilestoneInTask> Schedule(IEnumerable<BO.TaskInList> dependencies);
    public BO.Milestone Update(BO.Milestone item);
    public BO.Milestone Read(int id);
}