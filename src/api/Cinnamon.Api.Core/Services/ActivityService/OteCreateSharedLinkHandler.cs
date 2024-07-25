using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OteCreateSharedLinkHandler : IOteCreateSharedLinkHandler
{
    private readonly IOteTicketData oteTicketData;
    private readonly ITokenGeneratorProvider tokenGeneratorProvider;

    public OteCreateSharedLinkHandler(IOteTicketData oteTicketData, ITokenGeneratorProvider tokenGeneratorProvider)
    {
        this.oteTicketData = oteTicketData;
        this.tokenGeneratorProvider = tokenGeneratorProvider;
    }

    public AppResult<OteCreateSharedLinkResult> Execute(OteCreateSharedLinkArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OteCreateSharedLinkResult>> ExecuteAsync(OteCreateSharedLinkArgs args)
    {
        try
        {
            // generate token and guid
            var tokenGenerated = tokenGeneratorProvider.Generator();

            var createShareLinkRes = await oteTicketData.CreateSharedLink(new Framework.ApiCommand.ApiData.OteTicket.Request.CreateSharedLinkArgs {
                ActivityId = args.ActivityId,
                Guid = tokenGenerated.Guid,
                OteDateId = args.OteDateId,
                Token = tokenGenerated.Token
            });
            if(!createShareLinkRes.Succeeded || createShareLinkRes.Result is null || !createShareLinkRes.Result.IsSuccess)
            {
                return AppResult<OteCreateSharedLinkResult>.CreateFailed(
                    new ApplicationException(createShareLinkRes.Result?.ErrorInfo?.Message), createShareLinkRes.Message);
            }

            return AppResult<OteCreateSharedLinkResult>.CreateSucceeded(new OteCreateSharedLinkResult {
                Guid = tokenGenerated.Guid,
                Token = tokenGenerated.Token
            }, "Successfully create shared link.");
        }
        catch (Exception ex)
        {
            return AppResult<OteCreateSharedLinkResult>.CreateFailed(ex, "An error occured in OteCreateSharedLinkHandler");
        }
    }
}