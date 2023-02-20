using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;

public class GetActivityScheduleHandler : IGetActivitySchedulesHandler
{
    private readonly IGetOwnedActivitiesHandler getOwnedActivitiesHandler;

    public GetActivityScheduleHandler(IGetOwnedActivitiesHandler getOwnedActivitiesHandler)
    {
        this.getOwnedActivitiesHandler = getOwnedActivitiesHandler;
    }
    
    public AppResult<GetActivityScheduleResult> Execute(GetActivityScheduleArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetActivityScheduleResult>.CreateFailed(ex, "An error occured in GetActivityScheduleHandler");
        }
    }

    public async Task<AppResult<GetActivityScheduleResult>> ExecuteAsync(GetActivityScheduleArgs args)
    {
        try
        {
            var result = await getOwnedActivitiesHandler.ExecuteAsync(new ActivityService.Interactors.GetOwnedActivitiesArgs {
                IncludeAtivitySchedules = true
            });
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<GetActivityScheduleResult>.CreateFailed(result.Error.Exception, result.Message);
            }

            IList<GetActivityScheduleResult.ActivitySchedule> schedules = new List<GetActivityScheduleResult.ActivitySchedule>();
            foreach(var activity in result.Result.Activities)
            {
                foreach(var schedule in activity.ActivitySchedules)
                {
                    schedules.Add(new GetActivityScheduleResult.ActivitySchedule {
                        ActivityId = activity.Id,
                        Title = activity.Title,
                        Description = activity.Description,
                        ScheduleTitle = schedule.Name,
                        ScheduleDescription = schedule.DateTime,
                        ScheduleId = schedule.Id,
                        isSetSession = schedule.isSetSession,
                        SessionName = schedule.SessionName

                    });
                }
            }

            return AppResult<GetActivityScheduleResult>.CreateSucceeded(new GetActivityScheduleResult {
                Schedules = schedules
            }, "Successfully get activity schedules");
        }
        catch (Exception ex)
        {
            return AppResult<GetActivityScheduleResult>.CreateFailed(ex, "An error occured in GetActivityScheduleHandler");
        }
    }
}