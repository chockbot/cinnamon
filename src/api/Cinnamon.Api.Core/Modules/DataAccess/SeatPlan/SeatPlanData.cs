using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.SeatPlan.Request;
using Cinnamon.Framework.ApiCommand.ApiData.SeatPlan.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.SeatPlan;

public class SeatPlanData : ISeatPlanData
{
    private readonly IFlurlClient flurlClient;

    public SeatPlanData(ApplicationConfig config, IFlurlClientFactory flurlFac)
    {
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<GetSeatPlanTemplateResult>> GetSeatPlanTemplateAsync(GetSeatPlanTemplateArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request($"SeatPlan/templates")
							.SetQueryParams(args)
							.GetJsonAsync<GetSeatPlanTemplateResult>();

			return AppResult<GetSeatPlanTemplateResult>.CreateSucceeded(result, "Successfully getting seat plan template api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetSeatPlanTemplateResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetSeatPlanTemplateResult>.CreateFailed(ex, "An error occured when getting seat plan template api");
		}
	}

    public async Task<AppResult<GetSeatPlanTemplateByIdResult>> GetSeatPlanTemplateByIdAsync(int id)
    {
        try
        {
            var result = await flurlClient
                            .Request($"SeatPlan/templates/{id}")
                            .GetJsonAsync<GetSeatPlanTemplateByIdResult>();

            return AppResult<GetSeatPlanTemplateByIdResult>.CreateSucceeded(result, "Successfully getting seat plan template by id api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetSeatPlanTemplateByIdResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetSeatPlanTemplateByIdResult>.CreateFailed(ex, "An error occured when getting seat plan template by id api");
        }
    }

    public async Task<AppResult<GetSeatPlanFormatterResult>> GetSeatPlanFormatterAsync()
    {
        try
        {
            var result = await flurlClient
                            .Request($"SeatPlan/formatters")
                            .GetJsonAsync<GetSeatPlanFormatterResult>();

            return AppResult<GetSeatPlanFormatterResult>.CreateSucceeded(result, "Successfully getting seat plan formatter api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetSeatPlanFormatterResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetSeatPlanFormatterResult>.CreateFailed(ex, "An error occured when getting seat plan formatter api");
        }
    }

    public async Task<AppResult<CreateSeatPlanTemplateResult>> CreateSeatPlanTemplateAsync(CreateSeatPlanTemplateArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("SeatPlan/templates")
                .PostJsonAsync(args)
                .ReceiveJson<CreateSeatPlanTemplateResult>();

            return AppResult<CreateSeatPlanTemplateResult>.CreateSucceeded(result, "Successfully posting create seat plan template api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateSeatPlanTemplateResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateSeatPlanTemplateResult>.CreateFailed(ex, "An error occured when posting create seat plan template api");
        }
    }

    public async Task<AppResult<UpdateSeatPlanTemplateResult>> UpdateSeatPlanTemplateAsync(UpdateSeatPlanTemplateArgs args, int id)
    {
        try
        {
            var result = await flurlClient
                .Request($"SeatPlan/templates/{id}")
                .PutJsonAsync(args)
                .ReceiveJson<UpdateSeatPlanTemplateResult>();

            return AppResult<UpdateSeatPlanTemplateResult>.CreateSucceeded(result, "Successfully posting update seat plan template api");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateSeatPlanTemplateResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateSeatPlanTemplateResult>.CreateFailed(ex, "An error occured when posting update seat plan template api");
        }
    }
}