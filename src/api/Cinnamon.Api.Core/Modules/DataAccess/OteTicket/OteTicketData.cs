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
	
	public async Task<AppResult<GetTicketDetailsResult>> GetTicketDetails(GetTicketDetailsArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request($"OteTicket/GetTicketDetails")
							.SetQueryParams(args)
							.GetJsonAsync<GetTicketDetailsResult>();

			return AppResult<GetTicketDetailsResult>.CreateSucceeded(result, "Successfully getting get all tickets");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetTicketDetailsResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetTicketDetailsResult>.CreateFailed(ex, "An error occured when getting all tickets");
		}
	}

	public async Task<AppResult<GetByPurchaseOrderIdResult>> GetByPurchaseOrderId(int purchaseOrderId, GetByPurchaseOrderIdArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request($"OteTicket/by-purchase-order/{purchaseOrderId}")
							.SetQueryParams(args)
							.GetJsonAsync<GetByPurchaseOrderIdResult>();

			return AppResult<GetByPurchaseOrderIdResult>.CreateSucceeded(result, "Successfully getting get all tickets");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetByPurchaseOrderIdResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetByPurchaseOrderIdResult>.CreateFailed(ex, "An error occured when getting all tickets");
		}
	}

	public async Task<AppResult<CreateSharedLinkResult>> CreateSharedLink(CreateSharedLinkArgs args)
	{
		try
		{
			var result = await flurlClient
				.Request("OteTicket/CreateSharedLink")
				.PostJsonAsync(args)
				.ReceiveJson<CreateSharedLinkResult>();

			return AppResult<CreateSharedLinkResult>.CreateSucceeded(result, "Successfully posting create shared link.");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<CreateSharedLinkResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<CreateSharedLinkResult>.CreateFailed(ex, "An error occured when posting create shared link.");
		}
	}

	public async Task<AppResult<GetSharedLinkResult>> GetSharedLink(GetSharedLinkArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request($"OteTicket/GetSharedLink")
							.SetQueryParams(args)
							.GetJsonAsync<GetSharedLinkResult>();

			return AppResult<GetSharedLinkResult>.CreateSucceeded(result, "Successfully getting get all shared links");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetSharedLinkResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetSharedLinkResult>.CreateFailed(ex, "An error occured when getting all shared links");
		}
	}

	public async Task<AppResult<UpdateSharedLinkStatusResult>> UpdateSharedLinkStatus(UpdateSharedLinkStatusArgs args)
	{
		try
		{
			var result = await flurlClient
				.Request("OteTicket/UpdateSharedLinkStatus")
				.PostJsonAsync(args)
				.ReceiveJson<UpdateSharedLinkStatusResult>();

			return AppResult<UpdateSharedLinkStatusResult>.CreateSucceeded(result, "Successfully posting update shared link status.");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<UpdateSharedLinkStatusResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<UpdateSharedLinkStatusResult>.CreateFailed(ex, "An error occured when posting update shared link status.");
		}
	}

	public async Task<AppResult<CountBookedTicketsResult>> CountBookedTickets(CountBookedTicketsArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request($"OteTicket/CountBookedTickets")
							.SetQueryParams(args)
							.GetJsonAsync<CountBookedTicketsResult>();

			return AppResult<CountBookedTicketsResult>.CreateSucceeded(result, "Successfully get ticket booked count.");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<CountBookedTicketsResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<CountBookedTicketsResult>.CreateFailed(ex, "An error occured when getting ticket booked count.");
		}
	}

	public async Task<AppResult<BookedCustomersResult>> BookedCustomers(BookedCustomersArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request($"OteTicket/BookedCustomers")
							.SetQueryParams(args)
							.GetJsonAsync<BookedCustomersResult>();

			return AppResult<BookedCustomersResult>.CreateSucceeded(result, "Successfully get booked customers.");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<BookedCustomersResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<BookedCustomersResult>.CreateFailed(ex, "An error occured when getting booked customers.");
		}
	}
}