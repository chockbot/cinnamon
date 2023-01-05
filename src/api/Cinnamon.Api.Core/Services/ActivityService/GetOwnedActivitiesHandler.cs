using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class GetOwnedActivitiesHandler : IGetOwnedActivitiesHandler
{
    private readonly IHttpContextAccessor httpContext;
    private readonly IActivityData activityData;

    public GetOwnedActivitiesHandler(IHttpContextAccessor httpContext, IActivityData activityData)
    {
        this.httpContext = httpContext;
        this.activityData = activityData;
    }

    public AppResult<GetOwnedActivitiesResult> Execute(GetOwnedActivitiesArgs args)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<GetOwnedActivitiesResult>> ExecuteAsync(GetOwnedActivitiesArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<GetOwnedActivitiesResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            return null;
            
        }
        catch (Exception ex)
        {
            return AppResult<GetOwnedActivitiesResult>.CreateFailed(ex, "An error occured in GetOwnedActivitiesHandler");
        }
    }
}