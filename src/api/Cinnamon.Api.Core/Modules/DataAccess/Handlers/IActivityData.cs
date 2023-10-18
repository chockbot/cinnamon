using Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Response;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Request;
using Cinnamon.Framework.ApiCommand.ApiData.OteTicket.Response;
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
    Task<AppResult<RecommendedActivitiesResult>> RecommendedActivities(int primaryId, int count);
    Task<AppResult<PopularActivitiesResult>> PopularActivities(PopularActivitiesArgs args);
    Task<AppResult<CreateOteActivityResult>> CreateOteActivity(CreateOteActivityArgs args);
    Task<AppResult<UpdateOteActivityResult>> UpdateOteActivity(UpdateOteActivityArgs args);
    Task<AppResult<GetOteActivityByHandlerResult>> GetOteActivityByHandler(GetOteActivityArgs args, string handler);
    Task<AppResult<AddTicketSoldResult>> AddTicketSolds(AddTicketSoldArgs args);
    Task<AppResult<GetOTEByProvideResult>> GetOTEByProvider(GetOTEByProvideArgs args);
}
