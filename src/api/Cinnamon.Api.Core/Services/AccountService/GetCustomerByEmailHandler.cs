using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;
public class GetCustomerByEmailHandler: IGetCustomerByEmailHandler
{
    private readonly ICustomerData customerData;
    private readonly IHttpContextAccessor httpContext;
    public GetCustomerByEmailHandler(ICustomerData customerData, IHttpContextAccessor httpContext)
    {
        this.customerData = customerData;
        this.httpContext = httpContext; 
    }

    public AppResult<GetCustomerByEmailResult> Execute(GetCustomerByEmailArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetCustomerByEmailResult>.CreateFailed(ex, "An error occured in GetCustomerByEmailHandler");
        }
    }

    public async Task<AppResult<GetCustomerByEmailResult>> ExecuteAsync(GetCustomerByEmailArgs args)
    {
        try
        {
            //// get customer id saved in claims
            //var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            //if (customerId == null)
            //{
            //    return AppResult<GetCustomerByEmailResult>.CreateFailed(
            //        new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            //}
            //int id = Convert.ToInt32(customerId);

            var result = await customerData.GetCustomerByEmail(args.Email);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetCustomerByEmailResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetCustomerByEmailResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetGovernmentIdsHandler");
            }

            return AppResult<GetCustomerByEmailResult>.CreateSucceeded(new GetCustomerByEmailResult
            {
                About       = result.Result.Result.About,
                Birthdate   = result.Result.Result.Birthdate,
                DateJoined  = result.Result.Result.DateJoined,
                Email       = result.Result.Result.Email,  
                Id          = result.Result.Result.Id,
                IsMaker     = result.Result.Result.IsMaker,
                IsVerified  = result.Result.Result.IsVerified,
                IsOG        = result.Result.Result.IsOG,
                IsOfficial  = result.Result.Result.IsOfficial,
                ProfileImg  = result.Result.Result.ProfileImg,
                IsGuest     = result.Result.Result.IsGuest,
            }, "Successfully getting customer information");
        }
        catch (Exception ex)
        {
            return AppResult<GetCustomerByEmailResult>.CreateFailed(ex, "An error occured in GetGovernmentIdsHandler");
        }
    }
}
