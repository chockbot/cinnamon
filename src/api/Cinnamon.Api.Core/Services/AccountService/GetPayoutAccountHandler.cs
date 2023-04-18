using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class GetPayoutAccountHandler : IGetPayoutAccountHandler
{
    private readonly IPayoutAccountData payoutAccountData;
    private readonly IGetProfileHandler getProfileHandler;

    public GetPayoutAccountHandler(IPayoutAccountData payoutAccountData, IGetProfileHandler getProfileHandler)
    {
        this.payoutAccountData = payoutAccountData;
        this.getProfileHandler = getProfileHandler;
    }

    public AppResult<GetPayoutAccountResult> Execute(GetPayoutAccountArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetPayoutAccountResult>.CreateFailed(ex, "An error occured in GetPayoutAccountHandler");
        }
    }

    public async Task<AppResult<GetPayoutAccountResult>> ExecuteAsync(GetPayoutAccountArgs args)
    {
        try
        {
            // get customer profile
            var profileRes = await getProfileHandler.ExecuteAsync(new GetProfileArgs {});
            if(!profileRes.Succeeded || profileRes.Result == null)
            {
                return AppResult<GetPayoutAccountResult>.CreateFailed(new ApplicationException(profileRes.Message), profileRes.Message);
            }

            // can only request payout account if customer is maker
            if(!profileRes.Result.IsMaker)
            {
                return AppResult<GetPayoutAccountResult>.CreateFailed(new ApplicationException("Request not allowed"), "Request not allowed", "NOTMAKER");
            }

            var result = await payoutAccountData.GetPayoutAccountByCustomerId(profileRes.Result.Id);
            if(!result.Succeeded || result.Result == null || !result.Result.IsSuccess)
            {
                return AppResult<GetPayoutAccountResult>.CreateFailed(new ApplicationException("Account not set"), "Account not set", "NOTSET");
            }
            var account = result.Result.Result;

            return AppResult<GetPayoutAccountResult>.CreateSucceeded(new GetPayoutAccountResult {
                AccountHolder = account.AccountHolder,
                AccountNumber = account.AccountNumber,
                Payload = account.Payload,
                Id = account.Id,
                BankChannel = account.BankChannel
            }, "Successfully get payout account");
        }
        catch (Exception ex)
        {
            return AppResult<GetPayoutAccountResult>.CreateFailed(ex, "An error occured in GetPayoutAccountHandler");
        }
    }
}