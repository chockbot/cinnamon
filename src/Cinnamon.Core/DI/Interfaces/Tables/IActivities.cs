namespace Cinnamon.Core
{
    public interface IActivities : IBaseTable<ActivityModel>
    {
        Task<ActivityModel> GetActivityByIdAsync(int id);
        Task<IList<ActivityModel>> GetActivityByCreatedId(int id);
        Task<ActivityModel> SlowUpdateActivityAsync(ActivityModel activity);
        Task<IList<ActivityModel>> GetActivitiesByCategoriesAsync(int categoryId);
        Task<IList<ActivityModel>> GetActivitiesBySubCategoriesAsync(int[] subIds);
    }
}
