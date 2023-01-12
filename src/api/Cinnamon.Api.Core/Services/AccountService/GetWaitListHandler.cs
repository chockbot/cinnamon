using Cinnamon.Api.Core.Modules.DataAccess.Activity;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class GetWaitListHandler: IGetWaitListHandler
{
    private readonly IWaitListData waitListData;
	public GetWaitListHandler(IWaitListData waitListData)
	{
		this.waitListData = waitListData;	
	}

    public AppResult<GetWaitListResult> Execute(GetWaitListArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetWaitListResult>.CreateFailed(ex, "An error occured in SubmitWaitlistHandler");
        }
    }

    public async Task<AppResult<GetWaitListResult>> ExecuteAsync(GetWaitListArgs args)
    {
        try
        {
            var result = await waitListData.GetAllWaitlist(new Framework.ApiCommand.ApiData.Waitlist.Request.GetAllWaitlistArgs { });
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetWaitListResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetWaitListResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetWaitListHandler");
            }
            return AppResult<GetWaitListResult>.CreateSucceeded(new GetWaitListResult
            {
                WaitLists = result.Result.Result.Select(e => {
                    return new GetWaitListResult.WaitList
                    {
                       Email= e.Email,  
                       Guid= e.Guid,
                       Id= e.Id,
                       IsVerified= e.IsVerified,
                       Token = e.Token  
                    };
                })
            }, "Successfully get all wait list");
        }
        catch (Exception ex)
        {
            return AppResult<GetWaitListResult>.CreateFailed(ex, "An error occured in GetWaitListHandler");
        }
    }
}
