using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class UpdateCustomerProfileHandler : IUpdateCustomerProfileHandler
{
    private readonly ICustomerData customerData;

    public UpdateCustomerProfileHandler(ICustomerData customerData)
    {
        this.customerData = customerData;
    }

    public AppResult<UpdateCustomerProfileResult> Execute(UpdateCustomerProfileArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateCustomerProfileResult>.CreateFailed(ex, "An error occured in UpdateCustomerProfileHandler");
        }
    }

    public async Task<AppResult<UpdateCustomerProfileResult>> ExecuteAsync(UpdateCustomerProfileArgs args)
    {
        try
        {
            var result = await customerData.UpdateCustomer(new Framework.ApiCommand.ApiData.Customer.Request.UpdateCustomerArgs
            {
                CustomerId = args.CustomerId,
                IsVerified = args.VerifiedBadge,
                IsVerifiedDate = args.IsVerifiedDate,
                IsOG = args.IsOG,
                IsOGDate = args.IsOGDate,
                IsOF = args.IsOF,
                IsOFDate = args.IsOFDate,   
            });
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<UpdateCustomerProfileResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<UpdateCustomerProfileResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in UpdateCustomerProfileHandler");
            }

            return AppResult<UpdateCustomerProfileResult>.CreateSucceeded(new UpdateCustomerProfileResult
            {
                FirstName = result.Result.Result.FirstName,
                LastName = result.Result.Result.LastName,
                Id = result.Result.Result.Id,
                VerifiedBadge = result.Result.Result.IsVerified,
                IsVerifiedDate = result.Result.Result.IsVerifiedObtainedDate,
                IsOG = result.Result.Result.IsOG,
                IsOGDate = result.Result.Result.IsOGObtainedDate,
                IsOF = result.Result.Result.IsOfficial,
                IsOFDate = result.Result.Result.IsOfficialObtainedDate
            }, "Successfully updated profile details");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateCustomerProfileResult>.CreateFailed(ex, "An error occured in UpdateCustomerProfileHandler");
        }
    }
}