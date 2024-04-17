using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class ActivityFeedHandler : IActivityFeedHandler
{
    public AppResult<ActivityFeedResult> Execute(ActivityFeedArgs interactor)
    {
        throw new NotImplementedException();
    }

    public Task<AppResult<ActivityFeedResult>> ExecuteAsync(ActivityFeedArgs interactor)
    {
        throw new NotImplementedException();
    }
}