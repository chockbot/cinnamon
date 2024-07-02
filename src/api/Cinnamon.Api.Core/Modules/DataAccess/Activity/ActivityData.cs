using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Response;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Response;
using Cinnamon.Framework.ApiCommand.ApiData.AddOns.Request;
using Cinnamon.Framework.ApiCommand.ApiData.AddOns.Response;
using Cinnamon.Framework.ApiCommand.ApiData.OnlineEvent.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OnlineEvent.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;
using Cinnamon.Framework.ApiCommand.ApiData.OteWaitlist.Response;
using Cinnamon.Framework.ApiCommand.ApiData.OteWaitlist.Request;

namespace Cinnamon.Api.Core.Modules.DataAccess.Activity;

public class ActivityData: IActivityData
{
	private readonly IFlurlClient flurlClient;
	public ActivityData(ApplicationConfig config, IFlurlClientFactory flurlFac)
	{
		flurlClient = flurlFac.Get(config.ApiDataUrl);
	}

	public async Task<AppResult<CreatedActivityResult>> CreateActivity(CreateActivityArgs args)
	{
		try
		{
			var result = await flurlClient
				.Request("Activity/CreateActivity")
				.PostJsonAsync(args)
				.ReceiveJson<CreatedActivityResult>();

			return AppResult<CreatedActivityResult>.CreateSucceeded(result, "Successfully posting create activity api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<CreatedActivityResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<CreatedActivityResult>.CreateFailed(ex, "An error occured when posting create activity api");
		}
	}

	public async Task<AppResult<GetActivitiesByCategoriesResult>> GetActivitiesByCategories(int id, GetActivityArgs? args = null)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/GetActivitiesByCategories/{id}")
							.SetQueryParams(args)
							.GetJsonAsync<GetActivitiesByCategoriesResult>();

			return AppResult<GetActivitiesByCategoriesResult>.CreateSucceeded(result, "Successfully getting activity by id api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetActivitiesByCategoriesResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetActivitiesByCategoriesResult>.CreateFailed(ex, "An error occured when getting activity by id api");
		}
	}

	public async Task<AppResult<GetActivitiesBySubCategoriesResult>> GetActivitiesBySubCategories(int id, GetActivityArgs? args = null)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/GetActivitiesBySubCategories/{id}")
							.SetQueryParams(args)
							.GetJsonAsync<GetActivitiesBySubCategoriesResult>();

