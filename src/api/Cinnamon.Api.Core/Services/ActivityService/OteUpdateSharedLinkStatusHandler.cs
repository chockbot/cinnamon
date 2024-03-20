using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OteUpdateSharedLinkStatusHandler : IOteUpdateSharedLinkStatusHandler
{
    private readonly IOteTicketData oteTicketData;

    public OteUpdateSharedLinkStatusHandler(IOteTicketData oteTicketData)
    {
        this.oteTicketData = oteTicketData;
    }
    
    public AppResult<OteUpdateSharedLinkResult> Execute(OteUpdateSharedLinkStatusArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OteUpdateSharedLinkResult>> ExecuteAsync(OteUpdateSharedLinkStatusArgs args)
    {
        try
        {
            var sharedLinkRes = await oteTicketData.GetSharedLink(new Framework.ApiCommand.ApiData.OteTicket.Request.GetSharedLinkArgs {
                Guid = args.Guid,
                Token = args.Token
            });
            if(!sharedLinkRes.Succeeded || sharedLinkRes.Result is null || !sharedLinkRes.Result.IsSuccess)
            {
                return AppResult<OteUpdateSharedLinkResult>.CreateFailed(
                    new ApplicationException(sharedLinkRes.Result?.ErrorInfo?.Message), sharedLinkRes.Message);
            }

            var sharedLink = sharedLinkRes.Result.Result.FirstOrDefault();
            if(sharedLink is null)
            {
                return AppResult<OteUpdateSharedLinkResult>.CreateFailed(
                    new ApplicationException("Invalid guid and token. Invalid request."), "Invalid guid and token. Invalid request.");
            }

            var updateSharedLinkStatusRes = await oteTicketData.UpdateSharedLinkStatus(new Framework.ApiCommand.ApiData.OteTicket.Request.UpdateSharedLinkStatusArgs {
                Id = sharedLink.Id,
                Status = args.Enable
            });
            if(!updateSharedLinkStatusRes.Succeeded || updateSharedLinkStatusRes.Result is null || !updateSharedLinkStatusRes.Result.IsSuccess)
            {
                return AppResult<OteUpdateSharedLinkResult>.CreateFailed(
                    new ApplicationException(updateSharedLinkStatusRes.Result?.ErrorInfo?.Message), updateSharedLinkStatusRes.Message);
            }
            
            return AppResult<OteUpdateSharedLinkResult>.CreateSucceeded(new OteUpdateSharedLinkResult {}, "Successfully update shared link status.");
        }
        catch (Exception ex)
        {
            return AppResult<OteUpdateSharedLinkResult>.CreateFailed(ex, "An error occured in OteUpdateSharedLinkStatusHandler.");
        }
    }
}