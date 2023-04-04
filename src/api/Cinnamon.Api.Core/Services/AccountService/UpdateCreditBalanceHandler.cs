using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class UpdateCreditBalanceHandler : IUpdateCreditBalanceHandler
{
    private readonly ICustomerData customerData;

    public UpdateCreditBalanceHandler(ICustomerData customerData)
    {
        this.customerData = customerData;
    }

    public AppResult<UpdateCreditBalanceResult> Execute(UpdateCreditBalanceArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateCreditBalanceResult>.CreateFailed(ex, "An error occured in UpdateCreditBalanceHandler");
        }
    }

    public async Task<AppResult<UpdateCreditBalanceResult>> ExecuteAsync(UpdateCreditBalanceArgs args)
    {
        try
        {
            var customerInfo = await customerData.GetCustomerById(args.CustomerId);
            if(!customerInfo.Succeeded || customerInfo.Result == null || !customerInfo.Result.IsSuccess)
            {
                return AppResult<UpdateCreditBalanceResult>.CreateFailed(new ApplicationException(customerInfo.Result?.ErrorInfo?.Message), customerInfo.Message);
            }
            var customerProfile = customerInfo.Result.Result;

            var actions = new int[] {0 , 1};
            if(!actions.Contains(args.ActionFlag))
            {
                return AppResult<UpdateCreditBalanceResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request.");
            }

            // convert amount to absolute value, remove negative amounts
            args.Amount = Math.Abs(args.Amount);

            if(args.ActionFlag == 1 && customerProfile.TotalCredits < args.Amount)
            {
                return AppResult<UpdateCreditBalanceResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request.");
            }

            var newTotaCredits = args.ActionFlag switch {
                0 => customerProfile.TotalCredits + args.Amount,
                1 => customerProfile.TotalCredits - args.Amount,
                _ => customerProfile.TotalCredits
            };

            var updatedCredit = await customerData.UpdateCustomer(new Cinnamon.Framework.ApiCommand.ApiData.Customer.Request.UpdateCustomerArgs {
                CustomerId = args.CustomerId,
                TotalCredits = newTotaCredits
            });
            if(!updatedCredit.Succeeded || updatedCredit.Result == null || !updatedCredit.Result.IsSuccess)
            {
                return AppResult<UpdateCreditBalanceResult>.CreateFailed(new ApplicationException(updatedCredit.Result?.ErrorInfo?.Message), updatedCredit.Message);
            }

            return AppResult<UpdateCreditBalanceResult>.CreateSucceeded(new UpdateCreditBalanceResult {}, "Successfully update total credits");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateCreditBalanceResult>.CreateFailed(ex, "An error occured in UpdateCreditBalanceHandler");
        }
    }
}