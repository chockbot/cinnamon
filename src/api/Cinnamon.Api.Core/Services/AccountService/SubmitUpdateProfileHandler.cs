using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;

public class SubmitUpdateProfileHandler : ISubmitUpdateProfileHandler
{
    private readonly ICustomerData customerData;
    private readonly IGetProfileHandler getProfileHandler;

    public SubmitUpdateProfileHandler(ICustomerData customerData, IGetProfileHandler getProfileHandler)
    {
        this.customerData = customerData;
        this.getProfileHandler = getProfileHandler;
    }

    public AppResult<SubmitUpdateProfileResult> Execute(SubmitUpdateProfileArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<SubmitUpdateProfileResult>.CreateFailed(ex, "An error occured in SubmitUpdateProfileHandler");
        }
    }

    public async Task<AppResult<SubmitUpdateProfileResult>> ExecuteAsync(SubmitUpdateProfileArgs args)
    {
        try
        {
            // get profile details
            var profileRes = await getProfileHandler.ExecuteAsync(new GetProfileArgs {});
            if(!profileRes.Succeeded || profileRes.Result == null)
            {
                return AppResult<SubmitUpdateProfileResult>.CreateFailed(new ApplicationException(profileRes.Message), profileRes.Message);
            }
            var profile = profileRes.Result;

            // validate birthdate, age between 18 to 120
            var age = DateTime.Today.Year - args.Birthdate.GetValueOrDefault().Year;
            if (age < 18 || age > 120)
            {
                return AppResult<SubmitUpdateProfileResult>.CreateFailed(
                    new ApplicationException("Please provide valid birth year. Age between 18 and 120"), "Please provide valid birth year. Age between 18 and 120");
            }

            var result = await customerData.UpdateCustomer(new Framework.ApiCommand.ApiData.Customer.Request.UpdateCustomerArgs {
                About = args.About ?? profile.About,
                FirstName = args.FirstName ?? profile.FirstName,
                LastName = args.LastName ?? profile.LastName,
                Birthdate = args.Birthdate ?? profile.Birthdate,
                CustomerId = profile.Id,
                IsVerified = args.VerifiedBadge ?? profile.IsVerified
            });
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<SubmitUpdateProfileResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<SubmitUpdateProfileResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in SubmitUpdateProfileHandler"); 
            }

            return AppResult<SubmitUpdateProfileResult>.CreateSucceeded(new SubmitUpdateProfileResult {
                About = result.Result.Result.About,
                Birthdate = result.Result.Result.Birthdate,
                FirstName = result.Result.Result.FirstName,
                LastName = result.Result.Result.LastName,
                Id = result.Result.Result.Id
            }, "Successfully update profile details");
        }
        catch (Exception ex)
        {
            return AppResult<SubmitUpdateProfileResult>.CreateFailed(ex, "An error occured in SubmitUpdateProfileHandler");
        }
    }
}