using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OteEmailReminderHandler : IOteEmailReminderHandler
{
    private readonly IOteRemindersData oteRemindersData;
    private readonly IOteFindByHandler oteFindByHandler;
    private readonly IGetActivityHandler getActivityHandler;

    public OteEmailReminderHandler(IOteRemindersData oteRemindersData, 
        IGetActivityHandler getActivityHandler, IOteFindByHandler oteFindByHandler)
    {
        this.oteRemindersData = oteRemindersData;
        this.getActivityHandler = getActivityHandler;
        this.oteFindByHandler = oteFindByHandler;
    }

    public AppResult<OteEmailReminderResult> Execute(OteEmailReminderArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OteEmailReminderResult>> ExecuteAsync(OteEmailReminderArgs args)
    {
        try
        {
            var forRemindersRes = await oteRemindersData.GetEventsForReminder();
            if(!forRemindersRes.Succeeded || forRemindersRes.Result is null || !forRemindersRes.Result.IsSuccess)
            {
                return AppResult<OteEmailReminderResult>.CreateFailed(new ApplicationException(forRemindersRes.Result?.ErrorInfo?.Message), forRemindersRes.Message);
            }
            var forReminders = forRemindersRes.Result.Result;

            IDictionary<int, int> fetchedActivities = new Dictionary<int, int>();
            var now = DateTime.Now.Date;

            foreach(var reminder in forReminders)
            {
                if(!fetchedActivities.ContainsKey(reminder.ActivityId))
                {
                    var activityRes = await getActivityHandler.ExecuteAsync(new GetActivityArgs {
                        ActivityId = reminder.ActivityId
                    });
                    if(!activityRes.Succeeded || activityRes.Result is null)
                    {
                        // skip and continue to next activity;
                        continue;
                    }

                    var oteByHandlerRes = await oteFindByHandler.ExecuteAsync(new OteFindByHandlerArgs {
                        Handler = activityRes.Result.Handler,
                        IncludeSchedule = true,
                    });
                    if(!oteByHandlerRes.Succeeded || oteByHandlerRes.Result is null)
                    {
                        // skip and continue to next activity;
                        continue;
                    }
                    var activity = oteByHandlerRes.Result;

                    var defaultReminderDays = activity.Schedule.EmailReminderDays == 0 ? 1 : activity.Schedule.EmailReminderDays;
                    fetchedActivities.Add(activity.Id, defaultReminderDays);
                }

                var daysToRemind = fetchedActivities[reminder.ActivityId];

                if(reminder.DateStart.AddDays(-daysToRemind).Date == now.Date)
                {
                    // then reminder users who booked the event
                    var 
                }
            }
        }
        catch (Exception ex)
        {
            return AppResult<OteEmailReminderResult>.CreateFailed(ex, "An error occured in OteEmailReminderHandler.");
        }
    }
}