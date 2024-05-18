using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.OteDate.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OteDate.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.OteDate;

public class OTeDateData : IOteDateData
{
    private readonly IFlurlClient flurlClient;

	public OTeDateData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
		flurlClient = flurlFac.Get(config.ApiDataUrl);
	}

    public async Task<AppResult<GetOteDateByIdResult>> GetOteDate(int dateId)
    {
        try
		{
			var result = await flurlClient
							.Request($"OteDate/GetDate/{dateId}")
							.GetJsonAsync<GetOteDateByIdResult>();

			return AppResult<GetOteDateByIdResult>.CreateSucceeded(result, "Successfully getting ote date");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetOteDateByIdResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetOteDateByIdResult>.CreateFailed(ex, "An error occured when getting ote date");
		}
    }

	public async Task<AppResult<GetOteDateResult>> GetOteDate(GetOteDateArgs args)
    {
        try
		{
			var result = await flurlClient
							.Request($"OteDate/GetDate")
							.SetQueryParams(args)
							.GetJsonAsync<GetOteDateResult>();

			return AppResult<GetOteDateResult>.CreateSucceeded(result, "Successfully getting ote dates.");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetOteDateResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetOteDateResult>.CreateFailed(ex, "An error occured when getting ote dates.");
		}
    }
}