using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class AccountSubmitVerifiedHandler : IAccountSubmitVerifiedHandler
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly ICustomerData customerData;

    public AccountSubmitVerifiedHandler(IGetProfileHandler getProfileHandler, ICustomerData customerData)
    {
        this.getProfileHandler = getProfileHandler;
        this.customerData = customerData;
    }

    public AppResult<AccountSubmitVerifiedResult> Execute(AccountSubmitVerifiedArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<AccountSubmitVerifiedResult>.CreateFailed(ex, "An error occured in AccountSubmitVerifiedHandler");
        }
    }

    public async Task<AppResult<AccountSubmitVerifiedResult>> ExecuteAsync(AccountSubmitVerifiedArgs args)
    {
        try
        {
            var profileResult = await getProfileHandler.ExecuteAsync(new GetProfileArgs {});
            if(!profileResult.Succeeded || profileResult.Result == null)
            {
                return AppResult<AccountSubmitVerifiedResult>.CreateFailed(new ApplicationException(profileResult.Message), profileResult.Message);
            }
            var profile = profileResult.Result;

            if(profile.IsVerified != 0)
            {
                return AppResult<AccountSubmitVerifiedResult>.CreateFailed(new ApplicationException("Already submitted verfication"), "Already submitted verfication");
            }

            var updateResult = await customerData.UpdateCustomer(new Framework.ApiCommand.ApiData.Customer.Request.UpdateCustomerArgs {
                IsVerified = 1,
                IsMaker = true,
                CustomerId = profile.Id
            });
            if(!updateResult.Succeeded || updateResult.Result == null || !updateResult.Result.IsSuccess)
            {
                return AppResult<AccountSubmitVerifiedResult>.CreateFailed(new ApplicationException(updateResult.Result?.ErrorInfo?.Message), updateResult.Message);
            }

            return AppResult<AccountSubmitVerifiedResult>.CreateSucceeded(new AccountSubmitVerifiedResult {}, "Successfullt submit account verification");

        }
        catch (Exception ex)
        {
            return AppResult<AccountSubmitVerifiedResult>.CreateFailed(ex, "An error occured in AccountSubmitVerifiedHandler");
        }
    }
}