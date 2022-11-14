namespace Cinnamon.Core
{
    public interface IActivities : IBaseTable<ActivityModel>
    {
        Task<ActivityModel> GetActivityByIdAsync(int id);
    }
}
