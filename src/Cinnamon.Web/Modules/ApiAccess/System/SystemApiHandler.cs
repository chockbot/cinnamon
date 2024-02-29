using Cinnamon.Framework.ApiCommand.ApiCore.System.Response;
using Cinnamon.Framework.Common;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;
using NuGet.Common;

namespace Cinnamon.Web.Modules.ApiAccess.System;

public class SystemApiHandler: ISystemApiHandler
{
    private readonly IFlurlClient flurlClient;

	public SystemApiHandler(IFlurlClientFactory flurlFac, Config.Config config)
	{
        flurlClient = flurlFac.Get(config.ApiUrl);
    }

    public async Task<AppResult<GetServerDateResult>> GetServerDate()
    {
        try
        {
            var result = await flurlClient
                .Request("System/GetServerDate")
                .GetJsonAsync<GetServerDateResult>();

            return AppResult<GetServerDateResult>.CreateSucceeded(result, "Successfully getting server date api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetServerDateResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetServerDateResult>.CreateFailed(ex, "An error occured when getting server date api");
        }
    }

    public async Task<AppResult<GetAnnouncementsResult>> GetAnnouncements()
    {
        try
        {
            var result = await flurlClient
                .Request("System/GetAnnouncements")
                .GetJsonAsync<GetAnnouncementsResult>();

            return AppResult<GetAnnouncementsResult>.CreateSucceeded(result, "Successfully get announcement api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAnnouncementsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAnnouncementsResult>.CreateFailed(ex, "An error occured when get announcement api");
        }
    }
}