			return AppResult<GetActivitiesBySubCategoriesResult>.CreateSucceeded(result, "Successfully getting activity by id api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetActivitiesBySubCategoriesResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetActivitiesBySubCategoriesResult>.CreateFailed(ex, "An error occured when getting activity by id api");
		}
	}

	public async Task<AppResult<GetActivityResult>> GetActivityById(int id, GetActivityArgs? args = null)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/GetActivityById/{id}")
							.SetQueryParams(args)
							.GetJsonAsync<GetActivityResult>();

			return AppResult<GetActivityResult>.CreateSucceeded(result, "Successfully getting activity by id api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetActivityResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetActivityResult>.CreateFailed(ex, "An error occured when getting activity by id api");
		}
	}

	public async Task<AppResult<GetActivityResult>> GetActivityByHandler(string handler, GetActivityArgs? args = null)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/GetActivityByHandler/{handler}")
							.SetQueryParams(args)
							.GetJsonAsync<GetActivityResult>();

			return AppResult<GetActivityResult>.CreateSucceeded(result, "Successfully getting activity by id api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetActivityResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetActivityResult>.CreateFailed(ex, "An error occured when getting activity by id api");
		}
	}

	public async Task<AppResult<GetAllActivitiesResult>> GetAllActivities(GetAllActivities args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/GetAllActivities")
							.SetQueryParams(args)
							.GetJsonAsync<GetAllActivitiesResult>();

			return AppResult<GetAllActivitiesResult>.CreateSucceeded(result, "Successfully getting get all activities api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetAllActivitiesResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetAllActivitiesResult>.CreateFailed(ex, "An error occured when getting all activities api");
		}
	}

	public async Task<AppResult<UpdatedActivityResult>> UpdateActivity(UpdateActivity args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/UpdateActivity")
							.PostJsonAsync(args)
							.ReceiveJson<UpdatedActivityResult>();

			return AppResult<UpdatedActivityResult>.CreateSucceeded(result, "Successfully posting update activity api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<UpdatedActivityResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<UpdatedActivityResult>.CreateFailed(ex, "An error occured when posting update activity api");
		}
	}

	public async Task<AppResult<GetAllActivitiesResult>> GetPopularActivities(GetAllActivities args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/Popular")
							.SetQueryParams(args)
							.GetJsonAsync<GetAllActivitiesResult>();

			return AppResult<GetAllActivitiesResult>.CreateSucceeded(result, "Successfully getting get all activities api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetAllActivitiesResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetAllActivitiesResult>.CreateFailed(ex, "An error occured when getting all activities api");
		}
	}

	public async Task<AppResult<UpdatedActivityResult>> UpdateActivityGuid(UpdateActivity args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/Guid/Update")
							.PostJsonAsync(args)
							.ReceiveJson<UpdatedActivityResult>();

			return AppResult<UpdatedActivityResult>.CreateSucceeded(result, "Successfully updated activity guids");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<UpdatedActivityResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<UpdatedActivityResult>.CreateFailed(ex, "An error occured when updating activity guids");
		}
	}

	public async Task<AppResult<DeleteActivityResult>> DeleteActivityById(DeleteActivityArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/Remove")
							.PostJsonAsync(args)
							.ReceiveJson<DeleteActivityResult>();

			return AppResult<DeleteActivityResult>.CreateSucceeded(result, "Successfully deleted activity");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<DeleteActivityResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<DeleteActivityResult>.CreateFailed(ex, "An error occured when deleting activity");
		}
	}

	public async Task<AppResult<RecommendedActivitiesResult>> RecommendedActivities(int primaryId, int count)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/RecommendedActivities/{primaryId}/{count}")
							.GetJsonAsync<RecommendedActivitiesResult>();

			return AppResult<RecommendedActivitiesResult>.CreateSucceeded(result, "Successfully getting recommended activities api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<RecommendedActivitiesResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<RecommendedActivitiesResult>.CreateFailed(ex, "An error occured when getting recommended activities api");
		}
	}

	public async Task<AppResult<PopularActivitiesResult>> PopularActivities(PopularActivitiesArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/PopularActivities")
							.SetQueryParams(args)
							.GetJsonAsync<PopularActivitiesResult>();

			return AppResult<PopularActivitiesResult>.CreateSucceeded(result, "Successfully getting popular activities");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<PopularActivitiesResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<PopularActivitiesResult>.CreateFailed(ex, "An error occured when getting popular activities");
		}
	}

	public async Task<AppResult<CreateOteActivityResult>> CreateOteActivity(CreateOteActivityArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/CreateOteActivity")
							.PostJsonAsync(args)
							.ReceiveJson<CreateOteActivityResult>();

			return AppResult<CreateOteActivityResult>.CreateSucceeded(result, "One time event activity successfully created.");
		}
		catch (FlurlHttpException ex)
		{
			var error = ex.GetResponseJsonAsync();
			return AppResult<CreateOteActivityResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<CreateOteActivityResult>.CreateFailed(ex, "An error occured when creating One time event activity");
		}
	}

	public async Task<AppResult<UpdateOteActivityResult>> UpdateOteActivity(UpdateOteActivityArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/UpdateOteActivity")
							.PostJsonAsync(args)
							.ReceiveJson<UpdateOteActivityResult>();

			return AppResult<UpdateOteActivityResult>.CreateSucceeded(result, "One time event activity successfully created.");
		}
		catch (FlurlHttpException ex)
		{
			var error = ex.GetResponseJsonAsync();
			return AppResult<UpdateOteActivityResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<UpdateOteActivityResult>.CreateFailed(ex, "An error occured when creating One time event activity");
		}
	}

	public async Task<AppResult<GetOteActivityByHandlerResult>> GetOteActivityByHandler(GetOteActivityArgs args, string handler)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/OteActivity/{handler}")
							.SetQueryParams(args)
							.GetJsonAsync<GetOteActivityByHandlerResult>();

			return AppResult<GetOteActivityByHandlerResult>.CreateSucceeded(result, "One time event activity successfully get.");
		}
		catch (FlurlHttpException ex)
		{
			var error = ex.GetResponseJsonAsync();
			return AppResult<GetOteActivityByHandlerResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetOteActivityByHandlerResult>.CreateFailed(ex, "An error occured when getting One time event activity");
		}
	}

	public async Task<AppResult<GetOTEByProvideResult>> GetOTEByProvider(GetOTEByProvideArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/GetOTEByProvider")
							.SetQueryParams(args)
							.GetJsonAsync<GetOTEByProvideResult>();

			return AppResult<GetOTEByProvideResult>.CreateSucceeded(result, "One time event activity successfully get.");
		}
		catch (FlurlHttpException ex)
		{
			var error = ex.GetResponseJsonAsync();
			return AppResult<GetOTEByProvideResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetOTEByProvideResult>.CreateFailed(ex, "An error occured when getting One time event activity");
		}
	}

	public async Task<AppResult<AddTicketSoldResult>> AddTicketSolds(AddTicketSoldArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/ote/add-ticket-solds")
							.PostJsonAsync(args)
							.ReceiveJson<AddTicketSoldResult>();

			return AppResult<AddTicketSoldResult>.CreateSucceeded(result, "One time event ticket sold successfully updated");
		}
		catch (FlurlHttpException ex)
		{
			var error = ex.GetResponseJsonAsync();
			return AppResult<AddTicketSoldResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<AddTicketSoldResult>.CreateFailed(ex, "An error occured when updating one time event ticket sold");
		}
	}

	public async Task<AppResult<CustomerOteResult>> CustomerOte(int customerId)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/customer-ote/{customerId}")
							.GetJsonAsync<CustomerOteResult>();

			return AppResult<CustomerOteResult>.CreateSucceeded(result, "Successfully get customer ote.");
		}
		catch (FlurlHttpException ex)
		{
			var error = ex.GetResponseJsonAsync();
			return AppResult<CustomerOteResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<CustomerOteResult>.CreateFailed(ex, "An error occured when getting customer ote.");
		}
	}

	public async Task<AppResult<DeleteAddOnsResult>> DeleteAddOns(DeleteAddOnsArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("AddOn/DeleteManyAddOns")
							.PostJsonAsync(args)
							.ReceiveJson<DeleteAddOnsResult>();

			return AppResult<DeleteAddOnsResult>.CreateSucceeded(result, "Successfully deleted add-ons");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<DeleteAddOnsResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<DeleteAddOnsResult>.CreateFailed(ex, "An error occurred when deleting add-ons");
		}
	}

	public async Task<AppResult<DeleteAddOnResult>> DeleteAddOn(DeleteAddOnArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("AddOn/DeleteAddOn")
							.PostJsonAsync(args)
							.ReceiveJson<DeleteAddOnResult>();

			return AppResult<DeleteAddOnResult>.CreateSucceeded(result, "Successfully deleted add-on");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<DeleteAddOnResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<DeleteAddOnResult>.CreateFailed(ex, "An error occurred when deleting add-on");
		}
	}
	public async Task <AppResult<DeleteOnlineEventResult>> DeleteOnlineEvent(DeleteOnlineEventArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("OnlineEvent/DeleteOnlineEvent")
							.PostJsonAsync(args)
							.ReceiveJson<DeleteOnlineEventResult>();

			return AppResult<DeleteOnlineEventResult>.CreateSucceeded(result, "Successfully deleted online event");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<DeleteOnlineEventResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<DeleteOnlineEventResult>.CreateFailed(ex, "An error occurred when deleting online event");
		}
	}
	public async Task<AppResult<OtePerDateResult>> OtePerDate(OtePerDateArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/OtePerDate")
							.SetQueryParams(args)
							.GetJsonAsync<OtePerDateResult>();

			return AppResult<OtePerDateResult>.CreateSucceeded(result, "Successfully get customer ote per day.");
		}
		catch (FlurlHttpException ex)
		{
			var error = ex.GetResponseJsonAsync();
			return AppResult<OtePerDateResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<OtePerDateResult>.CreateFailed(ex, "An error occured when getting customer ote per day.");
		}
	}

	public async Task<AppResult<ExpiredEventsResult>> ExpiredEvents()
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/ExpiredEvents")
							.GetJsonAsync<ExpiredEventsResult>();

			return AppResult<ExpiredEventsResult>.CreateSucceeded(result, "Successfully get expired events.");
		}
		catch (FlurlHttpException ex)
		{
			var error = ex.GetResponseJsonAsync();
			return AppResult<ExpiredEventsResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<ExpiredEventsResult>.CreateFailed(ex, "An error occured when getting expired events.");
		}
	}

	public async Task<AppResult<ForceDisableActivitiesResult>> ForceDisableActivities(ForceDisableActivitiesArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/ForceDisableActivities")
							.PostJsonAsync(args)
							.ReceiveJson<ForceDisableActivitiesResult>();

			return AppResult<ForceDisableActivitiesResult>.CreateSucceeded(result, "Successfully disabled activities.");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<ForceDisableActivitiesResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<ForceDisableActivitiesResult>.CreateFailed(ex, "An error occurred when disabling activities.");
		}
	}

	public async Task<AppResult<DeleteTicketResult>> DeleteTicket(DeleteTicketArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/DeleteTicket")
							.PostJsonAsync(args)
							.ReceiveJson<DeleteTicketResult>();

			return AppResult<DeleteTicketResult>.CreateSucceeded(result, "Successfully removed ticket.");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<DeleteTicketResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<DeleteTicketResult>.CreateFailed(ex, "An error occurred when deleting ticket.");
		}
	}

	public async Task<AppResult<ActivityFeedResult>> ActivityFeed(ActivityFeedArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/ActivityFeed")
							.SetQueryParams(args)
							.GetJsonAsync<ActivityFeedResult>();

			return AppResult<ActivityFeedResult>.CreateSucceeded(result, "Successfully getting get all activity feed.");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<ActivityFeedResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<ActivityFeedResult>.CreateFailed(ex, "An error occured when getting all activity feed.");
		}
	}

	public async Task<AppResult<BatchSummaryUpdateResult>> BatchSummaryUpdate()
	{
		try
		{
			var result = await flurlClient
							.Request("Activity/BatchSummaryUpdate")
							.PostAsync()
							.ReceiveJson<BatchSummaryUpdateResult>();

			return AppResult<BatchSummaryUpdateResult>.CreateSucceeded(result, "Successfully update batch summary.");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<BatchSummaryUpdateResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<BatchSummaryUpdateResult>.CreateFailed(ex, "An error occurred when update batch summary.");
		}
	}

	public async Task<AppResult<OteAlreadyBookedResult>> OteAlreadyBooked(int activityId)
	{
		try
		{
			var result = await flurlClient
							.Request($"Activity/OteAlreadyBooked/{activityId}")
							.GetJsonAsync<OteAlreadyBookedResult>();

			return AppResult<OteAlreadyBookedResult>.CreateSucceeded(result, "Successfully getting get all activity feed.");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<OteAlreadyBookedResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<OteAlreadyBookedResult>.CreateFailed(ex, "An error occured when getting all activity feed.");
		}
	}

	public async Task<AppResult<CreateOteWaitlistResult>> CreateOteWaitlist(CreateOteWaitlistArgs args)
	{
		try
		{
			var result = await flurlClient
				.Request("OteWaitlist/CreateOteWaitlist")
				.PostJsonAsync(args)
				.ReceiveJson<CreateOteWaitlistResult>();

			return AppResult<CreateOteWaitlistResult>.CreateSucceeded(result, "Successfully posting create ote waitlist api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<CreateOteWaitlistResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<CreateOteWaitlistResult>.CreateFailed(ex, "An error occured when posting create ote waitlist api");
		}
	}
	public async Task<AppResult<UpdateOteWaitlistResult>> UpdateOteWaitlist(UpdateOteWaitlistArgs args)
	{
		try
		{
			var result = await flurlClient
				.Request("OteWaitlist/UpdateOteWaitlist")
				.PostJsonAsync(args)
				.ReceiveJson<UpdateOteWaitlistResult>();

			return AppResult<UpdateOteWaitlistResult>.CreateSucceeded(result, "Successfully posting update ote waitlist api");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<UpdateOteWaitlistResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<UpdateOteWaitlistResult>.CreateFailed(ex, "An error occured when posting update ote waitlist api");
		}
	}
	public async Task<AppResult<GetOteWaitlistByProviderResult>> GetOteWaitlistByProvider(GetOteWaitlistByProviderArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("OteWaitlist/GetWaitlistByProvider")
							.SetQueryParams(args)
							.GetJsonAsync<GetOteWaitlistByProviderResult>();

			return AppResult<GetOteWaitlistByProviderResult>.CreateSucceeded(result, "Successfully getting get ote waitlist.");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<GetOteWaitlistByProviderResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<GetOteWaitlistByProviderResult>.CreateFailed(ex, "An error occured when getting ote waitlist.");
		}
	}

	public async Task<AppResult<DeleteOteWaitlistResult>> DeleteOteWaitlist(DeleteOteWaitlistArgs args)
	{
		try
		{
			var result = await flurlClient
							.Request("OteWaitlist/DeleteOteWaitlist")
							.PostJsonAsync(args)
							.ReceiveJson<DeleteOteWaitlistResult>();

			return AppResult<DeleteOteWaitlistResult>.CreateSucceeded(result, "Successfully deleted ote waitlist");
		}
		catch (FlurlHttpException ex)
		{
			return AppResult<DeleteOteWaitlistResult>.CreateFailed(ex, ex.Message);
		}
		catch (Exception ex)
		{
			return AppResult<DeleteOteWaitlistResult>.CreateFailed(ex, "An error occurred when deleting ote waitlist");
		}
	}
}