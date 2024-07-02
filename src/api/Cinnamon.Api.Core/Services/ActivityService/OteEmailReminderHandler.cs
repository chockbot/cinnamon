using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
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
    private readonly IOteEventReminderNotificationHandler oteEventReminderNotificationHandler;
    private readonly IDynamicContentData dynamicContentData;

    public OteEmailReminderHandler(IOteRemindersData oteRemindersData, 
        IGetActivityHandler getActivityHandler, IOteFindByHandler oteFindByHandler,
        IOteEventReminderNotificationHandler oteEventReminderNotificationHandler,
        IDynamicContentData dynamicContentData)
    {
        this.oteRemindersData = oteRemindersData;
        this.getActivityHandler = getActivityHandler;
        this.oteFindByHandler = oteFindByHandler;
        this.oteEventReminderNotificationHandler = oteEventReminderNotificationHandler;
        this.dynamicContentData = dynamicContentData;
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

            IDictionary<int, CachedActivity> cachedActivities = new Dictionary<int, CachedActivity>();
            var now = DateTime.Now.Date;

            foreach(var reminder in forReminders)
            {
                if(!cachedActivities.ContainsKey(reminder.ActivityId))
                {
                    var activityRes = await getActivityHandler.ExecuteAsync(new GetActivityArgs {
                        ActivityId = reminder.ActivityId,
                        IncludeCustomer = true,
                    });
                    if(!activityRes.Succeeded || activityRes.Result is null)
                    {
                        // skip and continue to next activity;
                        continue;
                    }

                    var oteByHandlerRes = await oteFindByHandler.ExecuteAsync(new OteFindByHandlerArgs {
                        Handler = activityRes.Result.Handler,
                        IncludeSchedule = true,
                        IncludeAddress = true
                    });
                    if(!oteByHandlerRes.Succeeded || oteByHandlerRes.Result is null)
                    {
                        // skip and continue to next activity;
                        continue;
                    }
                    var oteActivity = oteByHandlerRes.Result;

                    var defaultReminderDays = oteActivity.Schedule.EmailReminderDays == 0 ? 1 : oteActivity.Schedule.EmailReminderDays;
                    cachedActivities.Add(reminder.ActivityId, new CachedActivity {
                        ActivityId = reminder.ActivityId,
                        DaysBeforeReminder = defaultReminderDays,
                        ProviderId = activityRes.Result.Owner?.Id ?? 0,
                        EventLocation = oteActivity.ExperienceTypeId == 2 ? 
                            "Online" : $"{oteActivity.CityName} {oteActivity.RegionName} {oteActivity.PinnedLocation}".Trim(),
                        EventName = oteActivity.EventName
                    });
                }

                var activity = cachedActivities[reminder.ActivityId];

                if(reminder.DateStart.AddDays(-activity.DaysBeforeReminder).Date != now.Date)
                {
                    // continue guard only send email to day before according to setted days
                    continue;
                }

                string subject = string.Empty;
                string body = string.Empty;

                // get custom subject and custom body
                var customSubBodyRes = await dynamicContentData.GetEmailTemplates(new Framework.ApiCommand.ApiData.DynamicContent.Request.GetEmailTemplatesArgs {
                    ActivityId = reminder.ActivityId,
                    ProviderId = activity.ProviderId,
                    TemplateType = Cinnamon.Framework.Enums.EmailTemplateType.OteReminder.ToString()
                });
                if(customSubBodyRes.Succeeded && customSubBodyRes.Result is not null && customSubBodyRes.Result.IsSuccess && customSubBodyRes.Result.Result.Any())
                {
                    var template = customSubBodyRes.Result.Result.First();
                    subject = template.Subject;
                    body = template.Body;
                }

                // then reminder users who booked the event
                var getCustomersToRemind = await oteRemindersData.GetCustomersToRemind(new Framework.ApiCommand.ApiData.OteReminderFlags.Request.GetCustomersToRemindArgs {
                    ActivityId = reminder.ActivityId,
                    OteDateId = reminder.DateId
                });

                if(!getCustomersToRemind.Succeeded || getCustomersToRemind.Result is null || !getCustomersToRemind.Result.IsSuccess)
                {
                    // skip and continue to next activity;
                    continue;
                }
                var customers = getCustomersToRemind.Result.Result;

                var sendEmailTasks = customers.Select(c => oteEventReminderNotificationHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.OteEventReminderNotificationArgs {
                    Body = body,
                    CustomerEmail = c.Email,
                    CustomerName = $"{c.FirstName} {c.LastName}",
                    DaysStart = activity.DaysBeforeReminder,
                    EventDate = reminder.DateStart,
                    EventLocation = activity.EventLocation,
                    EventName = activity.EventName,
                    Subject = subject
                }));

                await Task.WhenAll(sendEmailTasks);

                // create flag and dont check if success or not
                var createReminderFlagRes = await oteRemindersData.CreateReminderFlag(new Framework.ApiCommand.ApiData.OteReminderFlags.Request.CreateReminderFlagArgs {
                    ActivityId = reminder.ActivityId,
                    OteDateId = reminder.DateId
                });
            }

            return AppResult<OteEmailReminderResult>.CreateSucceeded(new(), "Successfully notify event reminders.");
        }
        catch (Exception ex)
        {
            return AppResult<OteEmailReminderResult>.CreateFailed(ex, "An error occured in OteEmailReminderHandler.");
        }
    }

    private class CachedActivity 
    {
        public int ActivityId {get; set;}
        public int DaysBeforeReminder {get; set;}
        public int ProviderId {get; set;}
        public string EventLocation {get; set;}
        public string EventName {get; set;}
    }
}