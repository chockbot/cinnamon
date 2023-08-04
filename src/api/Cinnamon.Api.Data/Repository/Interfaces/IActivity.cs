using System.Linq.Expressions;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IActivity : IGenericEntity<Activity>
{
    Task<AppResult<IEnumerable<Activity>>> FindActivitiesAsync(Expression<Func<Activity, bool>> expression, string searchValue,
        int? take = 100, int? skip = 0, IEnumerable<Expression<Func<Activity, object>>>? includes = null);

    Task<AppResult<IEnumerable<Activity>>> GetPopularActivities(Expression<Func<Activity, bool>> expression,
        int? take = 100, int? skip = 0, IEnumerable<Expression<Func<Activity, object>>>? includes = null);

    Task<AppResult<IEnumerable<Activity>>> GetRecommendedActivities(int primaryActivityId, int count);
}