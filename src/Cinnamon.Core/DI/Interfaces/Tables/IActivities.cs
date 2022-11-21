namespace Cinnamon.Core
{
    public interface IActivities : IBaseTable<ActivityModel>
    {
        Task<ActivityModel> GetActivityByIdAsync(int id);
        Task<IList<ActivityModel>> GetActivityByCreatedId(int id);
        Task<ActivityModel> SlowUpdateActivityAsync(ActivityModel activity);
    }
}
