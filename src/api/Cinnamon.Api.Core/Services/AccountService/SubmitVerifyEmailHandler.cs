using System.Text;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using Microsoft.AspNetCore.WebUtilities;

namespace Cinnamon.Api.Core.Services.AccountService;

public class SubmitVerifyEmailHandler : ISubmitVerifyEmailHandler
{
    private readonly IWaitListData waitListData;

    public SubmitVerifyEmailHandler(IWaitListData waitListData)
    {
        this.waitListData = waitListData;   
    }

    public AppResult<SubmitVerifyEmailResult> Execute(SubmitVerifyEmailArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<SubmitVerifyEmailResult>.CreateFailed(ex, "An error occured in SubmitVerifyEmailHandler");
        }
    }

    public async Task<AppResult<SubmitVerifyEmailResult>> ExecuteAsync(SubmitVerifyEmailArgs args)
    {
        try
        {
            if(string.IsNullOrEmpty(args.Token) || string.IsNullOrEmpty(args.UserId))
            {
                return AppResult<SubmitVerifyEmailResult>.CreateFailed(new ApplicationException("Provide valid token and userId"), "Provide valid token and userId");
            }

            // get waitlist data by guid
            var checkGuid = await waitListData.GetWaitListByGuid(args.UserId);
            if(!checkGuid.Succeeded || checkGuid.Result == null)
                return AppResult<SubmitVerifyEmailResult>.CreateFailed(new ApplicationException("Can't find user id provided"),"Can't find user id provided");

            if(checkGuid.Succeeded && !checkGuid.Result.IsSuccess)
                return AppResult<SubmitVerifyEmailResult>.CreateFailed(new ApplicationException("Can't find user id provided"),"Can't find user id provided");

            var waitlist = checkGuid.Result.Result;
            if(!waitlist.IsVerified)
            {
                // check token if the same
                var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(args.Token));
                if (decodedToken != waitlist.Token)
                {
                    return AppResult<SubmitVerifyEmailResult>.CreateFailed(new ApplicationException("Invalid userid or token"), "Invalid userid or token");
                }

                var updated = await waitListData.UpdateWaitlist(new Framework.ApiCommand.ApiData.Waitlist.Request.UpdateWaitlistArgs {
                    Email = waitlist.Email,
                    IsVerified = true
                });

                if(!updated.Succeeded || updated.Result == null)
                {
                    return AppResult<SubmitVerifyEmailResult>.CreateFailed(
                        new ApplicationException("An error occured when updating wait list data"), "An error occured when updating wait list data");
                }

                if(updated.Succeeded && !updated.Result.IsSuccess)
                {
                    return AppResult<SubmitVerifyEmailResult>.CreateFailed(
                        new ApplicationException("An error occured when updating wait list data"), "An error occured when updating wait list data");
                }
            }

            return AppResult<SubmitVerifyEmailResult>.CreateSucceeded(new SubmitVerifyEmailResult {
                Email = waitlist.Email,
                Guid = args.UserId,
                Id = waitlist.Id,
                Token = args.Token,
                IsVerified = true
            }, "Email successfully verified");
        }
        catch (Exception ex)
        {
            return AppResult<SubmitVerifyEmailResult>.CreateFailed(ex, "An error occured in SubmitVerifyEmailHandler");
        }
    }
}