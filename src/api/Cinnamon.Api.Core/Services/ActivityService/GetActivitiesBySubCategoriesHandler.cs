using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GetActivitiesBySubCategoriesHandler: IGetActivitiesBySubCategoriesHandler
{
    private readonly IActivityData activityData;
	public GetActivitiesBySubCategoriesHandler(IActivityData activityData)
	{
        this.activityData = activityData;
    }

    public AppResult<GetActivitiesBySubCategoriesResult> Execute(GetActivitiesBySubCategoriesArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetActivitiesBySubCategoriesResult>.CreateFailed(ex, "An error occured in GetActivitiesBySubCategoriesHandler");
        }
    }

    public async Task<AppResult<GetActivitiesBySubCategoriesResult>> ExecuteAsync(GetActivitiesBySubCategoriesArgs args)
    {
        try
        {
            return null;
        }
        catch (Exception)
        {

            throw;
        }
    }
}
