using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.BadgeList.Request;
using Cinnamon.Framework.ApiCommand.ApiData.BadgeList.Response;
using Cinnamon.Framework.Common;
using Flurl;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.Badges;

public class BadgesData : IBadgesData
{
    private readonly IFlurlClient flurlClient;
	public BadgesData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateBadgeResult>> CreateBadge(CreateBadgeArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Badge/CreateBadge")
                            .PostJsonAsync(args)
                            .ReceiveJson<CreateBadgeResult>();

            return AppResult<CreateBadgeResult>.CreateSucceeded(result, "Successfully posting create badge api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateBadgeResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateBadgeResult>.CreateFailed(ex, "An error occured when posting create badge api");
        }
    }

    public async Task<AppResult<GetAllBadgeResult>> GetAllBadges(GetAllBadgeArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Badge/GetAllBadge")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetAllBadgeResult>();

            return AppResult<GetAllBadgeResult>.CreateSucceeded(result, "Successfully getting get all badge api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllBadgeResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllBadgeResult>.CreateFailed(ex, "An error occured when getting all badge api");
        }
    }

    public async Task<AppResult<GetBadgeResult>> GetBadgeById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"Badge/GetBadgeById/{id}")
                            .GetJsonAsync<GetBadgeResult>();

            return AppResult<GetBadgeResult>.CreateSucceeded(result, "Successfully getting badge by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetBadgeResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetBadgeResult>.CreateFailed(ex, "An error occured when getting badge by id api");
        }
    }

    public async Task<AppResult<UpdateBadgeResult>> UpdateBadge(UpdateBadgeArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("Badge/UpdateBadge")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateBadgeResult>();

            return AppResult<UpdateBadgeResult>.CreateSucceeded(result, "Successfully posting update category api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateBadgeResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateBadgeResult>.CreateFailed(ex, "An error occured when posting update category api");
        }
    }
}
