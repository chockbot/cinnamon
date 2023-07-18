using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class IsAccountBlockedHandler : IIsAccountBlockedHandler
{
    private readonly ICustomerData customerData;

    public IsAccountBlockedHandler(ICustomerData customerData)
    {
        this.customerData = customerData;
    }

    public AppResult<IsAccountBlockedResult> Execute(IsAccountBlockedArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<IsAccountBlockedResult>.CreateFailed(ex, "An error occured in IsAccountBlockedHandler");
        }
    }

    public async Task<AppResult<IsAccountBlockedResult>> ExecuteAsync(IsAccountBlockedArgs args)
    {
        try
        {
            var checkRes = await customerData.GetCustomerByEmail(args.Email);
            if(!checkRes.Succeeded || checkRes.Result == null || !checkRes.Result.IsSuccess)
            {
                return AppResult<IsAccountBlockedResult>.CreateFailed(new ApplicationException(checkRes.Result?.ErrorInfo?.Message), checkRes.Message);
            }

            return AppResult<IsAccountBlockedResult>.CreateSucceeded(new IsAccountBlockedResult { IsAccountBlocked = checkRes.Result.Result.IsAccountBan }, "Successfully check account");
        }
        catch (Exception ex)
        {
            return AppResult<IsAccountBlockedResult>.CreateFailed(ex, "An error occured in IsAccountBlockedHandler");
        }
    }
}