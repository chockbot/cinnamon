using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Cinnamon.Core.Models;
using Cinnamon.Core.Common;
using Cinnamon.Core.Module.CustomerService.Interactors;
using Cinnamon.Core.Module.CustomerService.Interactors.Results;

namespace Cinnamon.Core.Module.CustomerService.Handler.SignedCustomer;

public class SignedCustomerHandler : ISignedCustomer 
{
    private readonly IHttpContextAccessor httpContext;

    public SignedCustomerHandler(IHttpContextAccessor httpContext)
    {
        this.httpContext = httpContext;
    }

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
            // dont have current user login
            if(httpContext.HttpContext.User != null || !httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return AppResult<CurrentLoginResult>.CreateFailed(new ApplicationException("Currently don't have login user"), "Currently don't have login user");
            }

            var userId = httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
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