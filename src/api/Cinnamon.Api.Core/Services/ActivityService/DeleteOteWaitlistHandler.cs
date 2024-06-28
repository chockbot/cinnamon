using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class DeleteOteWaitlistHandler : IDeleteOteWaitlistHandler
{
    private readonly IActivityData activityData;
    public DeleteOteWaitlistHandler(IActivityData activityData)
    {
        this.activityData = activityData;
    }

    public AppResult<DeleteOteWaitlistResult> Execute(DeleteOteWaitlistArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<DeleteOteWaitlistResult>.CreateFailed(ex, "An error occurred in DeleteOteWaitlistHandler");
        }
    }

    public async Task<AppResult<DeleteOteWaitlistResult>> ExecuteAsync(DeleteOteWaitlistArgs args)
    {
        try
        {
            var removeWaitlist = await activityData.DeleteOteWaitlist(new Framework.ApiCommand.ApiData.OteWaitlist.Request.DeleteOteWaitlistArgs
            {
                Id = args.Id
            });
            if (!removeWaitlist.Succeeded || removeWaitlist.Result == null)
            {
                return AppResult<DeleteOteWaitlistResult>.CreateFailed(new ApplicationException(removeWaitlist.Message), removeWaitlist.Message);
            }
            if (removeWaitlist.Succeeded && !removeWaitlist.Result.IsSuccess)
            {
                return AppResult<DeleteOteWaitlistResult>.CreateFailed(
                    new ApplicationException(removeWaitlist.Result.ErrorInfo?.Message), "An error occurred in DeleteOteWaitlistHandler");
            }
            return AppResult<DeleteOteWaitlistResult>.CreateSucceeded(new DeleteOteWaitlistResult
            {
                IsSuccess = removeWaitlist.Result.IsSuccess
            }, "Successfully removed ote waitlist");
        }
        catch (Exception ex)
        {
            return AppResult<DeleteOteWaitlistResult>.CreateFailed(ex, "An error occured in DeleteOteWaitlistHandler");
        }
    }
}
