using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OteValidateSharedLinkHandler : IOteValidateSharedLinkHandler
{
    private readonly IOteTicketData oteTicketData;
    private readonly IGetActivityHandler getActivityHandler;

    public OteValidateSharedLinkHandler(IOteTicketData oteTicketData, IGetActivityHandler getActivityHandler)
    {
        this.oteTicketData = oteTicketData;
        this.getActivityHandler = getActivityHandler;
    }

    public AppResult<OteValidateSharedLinkResult> Execute(OteValidateSharedLinkArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OteValidateSharedLinkResult>> ExecuteAsync(OteValidateSharedLinkArgs args)
    {
        try
        {
            var sharedLinkRes = await oteTicketData.GetSharedLink(new Framework.ApiCommand.ApiData.OteTicket.Request.GetSharedLinkArgs {
                Guid = args.Guid,
                Token = args.Token
            });
            if(!sharedLinkRes.Succeeded || sharedLinkRes.Result is null || !sharedLinkRes.Result.IsSuccess)
            {
                return AppResult<OteValidateSharedLinkResult>.CreateFailed(
                    new ApplicationException(sharedLinkRes.Result?.ErrorInfo?.Message), sharedLinkRes.Message);
            }

            var sharedLink = sharedLinkRes.Result.Result.FirstOrDefault();
            if(sharedLink is null)
            {
                return AppResult<OteValidateSharedLinkResult>.CreateFailed(
                    new ApplicationException("Invalid guid and token. Invalid request."), "Invalid guid and token. Invalid request.");
            }

            var activityRes = await getActivityHandler.ExecuteAsync(new GetActivityArgs {
                ActivityId = sharedLink.ActivityId,
                IsActive = true
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<OteValidateSharedLinkResult>.CreateFailed(
                    new ApplicationException("Invalid guid and token. Invalid request."), "Invalid guid and token. Invalid request.");
            }
            var activity = activityRes.Result;

            if(activity.ForceDisable) 
            {
                return AppResult<OteValidateSharedLinkResult>.CreateFailed(
                    new ApplicationException("Event already expired."), "Event already expired.");
            }

            if(!sharedLink.Enable)
            {
                return AppResult<OteValidateSharedLinkResult>.CreateFailed(
                    new ApplicationException("Shared link expired or disabled."), "Shared link expired or disabled.");
            }

            return AppResult<OteValidateSharedLinkResult>.CreateSucceeded(new OteValidateSharedLinkResult {
                EventTitle = activity.Title,
                Id = activity.Id
            }, "Shared link successfully validated.");
        }
        catch (Exception ex)
        {
            return AppResult<OteValidateSharedLinkResult>.CreateFailed(ex, "An error occured in OteValidateSharedLinkHandler.");
        }
    }
}