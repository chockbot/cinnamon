using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class PopularActivitiesHandler : IPopularActivitiesHandler
{
    private readonly IActivityData activityData;
    private readonly IMapper mapper;

    public PopularActivitiesHandler(IActivityData activityData, IMapper mapper)
    {
        this.activityData = activityData;
        this.mapper = mapper;
    }

    public AppResult<PopularActivitiesResult> Execute(PopularActivitiesArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<PopularActivitiesResult>> ExecuteAsync(PopularActivitiesArgs args)
    {
        try
        {
            var popularRes = await activityData.PopularActivities(new Framework.ApiCommand.ApiData.Activity.Request.PopularActivitiesArgs {
                CountPerPage = args.Take,
                PageIndex = args.Skip,
                CategoryId = args.CategoryId
            });
            if(!popularRes.Succeeded || popularRes.Result is null || !popularRes.Result.IsSuccess)
            {
                return AppResult<PopularActivitiesResult>.CreateFailed(new ApplicationException(popularRes.Result?.ErrorInfo?.Message), popularRes.Message);
            }

            var result = mapper.Map<IEnumerable<PopularActivitiesResult.ActivityFeed>>(popularRes.Result.Result);
            return AppResult<PopularActivitiesResult>.CreateSucceeded(new PopularActivitiesResult { ActivityFeeds = result}, "Popular activities successfully get");
        }
        catch (Exception ex)
        {
            return AppResult<PopularActivitiesResult>.CreateFailed(ex, "An error occured in PopularActivitiesHandler");
        }
    }
}