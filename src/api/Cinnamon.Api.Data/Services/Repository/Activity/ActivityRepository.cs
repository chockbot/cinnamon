using Cinnamon.Api.Data.Services.Repository.Activity.DTO;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Activity;

public class ActivityRepository : IActivityRepository
{
    private readonly IDataStore dataStore;

    public ActivityRepository(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<AppResult<IEnumerable<ActivityDTO>>> GetAllAsync(bool? isActive, int? count, int? skip)
    {
        try
        {
            count = count.HasValue ? count.Value : 0;
            skip = skip.HasValue ? skip.Value : 0;

            var result = await dataStore.Activity.FindAsync(a => a.IsPublished == isActive, count.Value, skip.Value);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(result.Error.Exception, result.Message);
            }

            var activities = result.Result.Select(a =>
            {
                return new ActivityDTO
                {
                    Id = a.Id,
                    SubTitle = a.Subtitle,
                    Title = a.Title,
                    Description = a.Description,
                    Price = a.Price,
                    Remarks = a.Remarks,
                    IsPublished = a.IsPublished,
                };
            });

            return AppResult<IEnumerable<ActivityDTO>>.CreateSucceeded(activities, "Successfully get activities");
        }
        catch (Exception ex)
        {
            return AppResult<IEnumerable<ActivityDTO>>.CreateFailed(ex, "An error occured in getting activities");
        }
    }

    public async Task<AppResult<ActivityDTO>> GetByIdAsync(int id)
    {
        try
        {
            var result = await dataStore.Activity.GetByIdAsync(id);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<ActivityDTO>.CreateFailed(result.Error.Exception, result.Message);
            }

            var activityDao = new ActivityDTO
            {
                Id = result.Result.Id,
                SubTitle = result.Result.Subtitle,
                Title = result.Result.Title,
                Description = result.Result.Description,
                Price = result.Result.Price,
                Remarks = result.Result.Remarks,
                IsPublished = result.Result.IsPublished,
            };

            return AppResult<ActivityDTO>.CreateSucceeded(activityDao, "Successfully getting activity by id");
        }
        catch (Exception ex)
        {
            return AppResult<ActivityDTO>.CreateFailed(ex, "An error occured when getting activity by id");
        }
    }
}