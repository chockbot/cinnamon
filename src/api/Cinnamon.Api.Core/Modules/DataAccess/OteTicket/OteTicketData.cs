using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.OteTicket;

public class OteTicketData : IOteTicketData
{
    private readonly IFlurlClient flurlClient;

    public OteTicketData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
        flurlClient = flurlFac.Get(config.ApiDataUrl);
    }

    public async Task<AppResult<CreateManyOteTicketsResult>> CreateTickets(CreateManyOteTicketsArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("OteTicket/create-many")
                .PostJsonAsync(args)
                .ReceiveJson<CreateManyOteTicketsResult>();

            return AppResult<CreateManyOteTicketsResult>.CreateSucceeded(result, "Successfully posting create multiple tickets.");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<CreateManyOteTicketsResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<CreateManyOteTicketsResult>.CreateFailed(ex, "An error occured when posting create multiple tickets.");
        }
    }

    public async Task<AppResult<GetByActivityIdResult>> GetByActivityId(int activityId, GetByActivityIdArgs args)
    {
        try
        {
            var result = await flurlClient
                            .Request($"OteTicket/by-activity/{activityId}")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetByActivityIdResult>();

            return AppResult<GetByActivityIdResult>.CreateSucceeded(result, "Successfully getting get all tickets");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetByActivityIdResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetByActivityIdResult>.CreateFailed(ex, "An error occured when getting all tickets");
        }
    }

    public async Task<AppResult<GetByCodeResult>> GetByCode(string code)
    {
        try
        {
            var result = await flurlClient
                            .Request($"OteTicket/by-code/{code}")
                            .GetJsonAsync<GetByCodeResult>();

            return AppResult<GetByCodeResult>.CreateSucceeded(result, "Successfully getting ticket by code");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<GetByCodeResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<GetByCodeResult>.CreateFailed(ex, "An error occured when getting ticket by code");
        }
    }

    public async Task<AppResult<UpdateTicketResult>> UpdateTicket(UpdateTicketArgs args)
    {
        try
        {
            var result = await flurlClient
                .Request("OteTicket/update-ticket-status")
                .PostJsonAsync(args)
                .ReceiveJson<UpdateTicketResult>();

            return AppResult<UpdateTicketResult>.CreateSucceeded(result, "Successfully posting update ticket");
        }
        catch (FlurlHttpException ex)
        {
            return AppResult<UpdateTicketResult>.CreateFailed(ex, ex.Message);
        }
        catch (Exception ex)
        {
            return AppResult<UpdateTicketResult>.CreateFailed(ex, "An error occured when posting update ticket");
        }
    }
}