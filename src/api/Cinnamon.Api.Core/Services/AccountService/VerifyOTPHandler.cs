using System.Text;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using Microsoft.AspNetCore.WebUtilities;

namespace Cinnamon.Api.Core.Services.AccountService;
public class VerifyOTPHandler : IVerifyOTPHandler
{
    private readonly IGuestOTPData guestOTPData;
    private readonly IWaitListData waitListData;

    public VerifyOTPHandler(IGuestOTPData guestOTPData, IWaitListData waitListData)
    {
        this.guestOTPData = guestOTPData;
        this.waitListData = waitListData;
    }

    public AppResult<VerifyOTPResult> Execute(VerifyOTPArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<VerifyOTPResult>.CreateFailed(ex, "An error occured in SubmitVerifyEmailHandler");
        }
    }

    public async Task<AppResult<VerifyOTPResult>> ExecuteAsync(VerifyOTPArgs args)
    {
        try
        {
            if (string.IsNullOrEmpty(args.Email))
            {
                return AppResult<VerifyOTPResult>.CreateFailed(new ApplicationException("Provide valid email"), "Provide valid email");
            }
            var checkEmail = await waitListData.GetWaitListByEmail(args.Email);
            if (!checkEmail.Succeeded || checkEmail.Result == null)
                return AppResult<VerifyOTPResult>.CreateFailed(new ApplicationException("Can't find email provided"), "Can't find email provided");
            if (checkEmail.Succeeded && !checkEmail.Result.IsSuccess)
                return AppResult<VerifyOTPResult>.CreateFailed(new ApplicationException("Can't find email provided"), "Can't find email provided");

            var waitlist = checkEmail.Result.Result;

            if (!waitlist.IsVerified)
            {
                var updated = await waitListData.UpdateWaitlist(new Framework.ApiCommand.ApiData.Waitlist.Request.UpdateWaitlistArgs
                {
                    Email = waitlist.Email,
                    IsVerified = true
                });

                if (!updated.Succeeded || updated.Result == null)
                {
                    return AppResult<VerifyOTPResult>.CreateFailed(
                        new ApplicationException("An error occured when updating wait list data"), "An error occured when updating wait list data");
                }

                if (updated.Succeeded && !updated.Result.IsSuccess)
                {
                    return AppResult<VerifyOTPResult>.CreateFailed(
                        new ApplicationException("An error occured when updating wait list data"), "An error occured when updating wait list data");
                }
            }
            return AppResult<VerifyOTPResult>.CreateSucceeded(new VerifyOTPResult
            {
                Email = waitlist.Email,
            }, "Email successfully verified");
        }
        catch (Exception)
        {

            throw;
        }
    }
}
