using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class PopularActivitiesHandler : IPopularActivitiesHandler
{
    private readonly IActivityData activityData;

    public PopularActivitiesHandler(IActivityData activityData)
    {
        this.activityData = activityData;
    }

    public AppResult<PopularActivitiesResult> Execute(PopularActivitiesArgs interactor)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<PopularActivitiesResult>> ExecuteAsync(PopularActivitiesArgs args)
    {
        try
        {
            var result = await activityData.PopularActivities(new Framework.ApiCommand.ApiData.Activity.Request.PopularActivitiesArgs {
                CountPerPage = args.Take,
                PageIndex = args.Skip
            });
            if(!result.Succeeded || result.Result is null || !result.Result.IsSuccess)
            {
                return AppResult<PopularActivitiesResult>.CreateFailed(new ApplicationException(result.Result?.ErrorInfo?.Message), result.Message);
            }
            var activities = result.Result.Result;

            return AppResult<PopularActivitiesResult>.CreateSucceeded(new PopularActivitiesResult {
                Activities = activities.Select(a => {
                    return new PopularActivitiesResult.Activity {
                        CityName                 = a.CityName,
                        ExperienceTypeId         = a.ExperienceTypeId,
                        Handler                  = a.Handler,
                        Id                       = a.Id,
                        ImageSrc                 = a.ImageSrc,
                        IsNew                    = a.IsNew,
                        MakerId                  = a.MakerId,
                        OngoingStudentCount      = a.OngoingStudentCount,
                        Price                    = a.Price,
                        Rating                   = a.Rating,
                        RegionName               = a.RegionName,
                        ReviewCount              = a.ReviewCount,
                        StudentCount             = a.StudentCount,
                        Title                    = a.Title,
                        ExperienceCreationTypeId = a.ExperienceCreationTypeId,
                        NumberOfTickets          = a.NumberOfTickets
                    };
                })
            }, "Popular activities successfully get");
        }
        catch (Exception ex)
        {
            return AppResult<PopularActivitiesResult>.CreateFailed(ex, "An error occured in PopularActivitiesHandler");
        }
    }
}