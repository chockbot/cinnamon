using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using Flurl;

namespace Cinnamon.Api.Core.Services.AccountService;

public class SubmitResendVerificationHandler : ISubmitResendVerificationHandler
{
    private readonly IWaitListData waitListData;
    private readonly ISendVerifyEmailHandler sendVerifyEmailHandler;
    private readonly IResendEmailData resendEmailData;
    private readonly ITokenGeneratorProvider tokenGeneratorProvider;

    public SubmitResendVerificationHandler(IWaitListData waitListData, ISendVerifyEmailHandler sendVerifyEmailHandler,
        IResendEmailData resendEmailData, ITokenGeneratorProvider tokenGeneratorProvider)
    {
        this.waitListData = waitListData;
        this.sendVerifyEmailHandler = sendVerifyEmailHandler;
        this.resendEmailData = resendEmailData;
        this.tokenGeneratorProvider = tokenGeneratorProvider;
    }
    
    public AppResult<SubmitResendVerificationResult> Execute(SubmitResendVerificationArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<SubmitResendVerificationResult>.CreateFailed(ex, "An error occured in SubmitWaitlistHandler");
        }
    }

    public async Task<AppResult<SubmitResendVerificationResult>> ExecuteAsync(SubmitResendVerificationArgs args)
    {
        try
        {
            if(string.IsNullOrEmpty(args.Email))
            {
                return AppResult<SubmitResendVerificationResult>.CreateFailed(new ApplicationException("Provide valid email"), "Provide valid email");
            }

            // check first if email already in the wait list
            var emailCheck = await waitListData.GetWaitListByEmail(args.Email);
            if(!emailCheck.Succeeded || emailCheck.Result == null)
                return AppResult<SubmitResendVerificationResult>.CreateFailed(emailCheck.Error.Exception, emailCheck.Message);
            
            if(emailCheck.Succeeded && !emailCheck.Result.IsSuccess)
            {
                return AppResult<SubmitResendVerificationResult>.CreateFailed(new ApplicationException("Email not yet registered"), "Email not yet registered");
            }

            // if email already verified not need to resend email
            if(emailCheck.Result.Result.IsVerified)
            {
                return AppResult<SubmitResendVerificationResult>.CreateFailed(new ApplicationException("Can't resend email, already verified"), "Can't resend email, already verified");
            }

            // check if email already resend
            DateTime to = DateTime.Now, from = to.AddDays(-1);
            var resendRes = await resendEmailData.GetResendEmailByDataRange(new Framework.ApiCommand.ApiData.ResendEmail.Request.GetResendEmailByEmailDateRangeArgs {
                DateFrom = from.ToString("yyyyMMddHHmmss"), DateTo = to.ToString("yyyyMMddHHmmss"), Email = args.Email
            });
            if(!resendRes.Succeeded || resendRes.Result == null)
            {
                return AppResult<SubmitResendVerificationResult>.CreateFailed(resendRes.Error.Exception, resendRes.Message);
            }

            if(resendRes.Succeeded && !resendRes.Result.IsSuccess)
            {
                return AppResult<SubmitResendVerificationResult>.CreateFailed(new ApplicationException(resendRes.Result.ErrorInfo?.Message), "An error occured. Please try again later");
            }

            if(resendRes.Result.Result.Count() > 0)
            {
                return AppResult<SubmitResendVerificationResult>.CreateFailed(new ApplicationException("Can resend 1 email per day"), "Can resend 1 email per day", "RESENDEXCEED");
            }

            // generate token and guid
            var tokenGenerated = tokenGeneratorProvider.Generator();

            var updateRes = await waitListData.UpdateWaitlist(new Framework.ApiCommand.ApiData.Waitlist.Request.UpdateWaitlistArgs {
                Email = args.Email,
                Guid = tokenGenerated.Guid,
                Token = tokenGenerated.Token
            });

            if(!updateRes.Succeeded || updateRes.Result == null)
            {
                return AppResult<SubmitResendVerificationResult>.CreateFailed(new ApplicationException(updateRes.Message), updateRes.Message);
            }
            var updated = updateRes.Result.Result;

            var verificationLink = args.ValidationRoute.SetQueryParams(new {userid = tokenGenerated.Guid, token = tokenGenerated.Token}).ToString();

            // send email verification link
            var sendEmailRes = await sendVerifyEmailHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.SendVerifyEmailArgs {
                Email = args.Email,
                VerificationLink = verificationLink
            });
            if(!sendEmailRes.Succeeded)
            {
                return AppResult<SubmitResendVerificationResult>.CreateFailed(
                    new ApplicationException("An error occured when sending email verification link"), "An error occured when sending email verification link");
            }

            var createResendRes = await resendEmailData.CreateEmailResend(new Framework.ApiCommand.ApiData.ResendEmail.Request.CreateEmailResendArgs {
                Email = args.Email
            });
            if(!createResendRes.Succeeded || createResendRes.Result == null)
            {
                return AppResult<SubmitResendVerificationResult>.CreateFailed(new ApplicationException(createResendRes.Message), createResendRes.Message);
            }

            if(createResendRes.Succeeded && !createResendRes.Result.IsSuccess)
            {
                return AppResult<SubmitResendVerificationResult>.CreateFailed(
                    new ApplicationException(createResendRes.Result.ErrorInfo?.Message), "An error occured in SubmitWaitlistHandler");
            }

            return AppResult<SubmitResendVerificationResult>.CreateSucceeded(new SubmitResendVerificationResult {
                Email = updated.Email,
                Guid = updated.Guid,
                Token = tokenGenerated.Token,
                Id = updated.Id,
                VerificationLink = verificationLink
            }, "Successfully resend email");

        }
        catch (Exception ex)
        {
            return AppResult<SubmitResendVerificationResult>.CreateFailed(ex, "An error occured in SubmitWaitlistHandler");
        }
    }
}