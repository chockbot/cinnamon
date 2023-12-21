using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;
public class DeleteAddOnsHandler : IDeleteAddOnsHandler
{
    private readonly IActivityData activityData;

    public DeleteAddOnsHandler(IActivityData activityData)
    {
        this.activityData = activityData;
    }

    public AppResult<DeleteAddOnsResult> Execute(DeleteAddOnsArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<DeleteAddOnsResult>.CreateFailed(ex, "An error occurred in DeleteAddOnsHandler");
        }
    }

    public async Task<AppResult<DeleteAddOnsResult>> ExecuteAsync(DeleteAddOnsArgs args)
    {
        var removeAddOns = await activityData.DeleteAddOns(new Framework.ApiCommand.ApiData.AddOns.Request.DeleteAddOnsArgs
        {
            AddOnsId = args.AddOnIds
        });
        if (!removeAddOns.Succeeded || removeAddOns.Result == null)
        {
            return AppResult<DeleteAddOnsResult>.CreateFailed(new ApplicationException(removeAddOns.Message), removeAddOns.Message);
        }
        if (removeAddOns.Succeeded && !removeAddOns.Result.IsSuccess)
        {
            return AppResult<DeleteAddOnsResult>.CreateFailed(
                new ApplicationException(removeAddOns.Result.ErrorInfo?.Message), "An error occurred in DeleteAddOnsHandler");
        }

        return AppResult<DeleteAddOnsResult>.CreateSucceeded(new DeleteAddOnsResult
        {
            IsSuccess = removeAddOns.Result.IsSuccess
        }, "Successfully removed add-on");
    }
}
