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

    public async Task<AppResult<GetTemplatesResult>> GetTemplates(GetTemplatesArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("SeatPlan")
                .SetQueryParams(args)
                .GetJsonAsync<GetTemplatesResult>();

            return AppResult<GetTemplatesResult>.CreateSucceeded(result, "Successfully retrieved templates");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetTemplatesResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetTemplatesResult>.CreateFailed(ex, "An error occurred while retrieving templates");
        }
    }

    public async Task<AppResult<CreateSeatPlanTemplateResult>> CreateSeatPlanTemplate(CreateSeatPlanTemplateArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("SeatPlan")
                .PostMultipartAsync(mp => {
                    mp.AddString("Name", args.Name);
                    mp.AddString("Address", args.Address);
                    mp.AddString("FormatterId", args.FormatterId.ToString());
                    mp.AddFile("ImageFile", args.ImageFile.OpenReadStream(), args.ImageFile.FileName, args.ImageFile.ContentType);
                    mp.AddFile("JsonFile", args.JsonFile.OpenReadStream(), args.JsonFile.FileName, args.JsonFile.ContentType);
                })
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

    public async Task<AppResult<GetTemplateResult>> GetTemplate(int templateId, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"SeatPlan/{templateId}")
                .GetJsonAsync<GetTemplateResult>();

            return AppResult<GetTemplateResult>.CreateSucceeded(result, "Successfully retrieved seat plan template");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetTemplateResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetTemplateResult>.CreateFailed(ex, "An error occurred while retrieving seat plan template");
        }
    }

    public async Task<AppResult<ChangeSeatPlanStatusResult>> ChangeSeatPlanStatus(int id, ChangeSeatPlanStatusArgs status, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request($"SeatPlan/{id}/Status")
                .PutJsonAsync(status)
                .ReceiveJson<ChangeSeatPlanStatusResult>();

            return AppResult<ChangeSeatPlanStatusResult>.CreateSucceeded(result, "Seat plan status changed successfully");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<ChangeSeatPlanStatusResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<ChangeSeatPlanStatusResult>.CreateFailed(ex, "An error occurred while changing seat plan status");
        }
    }

    public async Task<AppResult<UpdateSeatStatusResult>> UpdateSeatStatus(UpdateSeatStatusArgs args, string token)
    {
        try
        {
            var result = await flurlClient
                .WithOAuthBearerToken(token)
                .Request("SeatPlan/UpdateSeatStatus")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateSeatStatusResult>();

            return AppResult<UpdateSeatStatusResult>.CreateSucceeded(result, "Seat  status changed successfully");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateSeatStatusResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateSeatStatusResult>.CreateFailed(ex, "An error occurred while changing seat status");
        }
    }
}