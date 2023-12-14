using System.Linq.Expressions;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Activity;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IActivity : IGenericEntity<Activity>
{
    Task<AppResult<IEnumerable<Activity>>> FindActivitiesAsync(Expression<Func<Activity, bool>> expression, string searchValue,
        int? take = 100, int? skip = 0, IEnumerable<Expression<Func<Activity, object>>>? includes = null);

    Task<AppResult<IEnumerable<Activity>>> GetPopularActivities(Expression<Func<Activity, bool>> expression,
        int? take = 100, int? skip = 0, IEnumerable<Expression<Func<Activity, object>>>? includes = null);

    Task<AppResult<IEnumerable<Activity>>> GetRecommendedActivities(int primaryActivityId, int count);
    Task<AppResult<IEnumerable<PopularActivityDTO>>> PopularActivities(int? take, int? skip);
    Task<AppResult<Activity>> CreateOteActivity(Activity activity, ActivityDescription description, ActivityAddress address, OteSchedule oteSchedule);
    Task<AppResult<Activity>> UpdateOteActivity(Activity activity, ActivityDescription description, ActivityAddress address, OteSchedule oteSchedule);
    Task<AppResult<Activity>> FindOteByHandler(string handler, bool includeDescription = false, bool includeAddress = false,
        bool includeSchedule = false, bool includePricing = false, bool includeProvider = false, bool includeImages = false);
    Task<AppResult<IEnumerable<ActivityDTO>>> GetOTEByProvider(int Id);
    Task<AppResult<IEnumerable<OteOngoingDTO>>> CustomerOte(int customerId);
    Task<AppResult<IEnumerable<OteActivityPerDateDTO>>> OtePerDate(int? providerId);
}