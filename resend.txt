using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Cinnamon.Core.Common;
using Cinnamon.Core.Config;
using Cinnamon.Core.Models;
using Cinnamon.Core.Extensions;
using Cinnamon.Core.Module.NotificationService.Interactors;
using Cinnamon.Core.Module.NotificationService.Handler;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors.Results;

namespace Cinnamon.Core.Module.CinnamonMakerService.Handler.WaitingList;

public class ResendEmailHandler : IResendEmail 
{
    private readonly IEmailVerification emailVerification;
    private readonly CoreConfig coreConfig;

    public ResendEmailHandler(IEmailVerification emailVerification, CoreConfig coreConfig)
    {
        this.emailVerification = emailVerification;
        this.coreConfig = coreConfig;
    }

    public AppResult<ResendEmailResult> Execute(ResendEmailArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<ResendEmailResult>.CreateFailed(ex, "An error occured in ResendEmailHandler");
        }
    }

    public async Task<AppResult<ResendEmailResult>> ExecuteAsync(ResendEmailArgs args)
    {
        try
        {
            if(string.IsNullOrEmpty(args.Email))
            {
                return AppResult<ResendEmailResult>.CreateFailed(new ApplicationException("Provide valid email"), "Provide valid email");
            }

            // check if email already in waiting list
            var waitRes = await CoreDI.DataStore.WaitList.GetWaitListByEmail(args.Email);
            if(waitRes == null)
            {
                return AppResult<ResendEmailResult>.CreateFailed(new ApplicationException("Email not yet registered"), "Email not yet registered");
            }

            // email already verified
            if(waitRes.IsVerified)
            {
                return AppResult<ResendEmailResult>.CreateFailed(new ApplicationException("Can't resend email, already verified"), "Can't resend email, already verified");
            }

            // check if email already resend
            DateTime to = DateTime.Now, from = to.AddDays(-1);
            var resendRes = await CoreDI.DataStore.ResendEmail.GetLisResendEmailAsync(args.Email, from, to);
            if(resendRes.Count > 0)
            {
                return AppResult<ResendEmailResult>.CreateFailed(new ApplicationException("Can resend 1 email per day"), "Can resend 1 email per day", "RESENDEXCEED");
            }

            var guid = Guid.NewGuid();
            var timestamp = DateTime.UtcNow;

            // generate token
            byte[] time = BitConverter.GetBytes(timestamp.ToBinary());
            byte[] key = guid.ToByteArray();
            var token = Convert.ToBase64String(time.Concat(key).ToArray());
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            // update waiting res token and guid
            waitRes.Token = token;
            waitRes.Guid = guid.ToString();
            var updatedWaitingRes = await CoreDI.DataStore.WaitList.SaveDataAsync(waitRes);
            if(!updatedWaitingRes.Message.ToLower().Contains("saved"))
            {
                return AppResult<ResendEmailResult>.CreateFailed(
                    new ApplicationException("An error ocurred when saving updated waiting data"), "An error ocurred when saving updated waiting data");
            }

            var verificationLink = $"{coreConfig.BaseUrl}/Confirm-Email/?userid={guid.ToString()}&token={encodedToken}";
            var emailRes = await emailVerification.ExecuteAsync(new EmailVerification { Email = args.Email, VerificationLink = verificationLink });
            if(!emailRes.Succeeded) 
            {
                return AppResult<ResendEmailResult>.CreateFailed(emailRes.Error.Exception, emailRes.Message);
            }

            // save resend email data
            var resendPayload = new ResendEmailModel
            {
                Email = args.Email,
                DateResend = (DateTime.Now).SetKindUtc()
            };
            var resendDataRes = await CoreDI.DataStore.ResendEmail.SaveDataAsync(resendPayload);
            if(!resendDataRes.Message.ToLower().Contains("saved"))
            {
                return AppResult<ResendEmailResult>.CreateFailed(
                    new ApplicationException("An error ocurred when saving resend data"), "An error ocurred when saving resend data");
            }

            return AppResult<ResendEmailResult>.CreateSucceeded(new ResendEmailResult { GeneratedVerificationLink = verificationLink }, "Resend email success");

        }
        catch (Exception ex)
        {
            return AppResult<ResendEmailResult>.CreateFailed(ex, "An error occured in ResendEmailHandler");
        }
    }
}