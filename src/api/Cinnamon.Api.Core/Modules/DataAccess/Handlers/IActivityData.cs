using Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Response;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Response;
using Cinnamon.Framework.ApiCommand.ApiData.AddOns.Request;
using Cinnamon.Framework.ApiCommand.ApiData.AddOns.Response;
using Cinnamon.Framework.ApiCommand.ApiData.OnlineEvent.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OnlineEvent.Response;
using Cinnamon.Framework.ApiCommand.ApiData.OteWaitlist.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OteWaitlist.Response;
using Cinnamon.Framework.Common;
namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IActivityData
{
    Task<AppResult<GetActivityResult>> GetActivityById(int id, GetActivityArgs? args = null);
    Task<AppResult<GetActivityResult>> GetActivityByHandler(string handler, GetActivityArgs? args = null);
    Task<AppResult<GetAllActivitiesResult>> GetAllActivities(GetAllActivities args);
    Task<AppResult<CreatedActivityResult>> CreateActivity(CreateActivityArgs args);
    Task<AppResult<UpdatedActivityResult>> UpdateActivity(UpdateActivity args);
    Task<AppResult<GetActivitiesByCategoriesResult>> GetActivitiesByCategories(int id, GetActivityArgs? args = null);
    Task<AppResult<GetActivitiesBySubCategoriesResult>> GetActivitiesBySubCategories(int id, GetActivityArgs? args = null);
    Task<AppResult<GetAllActivitiesResult>> GetPopularActivities(GetAllActivities args);
    Task<AppResult<UpdatedActivityResult>> UpdateActivityGuid(UpdateActivity args);
    Task<AppResult<DeleteActivityResult>> DeleteActivityById(DeleteActivityArgs args);
    Task<AppResult<RecommendedActivitiesResult>> RecommendedActivities(int primaryId, int count, GetRecommendedActivitiesArgs args);
    Task<AppResult<PopularActivitiesResult>> PopularActivities(PopularActivitiesArgs args);
    Task<AppResult<CreateOteActivityResult>> CreateOteActivity(CreateOteActivityArgs args);
    Task<AppResult<UpdateOteActivityResult>> UpdateOteActivity(UpdateOteActivityArgs args);
    Task<AppResult<GetOteActivityByHandlerResult>> GetOteActivityByHandler(GetOteActivityArgs args, string handler);
    Task<AppResult<GetOTEByProvideResult>> GetOTEByProvider(GetOTEByProvideArgs args);
    Task<AppResult<AddTicketSoldResult>> AddTicketSolds(AddTicketSoldArgs args);
    Task<AppResult<CustomerOteResult>> CustomerOte(int customerId);
    Task<AppResult<DeleteAddOnsResult>> DeleteAddOns(DeleteAddOnsArgs args);
    Task<AppResult<DeleteAddOnResult>> DeleteAddOn(DeleteAddOnArgs args);
    Task<AppResult<OtePerDateResult>> OtePerDate(OtePerDateArgs args);
    Task<AppResult<DeleteOnlineEventResult>> DeleteOnlineEvent(DeleteOnlineEventArgs args);
    Task<AppResult<ExpiredEventsResult>> ExpiredEvents();
    Task<AppResult<ForceDisableActivitiesResult>> ForceDisableActivities(ForceDisableActivitiesArgs args);
    Task<AppResult<DeleteTicketResult>> DeleteTicket(DeleteTicketArgs args);
    Task<AppResult<ActivityFeedResult>> ActivityFeed(ActivityFeedArgs args);
    Task<AppResult<BatchSummaryUpdateResult>> BatchSummaryUpdate();
    Task<AppResult<OteAlreadyBookedResult>> OteAlreadyBooked(int activityId);
    Task<AppResult<CreateOteWaitlistResult>> CreateOteWaitlist(CreateOteWaitlistArgs args);
    Task<AppResult<UpdateOteWaitlistResult>> UpdateOteWaitlist(UpdateOteWaitlistArgs args);
    Task<AppResult<GetOteWaitlistByProviderResult>> GetOteWaitlistByProvider(GetOteWaitlistByProviderArgs args);
    Task<AppResult<DeleteOteWaitlistResult>> DeleteOteWaitlist(DeleteOteWaitlistArgs args);
    Task<AppResult<GetWaitListResult>> GetOteWaitList(int id);
}