using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class DisabledExpiredEventHandler : IDisabledExpiredEventHandler
{
    private readonly IActivityData activityData;

    public DisabledExpiredEventHandler(IActivityData activityData)
    {
        this.activityData = activityData;
    }

    public AppResult<DisabledExpiredEventResult> Execute(DisabledExpiredEventArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<DisabledExpiredEventResult>> ExecuteAsync(DisabledExpiredEventArgs args)
    {
        try
        {
            var activitiesRes = await activityData.ExpiredEvents();
            if(!activitiesRes.Succeeded || activitiesRes.Result is null || !activitiesRes.Result.IsSuccess)
            {
                return AppResult<DisabledExpiredEventResult>.CreateFailed(new ApplicationException(activitiesRes.Result?.ErrorInfo?.Message), activitiesRes.Message);
            }
            var activities = activitiesRes.Result.Result;

            if(activities.Count() > 0)
            {
                var disableRes = await activityData.ForceDisableActivities(new Framework.ApiCommand.ApiData.OteTicket.Request.ForceDisableActivitiesArgs {
                    Ids = activities.Select(a => a.Id).ToList()
                });
                if(!disableRes.Succeeded || disableRes.Result is null || !disableRes.Result.IsSuccess)
                {
                    return AppResult<DisabledExpiredEventResult>.CreateFailed(new ApplicationException(disableRes.Result?.ErrorInfo?.Message), disableRes.Message);
                }
            }

            return AppResult<DisabledExpiredEventResult>.CreateSucceeded(new DisabledExpiredEventResult {IsSuccess = true}, "Successfully disabled expired activities.");
        }
        catch (Exception ex)
        {
            return AppResult<DisabledExpiredEventResult>.CreateFailed(ex, "An error occured in DisabledExpiredEventHandler.");
        }
    }
}