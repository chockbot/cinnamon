using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;
public class DeleteTicketHandler : IDeleteTicketHandler
{
    private readonly IActivityData activityData;
    public DeleteTicketHandler(IActivityData activityData)
    {
        this.activityData = activityData;
    }

    public AppResult<DeleteTicketResult> Execute(DeleteTicketArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<DeleteTicketResult>.CreateFailed(ex, "An error occurred in DeleteTicketHandler");
        }
    }

    public async Task<AppResult<DeleteTicketResult>> ExecuteAsync(DeleteTicketArgs args)
    {
        var removeTicket = await activityData.DeleteTicket(new Framework.ApiCommand.ApiData.Activity.Request.DeleteTicketArgs
        {
            Id = args.Id
        });
        if (!removeTicket.Succeeded || removeTicket.Result == null)
        {
            return AppResult<DeleteTicketResult>.CreateFailed(new ApplicationException(removeTicket.Message), removeTicket.Message);
        }
        if (removeTicket.Succeeded && !removeTicket.Result.IsSuccess)
        {
            return AppResult<DeleteTicketResult>.CreateFailed(
                new ApplicationException(removeTicket.Result.ErrorInfo?.Message), "An error occurred in DeleteTicketHandler");
        }

        return AppResult<DeleteTicketResult>.CreateSucceeded(new DeleteTicketResult
        {
            IsSuccess = removeTicket.Result.IsSuccess
        }, "Successfully removed ticket");
    }
}
