using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.AccountService;
public class ChangeEmailHandler:IChangeEmailHandler
{
    private readonly ICustomerData customerData; 
    private readonly IIsAccountBlockedHandler isAccountBlockedHandler;
    public ChangeEmailHandler(ICustomerData customerData, IIsAccountBlockedHandler isAccountBlockedHandler)
    {
        this.customerData = customerData;
        this.isAccountBlockedHandler = isAccountBlockedHandler;
    }

    public AppResult<ChangeEmailResult> Execute(ChangeEmailArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<ChangeEmailResult>.CreateFailed(ex, "An error occured in ChangeEmailResult");
        }
    }

    public async Task<AppResult<ChangeEmailResult>> ExecuteAsync(ChangeEmailArgs args)
    {
        try
        {
            // check first if customer existed using email
            var customerRes = await customerData.GetCustomerByEmail(args.CurrentEmail);
            if (!customerRes.Succeeded || customerRes.Result == null || !customerRes.Result.IsSuccess)
            {
                return AppResult<ChangeEmailResult>.CreateFailed(new ApplicationException("Invalid email provided"), "Invalid email provided");
            }
            // check account if blocked
            var checkAccountBlocked = await isAccountBlockedHandler.ExecuteAsync(new IsAccountBlockedArgs { Email = args.CurrentEmail});
            if (!checkAccountBlocked.Succeeded || checkAccountBlocked.Result == null)
            {
                return AppResult<ChangeEmailResult>.CreateFailed(new ApplicationException(checkAccountBlocked.Message), checkAccountBlocked.Message);
            }
            if (checkAccountBlocked.Result.IsAccountBlocked)
            {
                return AppResult<ChangeEmailResult>.CreateFailed(new ApplicationException("Your account blocked by the administrator. Please contact support"), "Your account blocked by the administrator. Please contact support");
            }
            //Change Email Address
            var changeEmail = await customerData.ChangeEmailAddress(new Framework.ApiCommand.ApiData.Customer.Request.ChangeEmailAddressArgs
            {
                NewEmail = args.NewEmail,
                CurrentEmail = args.CurrentEmail,
            });
            if (!changeEmail.Succeeded || changeEmail.Result == null)
            {
                return AppResult<ChangeEmailResult>.CreateFailed(new ApplicationException(changeEmail.Message), changeEmail.Message);
            }
            if (changeEmail.Succeeded && !changeEmail.Result.IsSuccess)
            {
                return AppResult<ChangeEmailResult>.CreateFailed(
                    new ApplicationException(changeEmail.Result.ErrorInfo?.Message), "An error occured in ChangeEmailResult");
            }
            return AppResult<ChangeEmailResult>.CreateSucceeded(new ChangeEmailResult
            {
                Email = args.NewEmail,
            }, "Successfully change email address");
        }
        catch (Exception ex)
        {
            return AppResult<ChangeEmailResult>.CreateFailed(ex, "An error occured in ChangeEmailResult");
        }
    }
}
