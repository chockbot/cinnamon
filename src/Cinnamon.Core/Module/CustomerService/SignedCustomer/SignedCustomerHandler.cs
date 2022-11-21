using System.Security.Claims;
using Cinnamon.Core.Common;
using Cinnamon.Core.Module.CustomerService.Interactors;
using Cinnamon.Core.Module.CustomerService.Interactors.Results;

namespace Cinnamon.Core.Module.CustomerService.Handler.SignedCustomer;

public class SignedCustomerHandler : ISignedCustomer 
{
    public AppResult<CurrentLoginResult> Execute(CurrentLogin args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CurrentLoginResult>.CreateFailed(ex, "An error occured in SignedCustomerHandler");
        }
    }

    public async Task<AppResult<CurrentLoginResult>> ExecuteAsync(CurrentLogin args)
    {
        try 
        {
            if(args.User == null)
            {
                return AppResult<CurrentLoginResult>.CreateFailed(new ApplicationException("Can't find current login user id"), "Can't find current login user id");
            }

            if(args.User.Identity != null && (!args.User.Identity.IsAuthenticated))
            {
                return AppResult<CurrentLoginResult>.CreateFailed(new ApplicationException("Don't have user login"), "Don't have user login");
            }

            var userId = args.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
            {
                return AppResult<CurrentLoginResult>.CreateFailed(new ApplicationException("Can't find current login user id"), "Can't find current login user id");
            }

            var customer = await CoreDI.DataStore.Customers.GetCustomerByUserId(userId);
            if(customer == null)
            {
                return AppResult<CurrentLoginResult>.CreateFailed(new ApplicationException("Can't find customer data"), "Can't find customer data");
            }

            return AppResult<CurrentLoginResult>.CreateSucceeded(new CurrentLoginResult { Customer = customer }, "Success get current customer login");

        }
        catch (Exception ex)
        {
            return AppResult<CurrentLoginResult>.CreateFailed(ex, "An error occured in SignedCustomerHandler");
        }
    }
}