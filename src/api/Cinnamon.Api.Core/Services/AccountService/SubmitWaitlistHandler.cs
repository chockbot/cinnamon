using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.Waitlist.Request;
using Flurl;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Providers;

namespace Cinnamon.Api.Core.Services.AccountService;

public class SubmitWaitlistHandler : ISubmitWaitlistHandler
{
    private readonly IWaitListData waitListData;
    private readonly ISendVerifyEmailHandler sendVerifyEmailHandler;
    private readonly ICustomerData customerData;
    private readonly ITokenGeneratorProvider tokenGeneratorProvider;

    public SubmitWaitlistHandler(IWaitListData waitListData, ISendVerifyEmailHandler sendVerifyEmailHandler,
        ICustomerData customerData, ITokenGeneratorProvider tokenGeneratorProvider)
    {
        this.waitListData = waitListData;
        this.sendVerifyEmailHandler = sendVerifyEmailHandler;
        this.customerData = customerData;
        this.tokenGeneratorProvider = tokenGeneratorProvider;
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
            // check if email already registered
            var customerRes = await customerData.GetCustomerByEmail(args.Email);
            if(!customerRes.Succeeded || customerRes.Result == null)
            {
                return AppResult<SubmitWaitlistResult>.CreateFailed(customerRes.Error.Exception, customerRes.Message);
            }

            if(customerRes.Succeeded && customerRes.Result.IsSuccess)
            {
                return AppResult<SubmitWaitlistResult>.CreateFailed(
                    new ApplicationException("Email already in registered"), "Email already registered", "EMAIL-ALREADY-REGISTERED");
            }

            // check first if email already in the wait list
            var emailCheck = await waitListData.GetWaitListByEmail(args.Email);
            if(!emailCheck.Succeeded || emailCheck.Result == null)
                return AppResult<SubmitWaitlistResult>.CreateFailed(emailCheck.Error.Exception, emailCheck.Message);

            if(emailCheck.Succeeded && emailCheck.Result.IsSuccess && !emailCheck.Result.Result.IsVerified)
            {
                return AppResult<SubmitWaitlistResult>.CreateFailed(
                    new ApplicationException("Email already existed but not verified."),"Email already existed but not verified.", "EMAIL-ALREADY-REGISTERED-NOT-VERIFIED");
            }

            if(emailCheck.Succeeded && emailCheck.Result.IsSuccess && emailCheck.Result.Result.IsVerified)
            {
                return AppResult<SubmitWaitlistResult>.CreateFailed(
                    new ApplicationException("Email already existed but already verified."),"Email already existed but already verified.", "EMAIL-ALREADY-REGISTERED-VERIFIED");
            }
            
            // generate token and guid
            var tokenGenerated = tokenGeneratorProvider.Generator();
            
            var createRes = await waitListData.CreateWaitlist(new CreateWaitlistArgs {
                Email = args.Email,
                Token = tokenGenerated.Token,
                Guid = tokenGenerated.Guid
            });

            if(!createRes.Succeeded || createRes.Result == null)
            {
                return AppResult<SubmitWaitlistResult>.CreateFailed(createRes.Error.Exception, createRes.Message);
            }
            var created = createRes.Result.Result;

            var verificationLink = args.ValidationRoute.SetQueryParams(new {userid = tokenGenerated.Guid, token = tokenGenerated.Token}).ToString();

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
                Token = tokenGenerated.Token,
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