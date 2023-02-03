using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;

public class UpdateStudentAttendanceHandler : IUpdateStudentAttendanceHandler
{
    private readonly IGetOwnedActivitiesHandler getOwnedActivitiesHandler;

    public UpdateStudentAttendanceHandler(IGetOwnedActivitiesHandler getOwnedActivitiesHandler)
    {
        this.getOwnedActivitiesHandler = getOwnedActivitiesHandler;
    }

    public AppResult<UpdateStudentAttendanceResult> Execute(UpdateStudentAttendanceArgs args)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<UpdateStudentAttendanceResult>> ExecuteAsync(UpdateStudentAttendanceArgs args)
    {
        try
        {
            var ownedAtivitiesResult = await getOwnedActivitiesHandler.ExecuteAsync(new ActivityService.Interactors.GetOwnedActivitiesArgs {
                IncludeAtivitySchedules = true
            });
            if(ownedAtivitiesResult.Succeeded || ownedAtivitiesResult.Result == null)
            {
                return AppResult<UpdateStudentAttendanceResult>.CreateFailed(ownedAtivitiesResult.Error.Exception, ownedAtivitiesResult.Message);
            }

            // convert activity schedule Ids to dictionary
            // to get the data faster, key = scheduleId, value = activityId
            IDictionary<int,int> activitySchedules = new Dictionary<int,int>();
            foreach(var activity in ownedAtivitiesResult.Result.Activities)
            {
                foreach(var schedule in activity.ActivitySchedules)
                {
                    activitySchedules.Add(schedule.Id, activity.Id);
                }
            }

            // get enrolled students
            return AppResult<UpdateStudentAttendanceResult>.CreateFailed(null, "An error occured in UpdateStudentAttendanceHandler");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateStudentAttendanceResult>.CreateFailed(ex, "An error occured in UpdateStudentAttendanceHandler");
        }
    }
}