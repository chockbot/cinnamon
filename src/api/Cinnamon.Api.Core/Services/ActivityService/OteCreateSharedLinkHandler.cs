using System.Text;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;
using Microsoft.AspNetCore.WebUtilities;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OteCreateSharedLinkHandler : IOteCreateSharedLinkHandler
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IOteFindByHandler oteFindByHandler;
    private readonly IOteTicketData oteTicketData;

    public OteCreateSharedLinkHandler(IGetProfileHandler getProfileHandler, IOteFindByHandler oteFindByHandler,
        IOteTicketData oteTicketData)
    {
        this.getProfileHandler = getProfileHandler;
        this.oteFindByHandler = oteFindByHandler;
        this.oteTicketData = oteTicketData;
    }

    public AppResult<OteCreateSharedLinkResult> Execute(OteCreateSharedLinkArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OteCreateSharedLinkResult>> ExecuteAsync(OteCreateSharedLinkArgs args)
    {
        try
        {
            var profileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!profileRes.Succeeded || profileRes.Result is null)
            {
                return AppResult<OteCreateSharedLinkResult>.CreateFailed(new ApplicationException(profileRes.Message), profileRes.Message);
            }
            var profile = profileRes.Result;

            var getOteRes = await oteFindByHandler.ExecuteAsync(new OteFindByHandlerArgs {
                Handler = args.Handler,
                IncludeSchedule = true
            });
            if(!getOteRes.Succeeded || getOteRes.Result is null)
            {
                return AppResult<OteCreateSharedLinkResult>.CreateFailed(new ApplicationException(getOteRes.Message), getOteRes.Message);
            }
            var oteActivity =  getOteRes.Result;

            if(oteActivity.ProviderId != profile.Id)
            {
                return AppResult<OteCreateSharedLinkResult>.CreateFailed(new ApplicationException("Invalid request. Action not allowed."), "Invalid request. Action not allowed.");
            }

            if(!oteActivity.OteDates.Any(d => d.Id == args.OteDateId))
            {
                return AppResult<OteCreateSharedLinkResult>.CreateFailed(new ApplicationException("Invalid request. Action not allowed."), "Invalid request. Action not allowed.");
            }

            // generate token and guid
            var guid = Guid.NewGuid();
            var timestamp = DateTime.UtcNow;
            byte[] time = BitConverter.GetBytes(timestamp.ToBinary());
            byte[] key = guid.ToByteArray();
            var token = Convert.ToBase64String(time.Concat(key).ToArray());
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var createShareLinkRes = await oteTicketData.CreateSharedLink(new Framework.ApiCommand.ApiData.OteTicket.Request.CreateSharedLinkArgs {
                ActivityId = oteActivity.Id,
                Guid = guid.ToString(),
                OteDateId = args.OteDateId,
                Token = encodedToken
            });
            if(!createShareLinkRes.Succeeded || createShareLinkRes.Result is null || !createShareLinkRes.Result.IsSuccess)
            {
                return AppResult<OteCreateSharedLinkResult>.CreateFailed(
                    new ApplicationException(createShareLinkRes.Result?.ErrorInfo?.Message), createShareLinkRes.Message);
            }

            return AppResult<OteCreateSharedLinkResult>.CreateSucceeded(new OteCreateSharedLinkResult {
                Guid = guid.ToString(),
                Token = encodedToken
            }, "Successfully create shared link.");
        }
        catch (Exception ex)
        {
            return AppResult<OteCreateSharedLinkResult>.CreateFailed(ex, "An error occured in OteCreateSharedLinkHandler");
        }
    }
}