using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.OngoingActivity.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OngoingActivity.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.OngoingActivities;

public class OngoingActivitiesData: IOngoingActivitiesData
{
    private readonly IFlurlClient flurlClient;

	public OngoingActivitiesData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateOngoingActivityResult>> CreateOngoingActivity(CreateOngoingActivityArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("OngoingActivity/CreateOngoingActivity")
                .PostJsonAsync(args)
                .ReceiveJson<CreateOngoingActivityResult>();

            return AppResult<CreateOngoingActivityResult>.CreateSucceeded(result, "Successfully posting create ongoing activity api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateOngoingActivityResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateOngoingActivityResult>.CreateFailed(ex, "An error occured when posting create ongoing activity api");
        }
    }
    public async Task<AppResult<GetOngoingActivityResult>> GetOngoingActivityById(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"OngoingActivity/GetOngoingActivityById/{id}")
                            .GetJsonAsync<GetOngoingActivityResult>();

            return AppResult<GetOngoingActivityResult>.CreateSucceeded(result, "Successfully getting activity ongoing activity by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetOngoingActivityResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetOngoingActivityResult>.CreateFailed(ex, "An error occured when getting activity ongoing activity by id api");
        }
    }
    public async Task<AppResult<GetAllOngoingActivityResult>> GetAllOngoingActivities(GetAllOngoingActivityArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("OngoingActivity/GetAllOngoingActivities")
                            .SetQueryParams(
                                new
                                {
                                    countPerPage = args.CountPerPage,
                                    pageIndex = args.PageIndex
                                }).GetJsonAsync<GetAllOngoingActivityResult>();

            return AppResult<GetAllOngoingActivityResult>.CreateSucceeded(result, "Successfully getting get all ongoing activities api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetAllOngoingActivityResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetAllOngoingActivityResult>.CreateFailed(ex, "An error occured when getting all ongoing activities api");
        }
    }
    public async Task<AppResult<UpdateongoingActivityResult>> UpdateOngoingActivity(UpdateOngoingActivityArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request("OngoingActivity/UpdateOngoingActivity")
                            .PostJsonAsync(args)
                            .ReceiveJson<UpdateongoingActivityResult>();

            return AppResult<UpdateongoingActivityResult>.CreateSucceeded(result, "Successfully posting update ongoing activity api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateongoingActivityResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateongoingActivityResult>.CreateFailed(ex, "An error occured when posting update ongoing activity api");
        }
    }
}
