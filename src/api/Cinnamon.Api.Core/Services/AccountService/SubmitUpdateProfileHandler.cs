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
    private readonly IGenerateCustomerHandler generateCustomerHandler;

    public SubmitUpdateProfileHandler(ICustomerData customerData, IGetProfileHandler getProfileHandler,
        IGenerateCustomerHandler generateCustomerHandler)
    {
        this.customerData = customerData;
        this.getProfileHandler = getProfileHandler;
        this.generateCustomerHandler = generateCustomerHandler;
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
            if(args.Birthdate.HasValue)
            {
                var age = DateTime.Today.Year - args.Birthdate.GetValueOrDefault().Year;
                if (age < 18 || age > 120)
                {
                    return AppResult<SubmitUpdateProfileResult>.CreateFailed(
                        new ApplicationException("Please provide valid birth year. Age between 18 and 120"), "Please provide valid birth year. Age between 18 and 120");
                }
            }

            // update handler only if have changes in firstname and lastname
            var handler = profile.Handler;
            if((!string.IsNullOrEmpty(args.FirstName) && args.FirstName.ToLower() != profile.FirstName.ToLower()) || 
                (!string.IsNullOrEmpty(args.LastName) && args.LastName.ToLower() != profile.LastName.ToLower()))
            {
                var generateHandlerRes = await generateCustomerHandler.ExecuteAsync(new GenerateCustomerHandlerArgs {
                    Handler = $"{args.FirstName ?? profile.FirstName} {args.LastName ?? profile.LastName}"
                });
                if(!generateHandlerRes.Succeeded || generateHandlerRes.Result == null)
                {
                    return AppResult<SubmitUpdateProfileResult>.CreateFailed(new ApplicationException(generateHandlerRes.Message), generateHandlerRes.Message);
                }

                handler = generateHandlerRes.Result.GeneratedHandler;
            }

            var result = await customerData.UpdateCustomer(new Framework.ApiCommand.ApiData.Customer.Request.UpdateCustomerArgs {
                About = args.About ?? profile.About,
                FirstName = args.FirstName ?? profile.FirstName,
                LastName = args.LastName ?? profile.LastName,
                Birthdate = args.Birthdate ?? profile.Birthdate,
                PhoneNumber = args.PhoneNumber ?? profile.PhoneNumber,
                CustomerId = profile.Id,
                IsVerified = args.VerifiedBadge ?? profile.IsVerified,
                Handler = handler
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
                PhoneNumber = result.Result.Result.PhoneNumber,
                Id = result.Result.Result.Id
            }, "Successfully update profile details");
        }
        catch (Exception ex)
        {
            return AppResult<SubmitUpdateProfileResult>.CreateFailed(ex, "An error occured in SubmitUpdateProfileHandler");
        }
    }
}