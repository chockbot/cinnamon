using Cinnamon.Framework.ApiCommand.ApiCore.SeatPlan.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.SeatPlan.Response;
using Cinnamon.Framework.Common;
using Cinnamon.Web.Modules.ApiAccess.Handlers;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Web.Modules.ApiAccess.SeatPlan;

public class SeatPlanApiHandler : ISeatPlanApiHandler
{
    private readonly IFlurlClient flurlClient;

    public SeatPlanApiHandler(IFlurlClientFactory flurlFac, Config.Config config)
    {
        flurlClient = flurlFac.Get(config.ApiUrl);
    }

    public async Task<AppResult<CreateSeatPlanTemplateResult>> CreateSeatPlanTemplate(CreateSeatPlanTemplateArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("SeatPlan")
                .PostJsonAsync(args)
                .ReceiveJson<CreateSeatPlanTemplateResult>();

            return AppResult<CreateSeatPlanTemplateResult>.CreateSucceeded(result, "Seat plan template created successfully");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateSeatPlanTemplateResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateSeatPlanTemplateResult>.CreateFailed(ex, "An error occurred while creating seat plan template");
        }
    }
}