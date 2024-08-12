using System.Text;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using Microsoft.AspNetCore.WebUtilities;

namespace Cinnamon.Api.Core.Services.AccountService;

public class VerifyResetPasswordHandler : IVerifyResetPasswordHandler
{
    private readonly IResetPasswordData resetPasswordData;
    private readonly ICustomerData customerData;
    private readonly IVerifyResetPasswordNotificationHandler verifyResetPasswordNotification;

    public VerifyResetPasswordHandler(IResetPasswordData resetPasswordData, ICustomerData customerData,
        IVerifyResetPasswordNotificationHandler verifyResetPasswordNotification)
    {
        this.resetPasswordData = resetPasswordData;
        this.customerData = customerData;
        this.verifyResetPasswordNotification = verifyResetPasswordNotification;
    }

    public AppResult<VerifyResetPasswordResult> Execute(VerifyResetPasswordArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<VerifyResetPasswordResult>.CreateFailed(ex, "An error occured when verifying reset password.");
        }
    }

    public async Task<AppResult<VerifyResetPasswordResult>> ExecuteAsync(VerifyResetPasswordArgs args)
    {
        try
        {
            // check token if the same
            var resetPaswordDetail = await resetPasswordData.GetResetPasswordByGuidToken(new Framework.ApiCommand.ApiData.ResetPassword.Request.GetResetPasswordArgs {
                Guid = args.Guid,
                Token = args.Token,
            });
            if(!resetPaswordDetail.Succeeded || resetPaswordDetail.Result == null || !resetPaswordDetail.Result.IsSuccess)
            {
                return AppResult<VerifyResetPasswordResult>.CreateFailed(new ApplicationException("Invalid token provided."), "Invalid token provided.");
            }
            var tokenDetail = resetPaswordDetail.Result.Result;

            if(tokenDetail.IsUsed)
            {
                return AppResult<VerifyResetPasswordResult>.CreateFailed(new ApplicationException("Invalid token provided."), "Invalid token provided.");
            }

            // reset password in db
            var customerResettedPasswordRes = await customerData.ResetPassword(new Framework.ApiCommand.ApiData.Customer.Request.ResetPasswordArgs{
                Email = tokenDetail.Email,
                Password = args.NewPassword,
                Token = tokenDetail.GeneratedToken
            });
            if(!customerResettedPasswordRes.Succeeded || customerResettedPasswordRes.Result == null || !customerResettedPasswordRes.Result.IsSuccess)
            {
                return AppResult<VerifyResetPasswordResult>.CreateFailed(
                    new ApplicationException(customerResettedPasswordRes.Result?.ErrorInfo?.Message), customerResettedPasswordRes.Message);
            }

            // update tokens to already used
            var updatedToken = await resetPasswordData.UpdateResetPassword(new Framework.ApiCommand.ApiData.ResetPassword.Request.UpdateResetPasswordArgs {
                Id = tokenDetail.Id,
                IsUsed = true
            });
            if(!updatedToken.Succeeded || updatedToken.Result == null || !updatedToken.Result.IsSuccess)
            {
                return AppResult<VerifyResetPasswordResult>.CreateFailed(
                    new ApplicationException(updatedToken.Result?.ErrorInfo?.Message), updatedToken.Message);
            }

            var sendNotificationRes = await verifyResetPasswordNotification.ExecuteAsync(new Modules.NotificationDriver.Interactors.VerifyResetPasswordNotificationArgs {
                DateChanged = DateTime.Now,
                Email = tokenDetail.Email
            });
            if(!sendNotificationRes.Succeeded || sendNotificationRes.Result == null)
            {
                return AppResult<VerifyResetPasswordResult>.CreateFailed(
                    new ApplicationException(sendNotificationRes.Message), sendNotificationRes.Message);
            }

            return AppResult<VerifyResetPasswordResult>.CreateSucceeded(new VerifyResetPasswordResult {}, "Successfully change password");
        }
        catch (Exception ex)
        {
            return AppResult<VerifyResetPasswordResult>.CreateFailed(ex, "An error occured when verifying reset password.");
        }
    }
}