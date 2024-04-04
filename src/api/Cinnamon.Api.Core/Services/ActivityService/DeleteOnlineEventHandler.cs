using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;
public class DeleteOnlineEventHandler : IDeleteOnlineEventHandler
{
    private readonly IActivityData activityData;
    public DeleteOnlineEventHandler(IActivityData activityData)
    {
        this.activityData = activityData;
    }

    public AppResult<DeleteOnlineEventResult> Execute(DeleteOnlineEventArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<DeleteOnlineEventResult>.CreateFailed(ex, "An error occurred in DeleteOnlineEventHandler");
        }
    }

    public async Task<AppResult<DeleteOnlineEventResult>> ExecuteAsync(DeleteOnlineEventArgs args)
    {
        var removeEvent = await activityData.DeleteOnlineEvent(new Framework.ApiCommand.ApiData.OnlineEvent.Request.DeleteOnlineEventArgs
        {
            Id = args.Id
        });
        if (!removeEvent.Succeeded || removeEvent.Result == null)
        {
            return AppResult<DeleteOnlineEventResult>.CreateFailed(new ApplicationException(removeEvent.Message), removeEvent.Message);
        }
        if (removeEvent.Succeeded && !removeEvent.Result.IsSuccess)
        {
            return AppResult<DeleteOnlineEventResult>.CreateFailed(
                new ApplicationException(removeEvent.Result.ErrorInfo?.Message), "An error occurred in DeleteOnlineEventHandler");
        }

        return AppResult<DeleteOnlineEventResult>.CreateSucceeded(new DeleteOnlineEventResult
        {
            IsSuccess = removeEvent.Result.IsSuccess
        }, "Successfully removed add-on");
    }
}
