using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.DbSets;

public class ActivityEntity : GenericEntity<Activity>, IActivity
{
    private readonly ApplicationContext applicationContext;

    public ActivityEntity(ApplicationContext applicationContext)
        : base(applicationContext)
    {
        this.applicationContext = applicationContext;
    }

    public async Task<AppResult<IEnumerable<Activity>>> FindActivitiesAsync(Expression<Func<Activity, bool>> expression, string searchValue,
        int? take = 100, int? skip = 0, IEnumerable<Expression<Func<Activity, object>>>? includes = null)
    {
        try
        {
            int limitCount = take.HasValue ? take.Value : int.MaxValue;
            int skipCount = skip.HasValue ? skip.Value : 0;

            var query = applicationContext.Set<Activity>().OrderBy(a => a.Guid).Where(expression);

            if (!string.IsNullOrEmpty(searchValue))
            {
                string[] keywords = searchValue.ToLower().Trim().Split(' ');

                foreach (string keyword in keywords)
                {
                    query = query.Where(a => (string.IsNullOrEmpty(keyword) ? true : a.Title.ToLower().Trim().Contains(keyword) ||
                                                  a.Address.Address1.ToLower().Trim().Contains(keyword) ||
                                                  a.Address.Address2.ToLower().Trim().Contains(keyword) ||
                                                  a.Address.District.ToLower().Trim().Contains(keyword) ||
                                                  a.Address.CityName.ToLower().Trim().Contains(keyword) ||
                                                  a.Address.Subdivision.ToLower().Trim().Contains(keyword) ||
                                                  a.Address.RegionName.ToLower().Trim().Contains(keyword) ||
                                                  a.Address.BarangayName.ToLower().Trim().Contains(keyword) ||
                                                  a.Address.PostalCode.ToLower().Trim().Contains(keyword) ||
                                                  a.ExperienceType.Name.ToLower().Trim().Contains(keyword) ||
                                                  a.ExperienceCategory.Category.ToLower().Trim().Contains(keyword) ||
                                                  a.SubCategory.SubCatergory.ToLower().Trim().Contains(keyword) ||
                                                  a.Customer.FirstName.ToLower().Trim().Contains(keyword) ||
                                                  a.Customer.LastName.ToLower().Trim().Contains(keyword)
                                                  ));
                }
            }

            query = query.Skip(skipCount).Take(limitCount);

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var results = query.AsQueryable();
            return AppResult<IEnumerable<Activity>>.CreateSucceeded(results, "Successfully find entities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<Activity>>.CreateFailed(ex, "An error occured when finding entities");
        }
    }

    public async Task<AppResult<IEnumerable<Activity>>> GetPopularActivities(Expression<Func<Activity, bool>> expression, int? take = 100, int? skip = 0, IEnumerable<Expression<Func<Activity, object>>>? includes = null)
    {
        try
        {
            int limitCount = take.HasValue ? take.Value : int.MaxValue;
            int skipCount = skip.HasValue ? skip.Value : 0;

            var query = applicationContext.Set<Activity>().Where(expression).OrderByDescending(a => a.PurchaseOrderCount).Skip(skipCount).Take(limitCount);

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var results = await query.ToListAsync();
            return AppResult<IEnumerable<Activity>>.CreateSucceeded(results, "Successfully find entities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<Activity>>.CreateFailed(ex, "An error occured when finding entities");
        }
    }

    public async Task<AppResult<IEnumerable<Activity>>> GetRecommendedActivities(int primaryActivityId, int count)
    {
        try
        {
            var activity = await applicationContext.Activities.FindAsync(primaryActivityId);
            if (activity == null)
            {
                return AppResult<IEnumerable<Activity>>.CreateFailed(new ApplicationException("Invalid activity"), "Invalid activity");
            }

            // based first in sub category
            var activitiesSubs = await applicationContext.Activities.Where(a => a.Id != primaryActivityId && 
                                                                                a.SubCategoryId == activity.SubCategoryId && 
                                                                                a.Status == 1 && a.IsPublished == true)
                                    .Include(a => a.Images)
                                    .ToListAsync();

            if (activitiesSubs.Count >= count)
            {
                var randomActivities = GenerateRandomActivity(activitiesSubs, count);
                return AppResult<IEnumerable<Activity>>.CreateSucceeded(randomActivities, "Successfully get recommended activities");
            }

            // bas in experience categories
            var activitiesCats = await applicationContext.Activities.Where(a => a.Id != primaryActivityId && 
                                                                                a.ExperienceCategoryId == activity.ExperienceCategoryId &&
                                                                                a.Status == 1 && a.IsPublished == true )
                                    .Include(a => a.Images)
                                    .ToListAsync();

            var activities = GenerateRandomActivity(activitiesCats, count);

            return AppResult<IEnumerable<Activity>>.CreateSucceeded(activities, "Successfully get recommended activities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<Activity>>.CreateFailed(ex, "An error occured when getting recommended activities");
        }
    }

    private IEnumerable<Activity> GenerateRandomActivity(IEnumerable<Activity> activities, int count)
    {
        if(activities.Count() < count) return activities;
        IList<Activity> results = new List<Activity>();
        var activityList = activities.ToList();

        while(results.Count != count)
        {
            int random = (new Random()).Next(0, activityList.Count);
            var activity = activityList[random];
            results.Add(activity);
            activityList.RemoveAt(random);
        }

        return results;
    }
}