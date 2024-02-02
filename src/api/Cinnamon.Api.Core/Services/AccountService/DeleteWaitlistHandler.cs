using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;
public class DeleteWaitlistHandler : IDeleteWaitlistHandler
{
    private readonly IWaitListData waitListData;

    public DeleteWaitlistHandler(IWaitListData waitListData)
    {
        this.waitListData = waitListData;
    }

    public AppResult<DeleteWaitlistResult> Execute(DeleteWaitlistArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<DeleteWaitlistResult>.CreateFailed(ex, "An error occurred in DeleteWaitlistHandler");
        }
    }

    public async Task<AppResult<DeleteWaitlistResult>> ExecuteAsync(DeleteWaitlistArgs args)
    {
        var deleteWaitlist = await waitListData.DeleteWaitlist(new Framework.ApiCommand.ApiData.Waitlist.Request.DeleteWaitlistArgs
        {
            Email = args.Email
        });

        if (!deleteWaitlist.Succeeded || deleteWaitlist.Result == null)
        {
            return AppResult<DeleteWaitlistResult>.CreateFailed(new ApplicationException(deleteWaitlist.Message), deleteWaitlist.Message);
        }
        if (deleteWaitlist.Succeeded && !deleteWaitlist.Result.IsSuccess)
        {
            return AppResult<DeleteWaitlistResult>.CreateFailed(
                new ApplicationException(deleteWaitlist.Result.ErrorInfo?.Message), "An error occurred in DeleteAddOnsHandler");
        }

        return AppResult<DeleteWaitlistResult>.CreateSucceeded(new DeleteWaitlistResult
        {
            IsSuccess = deleteWaitlist.Result.IsSuccess
        }, "Successfully removed add-on");
    }
}
