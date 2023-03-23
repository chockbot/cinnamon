using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;
public class GetMakerActivitiesHandler: IGetMakerActivitiesHandler
{
    private readonly IActivityData activityData;
    public GetMakerActivitiesHandler(IActivityData activityData)
    {
        this.activityData = activityData;
    }

    public AppResult<GetMakerActivitiesResult> Execute(GetMakerActivitiesArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetMakerActivitiesResult>.CreateFailed(ex, "An error occured in GetMakerActivitiesHandler");
        }
    }

    public async Task<AppResult<GetMakerActivitiesResult>> ExecuteAsync(GetMakerActivitiesArgs args)
    {
        try
        {
            var result = await activityData.GetAllActivities(new Framework.ApiCommand.ApiData.Activity.Request.GetAllActivities
            {
                CustomerId = args.CustomerId,
                IncludeDescription = args.IncludeActivityDescription,
                IsActive = args.IsActive,
                IncludeStudents = args.IncludeStudents,
            });

            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetMakerActivitiesResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetMakerActivitiesResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetOwnedActivitiesHandler");
            }

            return AppResult<GetMakerActivitiesResult>.CreateSucceeded(new GetMakerActivitiesResult
            {
                Activities = result.Result.Result.Select(a => {
                    return new GetMakerActivitiesResult.Activity
                    {
                        Id = a.Id,
                        Title = a.Title,
                        OngoingStudents = a.OngoingStudents,
                        CompletedStudents = a.CompletedStudents,
                    };
                })
            }, "Successfully get owned activities");
        }
        catch (Exception ex)
        {
            return AppResult<GetMakerActivitiesResult>.CreateFailed(ex, "An error occured in GetMakerActivitiesHandler");
        }
    }
}
