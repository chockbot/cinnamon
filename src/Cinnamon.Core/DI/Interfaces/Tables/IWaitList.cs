namespace Cinnamon.Core
{
    public interface IWaitList : IBaseTable<WaitListModel>
    {
        Task<WaitListModel> GetWaitListByGuid(string guid);
    }
}
