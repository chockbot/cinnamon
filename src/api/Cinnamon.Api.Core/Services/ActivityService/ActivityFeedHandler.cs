using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class ActivityFeedHandler : IActivityFeedHandler
{
    private readonly IActivityData activityData;
    private readonly IMapper mapper;

    public ActivityFeedHandler(IActivityData activityData, IMapper mapper)
    {
        this.activityData = activityData;
        this.mapper = mapper;
    }

    public AppResult<ActivityFeedResult> Execute(ActivityFeedArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<ActivityFeedResult>> ExecuteAsync(ActivityFeedArgs args)
    {
        try
        {
            var feedRes = await activityData.ActivityFeed(new Framework.ApiCommand.ApiData.Activity.Request.ActivityFeedArgs {
                CategoryId = args.CategoryId,
                Search = args.Search,
                Skip = args.Skip,
                Take = args.Take,
                ExperienceType = args.ExperienceType,
                StarReview = args.StarReview
            });
            if(!feedRes.Succeeded || feedRes.Result is null || !feedRes.Result.IsSuccess)
            {
                return AppResult<ActivityFeedResult>.CreateFailed(new ApplicationException(feedRes.Result?.ErrorInfo?.Message), feedRes.Message);
            }

            var result = mapper.Map<IEnumerable<ActivityFeedResult.ActivityFeed>>(feedRes.Result.Result);
            return AppResult<ActivityFeedResult>.CreateSucceeded(new ActivityFeedResult { ActivityFeeds = result}, "Successfully get activity feed.");
        }
        catch (Exception ex)
        {
            return AppResult<ActivityFeedResult>.CreateFailed(ex, "An error occured in ActivityFeedHandler.");
        }
    }
}