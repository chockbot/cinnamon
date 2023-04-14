using System.Security.Claims;
using AngleSharp.Dom;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;
using Ganss.XSS;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class UpdateActivityGuidHandler : IUpdateActivityGuidHandler
{
    private readonly IActivityData activityData;

    public UpdateActivityGuidHandler(IActivityData activityData)
    {
        this.activityData = activityData;
    }

    public AppResult<UpdateActivityResult> Execute(UpdateActivityArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateActivityResult>.CreateFailed(ex, "An error occured in UpdateActivityGuidHandler");
        }
    }

    public async Task<AppResult<UpdateActivityResult>> ExecuteAsync(UpdateActivityArgs args)
    {
        try
        {
            var updatedActivity = await activityData.UpdateActivityGuid(new Framework.ApiCommand.ApiData.Activity.Request.UpdateActivity { });

            if (!updatedActivity.Succeeded || updatedActivity.Result == null)
            {
                return AppResult<UpdateActivityResult>.CreateFailed(new ApplicationException(updatedActivity.Message), updatedActivity.Message);
            }
            if (updatedActivity.Succeeded && !updatedActivity.Result.IsSuccess)
            {
                return AppResult<UpdateActivityResult>.CreateFailed(
                    new ApplicationException(updatedActivity.Result.ErrorInfo?.Message), "An error occured in UpdateActivityGuidHandler");
            }

            return AppResult<UpdateActivityResult>.CreateSucceeded(new UpdateActivityResult
            {
               
            }, "Successfully updated activity guid");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateActivityResult>.CreateFailed(ex, "An error occured in UpdateActivityGuidHandler");
        }
    }
}