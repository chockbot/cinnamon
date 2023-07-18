using System.Text;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using Flurl;
using Microsoft.AspNetCore.WebUtilities;

namespace Cinnamon.Api.Core.Services.AccountService;

public class ResetPasswordHandler : IResetPasswordHandler
{
    private readonly ICustomerData customerData;
    private readonly IResetPasswordData resetPasswordData;
    private readonly IResetPasswordNotificationHandler resetPasswordNotificationHandler;
    private readonly IIsAccountBlockedHandler isAccountBlockedHandler;

    public ResetPasswordHandler(ICustomerData customerData, IResetPasswordData resetPasswordData,
        IResetPasswordNotificationHandler resetPasswordNotificationHandler, IIsAccountBlockedHandler isAccountBlockedHandler)
    {
        this.customerData = customerData;
        this.resetPasswordData = resetPasswordData;
        this.resetPasswordNotificationHandler = resetPasswordNotificationHandler;
        this.isAccountBlockedHandler = isAccountBlockedHandler;
    }

    public AppResult<ResetPasswordResult> Execute(ResetPasswordArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<ResetPasswordResult>.CreateFailed(ex, "An error occured in ResetPasswordHandler");
        }
    }

    public async Task<AppResult<ResetPasswordResult>> ExecuteAsync(ResetPasswordArgs args)
    {
        try
        {
            // check first if customer existed using email
            var customerRes = await customerData.GetCustomerByEmail(args.Email);
            if(!customerRes.Succeeded || customerRes.Result == null || !customerRes.Result.IsSuccess)
            {
                return AppResult<ResetPasswordResult>.CreateFailed(new ApplicationException("Invalid email provided"), "Invalid email provided");
            }

            // check account if blocked
            var checkAccountBlocked = await isAccountBlockedHandler.ExecuteAsync(new IsAccountBlockedArgs {Email = args.Email});
            if(!checkAccountBlocked.Succeeded || checkAccountBlocked.Result == null)
            {
                return AppResult<ResetPasswordResult>.CreateFailed(new ApplicationException(checkAccountBlocked.Message), checkAccountBlocked.Message);
            }

            if(checkAccountBlocked.Result.IsAccountBlocked)
            {
                return AppResult<ResetPasswordResult>.CreateFailed(new ApplicationException("Your account blocked by the administrator. Please contact support"), "Your account blocked by the administrator. Please contact support");
            }

            // generated token
            var tokenRes = await customerData.GenerateResetPasswordToken(new Framework.ApiCommand.ApiData.Customer.Request.GenerateResetPasswordTokenArgs {
                Email = args.Email
            });
            if(!tokenRes.Succeeded || tokenRes.Result == null || !tokenRes.Result.IsSuccess)
            {
                return AppResult<ResetPasswordResult>.CreateFailed(new ApplicationException(tokenRes.Result?.ErrorInfo?.Message), tokenRes.Message);
            }

            // generate token and guid
            var guid = Guid.NewGuid();
            var timestamp = DateTime.UtcNow;

            // generate token
            byte[] time = BitConverter.GetBytes(timestamp.ToBinary());
            byte[] key = guid.ToByteArray();
            var token = Convert.ToBase64String(time.Concat(key).ToArray());
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            // save guid, token, generatedToken
            var createTokens = await resetPasswordData.CreateResetPassword(new Framework.ApiCommand.ApiData.ResetPassword.Request.CreateResetPasswordArgs {
                Email = args.Email,
                GeneratedToken = tokenRes.Result.Result,
                Guid = guid.ToString(),
                Token = token.ToString(),
                IsUsed = false
            });
            if(!createTokens.Succeeded || createTokens.Result == null || !createTokens.Result.IsSuccess)
            {
                return AppResult<ResetPasswordResult>.CreateFailed(new ApplicationException(createTokens.Result?.ErrorInfo?.Message), createTokens.Message);
            }
            var savedTokens = createTokens.Result.Result;

            var verificationLink = args.ValidationRoute.SetQueryParams(new {userid = guid.ToString(), token = encodedToken}).ToString();

            // send email verfication link
            var sendEmailLink = await resetPasswordNotificationHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.ResetPasswordNotificationArgs {
                Email = args.Email,
                VerificationLink = verificationLink
            });
            if(!sendEmailLink.Succeeded || sendEmailLink.Result == null)
            {
                return AppResult<ResetPasswordResult>.CreateFailed(new ApplicationException(sendEmailLink.Message), sendEmailLink.Message);
            }

            return AppResult<ResetPasswordResult>.CreateSucceeded(new ResetPasswordResult {
                Email = args.Email,
                Id = savedTokens.Id,
                VerificationLink = verificationLink
            }, "Successfully create reset password link");
        }
        catch (Exception ex)
        {   
            return AppResult<ResetPasswordResult>.CreateFailed(ex, "An error occured in ResetPasswordHandler");
        }
    }
}