using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;
using Flurl;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GenerateEventSharedLinkHandler : IGenerateEventSharedLinkHandler
{
    private readonly IOteTicketData oteTicketData;
    private readonly IOteCreateSharedLinkHandler oteCreateSharedLink;
    private readonly IOteFindByHandler oteFindByHandler;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly ApplicationConfig applicationConfig;

    public GenerateEventSharedLinkHandler(IOteTicketData oteTicketData, IOteCreateSharedLinkHandler oteCreateSharedLink,
        IOteFindByHandler oteFindByHandler, IGetProfileHandler getProfileHandler, ApplicationConfig applicationConfig)
    {
        this.oteTicketData = oteTicketData;
        this.oteCreateSharedLink = oteCreateSharedLink;
        this.oteFindByHandler = oteFindByHandler;
        this.getProfileHandler = getProfileHandler;
        this.applicationConfig = applicationConfig;
    }

    public AppResult<GenerateEventSharedLinkResult> Execute(GenerateEventSharedLinkArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<GenerateEventSharedLinkResult>> ExecuteAsync(GenerateEventSharedLinkArgs args)
    {
        try
        {
            var profileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!profileRes.Succeeded || profileRes.Result is null)
            {
                return AppResult<GenerateEventSharedLinkResult>.CreateFailed(new ApplicationException(profileRes.Message), profileRes.Message);
            }
            var profile = profileRes.Result;

            var getOteRes = await oteFindByHandler.ExecuteAsync(new OteFindByHandlerArgs {
                Handler = args.Handler,
                IncludeSchedule = true,
                IncludePricing = true
            });
            if(!getOteRes.Succeeded || getOteRes.Result is null)
            {
                return AppResult<GenerateEventSharedLinkResult>.CreateFailed(new ApplicationException(getOteRes.Message), getOteRes.Message);
            }
            var oteActivity =  getOteRes.Result;

            if(oteActivity.ProviderId != profile.Id)
            {
                return AppResult<GenerateEventSharedLinkResult>.CreateFailed(new ApplicationException("Invalid request. Action not allowed."), "Invalid request. Action not allowed.");
            }

            if(!oteActivity.OteDates.Any(d => d.Id == args.DateId))
            {
                return AppResult<GenerateEventSharedLinkResult>.CreateFailed(new ApplicationException("Invalid request. Action not allowed."), "Invalid request. Action not allowed.");
            }

            var sharedLinksRes = await oteTicketData.GetSharedLink(new Framework.ApiCommand.ApiData.OteTicket.Request.GetSharedLinkArgs {
                ActivityId = oteActivity.Id,
                OteDateId = args.DateId
            });
            if(!sharedLinksRes.Succeeded || sharedLinksRes.Result is null || !sharedLinksRes.Result.IsSuccess)
            {
                return AppResult<GenerateEventSharedLinkResult>.CreateFailed(new ApplicationException(sharedLinksRes.Result?.ErrorInfo?.Message), sharedLinksRes.Message);
            }
            var sharedLinks = sharedLinksRes.Result.Result;

            if(sharedLinks.Count() > 0)
            {
                var defaultLink = sharedLinks.First();
                var generatedLink = applicationConfig.FrontendUrl
                                .AppendPathSegment("shared/event-scanner")
                                .SetQueryParam("Guid", defaultLink.Guid)
                                .SetQueryParam("Token", defaultLink.Token);

                return AppResult<GenerateEventSharedLinkResult>.CreateSucceeded(new GenerateEventSharedLinkResult {
                    GeneratedLink = generatedLink,
                    Enable = defaultLink.Enable,
                    Guid = defaultLink.Guid,
                    Token = defaultLink.Token
                }, "Successfully generate event shared link.");
            }

            var createEventSharedLinkRes = await oteCreateSharedLink.ExecuteAsync(new OteCreateSharedLinkArgs {
                ActivityId = oteActivity.Id,
                OteDateId = args.DateId
            });
            if(!createEventSharedLinkRes.Succeeded || createEventSharedLinkRes.Result is null)
            {
                return AppResult<GenerateEventSharedLinkResult>.CreateFailed(new ApplicationException(createEventSharedLinkRes.Message), createEventSharedLinkRes.Message);
            }
            var createdSharedLink = createEventSharedLinkRes.Result;

            var link = applicationConfig.FrontendUrl
                                .AppendPathSegment("shared/event-scanner")
                                .SetQueryParam("Guid", createdSharedLink.Guid)
                                .SetQueryParam("Token", createdSharedLink.Token);

            return AppResult<GenerateEventSharedLinkResult>.CreateSucceeded(new GenerateEventSharedLinkResult {
                GeneratedLink = link,
                Enable = true,
                Guid = createdSharedLink.Guid,
                Token = createdSharedLink.Token
            }, "Successfully generate event shared link.");
        }
        catch (Exception ex)
        {
            return AppResult<GenerateEventSharedLinkResult>.CreateFailed(ex, "An error occured in GenerateEventSharedLinkHandler.");
        }
    }
}