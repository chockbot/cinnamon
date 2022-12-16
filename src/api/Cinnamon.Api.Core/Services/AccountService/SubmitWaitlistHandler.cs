using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.Waitlist.Request;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Flurl;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;

namespace Cinnamon.Api.Core.Services.AccountService;

public class SubmitWaitlistHandler : ISubmitWaitlistHandler
{
    private readonly IWaitListData waitListData;
    private readonly ISendVerifyEmailHandler sendVerifyEmailHandler;

    public SubmitWaitlistHandler(IWaitListData waitListData, ISendVerifyEmailHandler sendVerifyEmailHandler)
    {
        this.waitListData = waitListData;
        this.sendVerifyEmailHandler = sendVerifyEmailHandler;
    }

    public AppResult<SubmitWaitlistResult> Execute(SubmitWaitlistArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<SubmitWaitlistResult>.CreateFailed(ex, "An error occured in SubmitWaitlistHandler");
        }
    }

    public async Task<AppResult<SubmitWaitlistResult>> ExecuteAsync(SubmitWaitlistArgs args)
    {
        try
        {
            // check first if email already in the wait list
            var emailCheck = await waitListData.GetWaitListByEmail(args.Email);
            if(!emailCheck.Succeeded)
                return AppResult<SubmitWaitlistResult>.CreateFailed(emailCheck.Error.Exception, emailCheck.Message);

            // email already existed
            if(emailCheck.Result != null && emailCheck.Result.IsSuccess)
                return AppResult<SubmitWaitlistResult>.CreateFailed(new ApplicationException("Email already existed."),"Email already existed.");
            
            // generate token and guid
            var guid = Guid.NewGuid();
            var timestamp = DateTime.UtcNow;

            // generate token
            byte[] time = BitConverter.GetBytes(timestamp.ToBinary());
            byte[] key = guid.ToByteArray();
            var token = Convert.ToBase64String(time.Concat(key).ToArray());
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            
            var createRes = await waitListData.CreateWaitlist(new CreateWaitlistArgs {
                Email = args.Email,
                Token = token.ToString(),
                Guid = guid.ToString()
            });

            if(!createRes.Succeeded || createRes.Result == null)
            {
                return AppResult<SubmitWaitlistResult>.CreateFailed(createRes.Error.Exception, createRes.Message);
            }
            var created = createRes.Result.Result;

            var verificationLink = args.ValidationRoute.SetQueryParams(new {userid = guid.ToString(), token = encodedToken}).ToString();

            // send email verification link
            var sendEmailRes = await sendVerifyEmailHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.SendVerifyEmailArgs {
                Email = args.Email,
                VerificationLink = verificationLink
            });
            if(!sendEmailRes.Succeeded)
            {
                return AppResult<SubmitWaitlistResult>.CreateFailed(
                    new ApplicationException("An error occured when sending email verification link"), "An error occured when sending email verification link");
            }

            return AppResult<SubmitWaitlistResult>.CreateSucceeded(new SubmitWaitlistResult {
                Email = created.Email,
                Guid = created.Guid,
                Token = encodedToken,
                Id = created.Id,
                VerificationLink = verificationLink
            }, "Successfully submit waitlist");
        }
        catch (Exception ex)
        {
            return AppResult<SubmitWaitlistResult>.CreateFailed(ex, "An error occured in SubmitWaitlistHandler");
        }
    }
}