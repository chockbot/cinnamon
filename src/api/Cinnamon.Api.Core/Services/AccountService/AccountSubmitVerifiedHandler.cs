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
    private readonly IGetGovernmentIdsHandler governmentIdsHandler;

    public AccountSubmitVerifiedHandler(IGetProfileHandler getProfileHandler, ICustomerData customerData,
        IGetGovernmentIdsHandler governmentIdsHandler)
    {
        this.getProfileHandler = getProfileHandler;
        this.customerData = customerData;
        this.governmentIdsHandler = governmentIdsHandler;
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
            // get profile details
            var profileResult = getProfileHandler.ExecuteAsync(new GetProfileArgs {});

            // get government ids
            var governmentIds = governmentIdsHandler.ExecuteAsync(new GetGovernmentIdsArgs {});

            await Task.WhenAll(profileResult, governmentIds);

            if(!profileResult.Result.Succeeded || profileResult.Result.Result == null)
            {
                return AppResult<AccountSubmitVerifiedResult>.CreateFailed(new ApplicationException(profileResult.Result.Message), profileResult.Result.Message);
            }

            if(!governmentIds.Result.Succeeded || governmentIds.Result.Result == null)
            {
                return AppResult<AccountSubmitVerifiedResult>.CreateFailed(new ApplicationException(governmentIds.Result.Message), governmentIds.Result.Message);
            }

            var profile = profileResult.Result.Result;
            var ids = governmentIds.Result.Result;

            if(string.IsNullOrEmpty(ids.FrontImageSrc) || string.IsNullOrEmpty(ids.BackImageSrc))
            {
                return AppResult<AccountSubmitVerifiedResult>.CreateFailed(new ApplicationException("Invalid Request. Must provide valid ids."), "Invalid Request. Must provide valid ids.");
            }

            if(profile.IsVerified != 0)
            {
                return AppResult<AccountSubmitVerifiedResult>.CreateFailed(new ApplicationException("Already submitted verfication"), "Already submitted verfication");
            }

            var updateResult = await customerData.UpdateCustomer(new Framework.ApiCommand.ApiData.Customer.Request.UpdateCustomerArgs {
                IsVerified = 1,
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