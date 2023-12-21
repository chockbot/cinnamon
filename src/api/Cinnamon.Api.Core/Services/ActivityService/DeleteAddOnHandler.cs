using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class DeleteAddOnHandler : IDeleteAddOnHandler
{
    private readonly IActivityData activityData;
    public DeleteAddOnHandler(IActivityData activityData)
    {
        this.activityData = activityData;
    }

    public AppResult<DeleteAddOnResult> Execute(DeleteAddOnArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<DeleteAddOnResult>.CreateFailed(ex, "An error occurred in DeleteAddOnHandler");
        }
    }

    public async Task<AppResult<DeleteAddOnResult>> ExecuteAsync(DeleteAddOnArgs args)
    {
        var removeAddOn = await activityData.DeleteAddOn(new Framework.ApiCommand.ApiData.AddOns.Request.DeleteAddOnArgs
        {
            AddOnId = args.AddOnId
        });
        if (!removeAddOn.Succeeded || removeAddOn.Result == null)
        {
            return AppResult<DeleteAddOnResult>.CreateFailed(new ApplicationException(removeAddOn.Message), removeAddOn.Message);
        }
        if (removeAddOn.Succeeded && !removeAddOn.Result.IsSuccess)
        {
            return AppResult<DeleteAddOnResult>.CreateFailed(
                new ApplicationException(removeAddOn.Result.ErrorInfo?.Message), "An error occurred in DeleteAddOnsHandler");
        }

        return AppResult<DeleteAddOnResult>.CreateSucceeded(new DeleteAddOnResult
        {
            IsSuccess = removeAddOn.Result.IsSuccess
        }, "Successfully removed add-on");
    }
}
