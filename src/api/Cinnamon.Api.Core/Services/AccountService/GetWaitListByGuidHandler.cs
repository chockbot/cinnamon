using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class GetWaitListByGuidHandler : IGetWaitListByGuidHandler
{
    private readonly IWaitListData waitListData;
	public GetWaitListByGuidHandler(IWaitListData waitListData)
	{
		this.waitListData = waitListData;	
	}

    public AppResult<GetWaitListByGuidResult> Execute(GetWaitListByGuidArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetWaitListByGuidResult>.CreateFailed(ex, "An error occured in GetCustomerByEmailHandler");
        }
    }

    public async Task<AppResult<GetWaitListByGuidResult>> ExecuteAsync(GetWaitListByGuidArgs args)
    {
        try
        {
            var result = await waitListData.GetWaitListByGuid(args.Guid);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetWaitListByGuidResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetWaitListByGuidResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetGovernmentIdsHandler");
            }

            return AppResult<GetWaitListByGuidResult>.CreateSucceeded(new GetWaitListByGuidResult
            {
                Email= result.Result.Result.Email,
                IsVerified= result.Result.Result.IsVerified,
                Id= result.Result.Result.Id,    
                Guid = result.Result.Result.Guid,
                Token= result.Result.Result.Token
            }, "Successfully getting customer information");
        }
        catch (Exception ex)
        {
            return AppResult<GetWaitListByGuidResult>.CreateFailed(ex, "An error occured in GetGovernmentIdsHandler");
        }
    }
}
