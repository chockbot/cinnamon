using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class CreateUpdatePayoutAccountHandler : ICreateUpdatePayoutAccountHandler
{
    private readonly IPayoutAccountData payoutAccountData;
    private readonly IGetProfileHandler getProfileHandler;

    public CreateUpdatePayoutAccountHandler(IPayoutAccountData payoutAccountData, IGetProfileHandler getProfileHandler)
    {
        this.payoutAccountData = payoutAccountData;
        this.getProfileHandler = getProfileHandler;
    }

    public AppResult<CreateUpdatePayoutAccountResult> Execute(CreateUpdatePayoutAccountArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CreateUpdatePayoutAccountResult>.CreateFailed(ex, "An error occured in CreateUpdatePayoutAccountHandler");
        }
    }

    public async Task<AppResult<CreateUpdatePayoutAccountResult>> ExecuteAsync(CreateUpdatePayoutAccountArgs args)
    {
        try
        {
            // get customer profile
            var profileRes = await getProfileHandler.ExecuteAsync(new GetProfileArgs {});
            if(!profileRes.Succeeded || profileRes.Result == null)
            {
                return AppResult<CreateUpdatePayoutAccountResult>.CreateFailed(new ApplicationException(profileRes.Message), profileRes.Message);
            }
            var account = profileRes.Result;

            // can only create payout account if maker
            if(!account.IsMaker)
            {
                return AppResult<CreateUpdatePayoutAccountResult>.CreateFailed(new ApplicationException("Invalid request"), "Invalid request");
            }

            if(string.IsNullOrEmpty(args.AccountHolder) || string.IsNullOrEmpty(args.AccountNumber))
            {
                return AppResult<CreateUpdatePayoutAccountResult>.CreateFailed(new ApplicationException("Invalid request"), "Invalid request");
            }

            var payoutRes = await payoutAccountData.GetPayoutAccountByCustomerId(account.Id);
            if(!payoutRes.Succeeded || payoutRes.Result == null)
            {
                return AppResult<CreateUpdatePayoutAccountResult>.CreateFailed(new ApplicationException(payoutRes.Result?.ErrorInfo?.Message), payoutRes.Message);
            }

            if(payoutRes.Succeeded && !payoutRes.Result.IsSuccess && payoutRes.Result.ErrorInfo?.Code != "NOTEXIST")
            {
                return AppResult<CreateUpdatePayoutAccountResult>.CreateFailed(new ApplicationException(payoutRes.Result?.ErrorInfo?.Message), payoutRes.Message);
            }

            // don't have yet existed payout account, then create the accout
            if(payoutRes.Succeeded && !payoutRes.Result.IsSuccess && payoutRes.Result.ErrorInfo?.Code == "NOTEXIST")
            {
                var created = await payoutAccountData.CreatePayoutAccount(new Framework.ApiCommand.ApiData.PayoutAccount.Request.CreatePayoutAccountArgs {
                    AccountHolder = args.AccountHolder,
                    AccountNumber = args.AccountNumber,
                    CustomerId = account.Id,
                    Payload = args.Payload
                });
                if(!created.Succeeded || created.Result == null || !created.Result.IsSuccess)
                {
                    return AppResult<CreateUpdatePayoutAccountResult>.CreateFailed(new ApplicationException(created.Result?.ErrorInfo?.Message), created.Message);
                }
                var newAcount = created.Result.Result;

                return AppResult<CreateUpdatePayoutAccountResult>.CreateSucceeded(new CreateUpdatePayoutAccountResult {
                    AccountHolder = newAcount.AccountHolder,
                    AccountNumber = newAcount.AccountNumber,
                    Id = newAcount.Id,
                    Payload = newAcount.Payload
                }, "Successfully created payout account");
            }

            // update the payout account
            var updated = await payoutAccountData.UpdatePayoutAccount(new Framework.ApiCommand.ApiData.PayoutAccount.Request.UpdatePayoutAccountArgs{
                AccountHolder = args.AccountHolder,
                AccountNumber = args.AccountNumber,
                Payload = args.Payload,
                Id = payoutRes.Result.Result.Id
            });
            if(!updated.Succeeded || updated.Result == null || !updated.Result.IsSuccess)
            {
                return AppResult<CreateUpdatePayoutAccountResult>.CreateFailed(new ApplicationException(updated.Result?.ErrorInfo?.Message), updated.Message);
            }
            var updatedAccount = updated.Result.Result;

            return AppResult<CreateUpdatePayoutAccountResult>.CreateSucceeded(new CreateUpdatePayoutAccountResult {
                AccountHolder = updatedAccount.AccountHolder,
                AccountNumber = updatedAccount.AccountNumber,
                Id = updatedAccount.Id,
                Payload = updatedAccount.Payload
            }, "Successfully update payout account");
        }
        catch (Exception ex)
        {
            return AppResult<CreateUpdatePayoutAccountResult>.CreateFailed(ex, "An error occured in CreateUpdatePayoutAccountHandler");
        }
    }
}