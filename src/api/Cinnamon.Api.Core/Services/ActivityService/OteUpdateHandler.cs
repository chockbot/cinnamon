using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Extensions.DateTimeExtension;
using Cinnamon.Framework.Common;
using Ganss.XSS;
using Cinnamon.Framework.Helpers;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OteUpdateHandler : IOteUpdateHandler
{
    private readonly IActivityData activityData;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IGenerateActivityHandler generateActivityHandler;
    private readonly IOteFindByHandler oteFindByHandler;
    private readonly HtmlSanitizer htmlSanitizer;
    private readonly GenerateRecurrenceDate recurrenceDateHelper;
    private readonly IOteAlreadyBookedHandler oteAlreadyBookedHandler;

    public OteUpdateHandler(IActivityData activityData, IGetProfileHandler getProfileHandler, 
        IGenerateActivityHandler generateActivityHandler, IOteFindByHandler oteFindByHandler,
        IOteAlreadyBookedHandler oteAlreadyBookedHandler)
    {
        this.activityData = activityData;
        this.getProfileHandler = getProfileHandler;
        this.generateActivityHandler = generateActivityHandler;
        this.oteFindByHandler = oteFindByHandler;
        this.oteAlreadyBookedHandler = oteAlreadyBookedHandler;
        this.recurrenceDateHelper = new();

        this.htmlSanitizer = new 
            HtmlSanitizer(
                allowedTags: new string[] {"p","strong", "em", "ul", "ol", "li", "br"});
    }

    public AppResult<OteUpdateResult> Execute(OteUpdateArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<OteUpdateResult>.CreateFailed(ex, "An error occured in OteUpdateHandler");
        }
    }

    public async Task<AppResult<OteUpdateResult>> ExecuteAsync(OteUpdateArgs args)
    {
        try
        {
            var isEmptyPricelist = args.Pricings.Count() == 0;
            if(isEmptyPricelist)
            {
                return AppResult<OteUpdateResult>.CreateFailed(new ApplicationException("Pricing must not empty. Invalid request."), "Pricing must not empty. Invalid request.");
            }

            if (args.Activity.ExperienceTypeId == 2)
            {
                var isEmptyOnlineEventlist = args.OnlineEvents.Count() == 0;
                if (isEmptyOnlineEventlist)
                {
                    return AppResult<OteUpdateResult>.CreateFailed(new ApplicationException("Online event informations must not empty. Invalid request."), "Online event informations must not empty. Invalid request.");
                }
            }

            var currentUser = await this.getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!currentUser.Succeeded || currentUser.Result is null)
            {
                return AppResult<OteUpdateResult>.CreateFailed(new ApplicationException(currentUser.Message), currentUser.Message);
            }
            var activityToUpdateRes = await activityData.GetActivityById(args.Activity.Id, new Framework.ApiCommand.ApiData.Activity.Request.GetActivityArgs {
                CustomerId = currentUser.Result.Id
            });
            if(!activityToUpdateRes.Succeeded || activityToUpdateRes.Result is null || !activityToUpdateRes.Result.IsSuccess)
            {
                return AppResult<OteUpdateResult>.CreateFailed(new ApplicationException("Can't find activity. Invalid request."), "Can't find activity. Invalid request.");
            }
            var activityToUpdate = activityToUpdateRes.Result.Result;

            // update handler only if have changes in activity title
            var handler = activityToUpdate.Handler;
            if(args.Activity.EventName.ToLower() != activityToUpdate.Title.ToLower())
            {
                var generateHandlerRes = await generateActivityHandler.ExecuteAsync(new GenerateActivityHandlerArgs {
                    ActivityName = args.Activity.EventName
                });
                if(!generateHandlerRes.Succeeded || generateHandlerRes.Result == null)
                {
                    return AppResult<OteUpdateResult>.CreateFailed(new ApplicationException(generateHandlerRes.Message), generateHandlerRes.Message);
                }
                handler = generateHandlerRes.Result.GeneratedHandler;
            }

            // validate accepted event duration unit time
            string[] timeUnits = { "hrs", "days", "weeks", "months" };
            if (!timeUnits.Any(t => t == args.Activity.EventDurationTimeUnit))
            {
                return AppResult<OteUpdateResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request.");
            }

            // validate and check dateStart and dateEnd for recurreing schedules
            if (args.Activity.Recurrence.ToLower() != "do-not-repeat")
            {
                if (args.Activity.DurationStart is null || args.Activity.DurationEnd is null || args.Activity.DurationEvery is null)
                {
                    return AppResult<OteUpdateResult>.CreateFailed(
                        new ApplicationException("Invalid arguments for recurring dates."), "Invalid arguments for recurring dates.");
                }
            }

            // validate and check for weekly and every-weekday recurring schedules
            var recurrence = args.Activity.Recurrence.ToLower();
            if (recurrence == "every-weekday" || recurrence == "weekly")
            {
                if (string.IsNullOrEmpty(args.Activity.WeekString))
                {
                    return AppResult<OteUpdateResult>.CreateFailed(
                        new ApplicationException("Invalid arguments for weekly recurring."), "Invalid arguments for weekly recurring.");
                }

                var acceptedDays = new string[] { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" };
                var dayArray = args.Activity.WeekString.Split("|");

                foreach (var day in dayArray)
                {
                    if (!acceptedDays.Any(d => d == day))
                    {
                        return AppResult<OteUpdateResult>.CreateFailed(
                            new ApplicationException("Invalid arguments for weekly recurring."), "Invalid arguments for weekly recurring.");
                    }
                }
            }

            // string format Type|Day|Week|Day
            // example 1|2|first|sunday, or 2|0|first|sunday
            string extraOptionsForMonthlyRecurring = string.Empty;

            // validate and check for monthly recurring schedules
            if (args.Activity.Recurrence.ToLower() == "monthly")
            {
                if (args.Activity.MonthSelection is null || args.Activity.MonthRepeat is null ||
                    args.Activity.MonthDay is null || args.Activity.OnDayDate is null)
                {
                    return AppResult<OteUpdateResult>.CreateFailed(
                        new ApplicationException("Invalid arguments for monthly recurring."), "Invalid arguments for monthly recurring.");
                }

                if (args.Activity.MonthSelection != 1 && args.Activity.MonthSelection != 2)
                {
                    return AppResult<OteUpdateResult>.CreateFailed(
                        new ApplicationException("Invalid arguments for monthly recurring."), "Invalid arguments for monthly recurring.");
                }

                var acceptedRepeats = new string[] { "first", "second", "third", "fourth", "last" };
                if (!acceptedRepeats.Any(r => r == args.Activity.MonthRepeat))
                {
                    return AppResult<OteUpdateResult>.CreateFailed(
                        new ApplicationException("Invalid arguments for monthly recurring."), "Invalid arguments for monthly recurring.");
                }

                var acceptedDays = new string[] { "monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday" };
                if (!acceptedDays.Any(d => d == args.Activity.MonthDay))
                {
                    return AppResult<OteUpdateResult>.CreateFailed(
                        new ApplicationException("Invalid arguments for monthly recurring."), "Invalid arguments for monthly recurring.");
                }

                extraOptionsForMonthlyRecurring = $"{args.Activity.MonthSelection}|{args.Activity.OnDayDate}|{args.Activity.MonthRepeat}|{args.Activity.MonthDay}";
            }

            var every = args.Activity.DurationEvery ?? 0;
            var dateStart = args.Activity.DurationStart ?? DateTime.Now;
            var dateEnd = args.Activity.DurationEnd ?? DateTime.Now;

            var timeStart = args.Activity.ScheduleFrom.TimeOfDay;
            var timeDuration = args.Activity.ScheduleTo - args.Activity.ScheduleFrom;

            var dateItems = args.Activity.Recurrence switch
            {
                "every-weekday" => recurrenceDateHelper.GenerateWeekday(every, dateStart, dateEnd, timeDuration, timeStart),
                "daily" => recurrenceDateHelper.GenerateDaily(every, dateStart, dateEnd, timeDuration, timeStart),
                "weekly" => recurrenceDateHelper.GenerateWeekly(every, dateStart, dateEnd, timeDuration, timeStart, args.Activity.WeekString ?? string.Empty),
                "monthly" => recurrenceDateHelper.GenerateMonthly(every, dateStart, dateEnd, timeDuration, timeStart, args.Activity.MonthSelection ?? 0,
                    args.Activity.OnDayDate ?? 1, args.Activity.MonthRepeat ?? string.Empty, args.Activity.MonthDay ?? string.Empty),
                _ => recurrenceDateHelper.GenerateNoRepeat(args.Activity.ScheduleFrom, args.Activity.ScheduleTo)
            };

            // override dates
            if (args.DateOverrides is not null)
            {
                var dictionaryDates = args.DateOverrides.ToDictionary(d => d.Date.Date);

                foreach (var item in dateItems)
                {
                    if (dictionaryDates.ContainsKey(item.Date.Date))
                    {
                        var date = dictionaryDates[item.Date.Date];
                        item.DateStart = item.DateStart.Date.Add(date.TimeStart);
                        item.DateEnd = item.DateStart.Date.Add(date.TimeEnd);
                    }
                }
            }

            var oteByHandlerRes = await oteFindByHandler.ExecuteAsync(new OteFindByHandlerArgs {
                Handler = handler,
                IncludeSchedule = true
            });
            if(!oteByHandlerRes.Succeeded || oteByHandlerRes.Result is null)
            {
                return AppResult<OteUpdateResult>.CreateFailed(
                        new ApplicationException(oteByHandlerRes.Message), oteByHandlerRes.Message);
            }
            var currentOteDetails = oteByHandlerRes.Result;

            var alreadyBookedRes = await oteAlreadyBookedHandler.ExecuteAsync(new OteAlreadyBookedArgs {
                ActivityId = currentOteDetails.Id
            });
            if(!alreadyBookedRes.Succeeded || alreadyBookedRes.Result is null)
            {
                return AppResult<OteUpdateResult>.CreateFailed(
                        new ApplicationException(alreadyBookedRes.Message), alreadyBookedRes.Message);
            }
            
            bool reCreateSchedule = 
                !currentOteDetails.Schedule.Recurrences.Equals(args.Activity.Recurrence) ||
                currentOteDetails.Schedule.From != args.Activity.ScheduleFrom ||
                currentOteDetails.Schedule.To != args.Activity.ScheduleTo ||
                currentOteDetails.Schedule.RecurrenceDateEnd != (args.Activity.DurationEnd ?? args.Activity.ScheduleTo) ||
                currentOteDetails.Schedule.RecurrenceDateStart != (args.Activity.DurationStart ?? args.Activity.ScheduleFrom) ||
                currentOteDetails.Schedule.RepeatEvery != (args.Activity.DurationEvery ?? 0) ||
                currentOteDetails.Schedule.SelectedDays != (args.Activity.WeekString ?? String.Empty) ||
                currentOteDetails.Schedule.ExtraOptions != (extraOptionsForMonthlyRecurring ?? String.Empty) ||
                currentOteDetails.Schedule.EventDurationCount != args.Activity.EventDurationCount ||
                currentOteDetails.Schedule.EventDurationTimeUnit != args.Activity.EventDurationTimeUnit;
            
            bool alreadyHaveBooked = alreadyBookedRes.Result.OteAlreadyBookedItems.Any(d => d.BookCount > 0);

            if(reCreateSchedule && alreadyHaveBooked)
            {
                return AppResult<OteUpdateResult>.CreateFailed(
                        new ApplicationException("Can't recreate schedule aready have booked tickets."), "Can't recreate schedule aready have booked tickets.");
            }

            List<OteUpdateArgs.OteReschedule> reschedules = new List<OteUpdateArgs.OteReschedule>();

            if(args.OteReschedules is not null)
            {
                foreach (var schedule in args.OteReschedules)
                {
                    var sched = new OteUpdateArgs.OteReschedule {
                        DateEnd = schedule.NewDate.Add(timeStart).Add(timeDuration),
                        DateStart = schedule.NewDate.Add(timeStart),
                        NewDate = schedule.NewDate,
                        OldDate = schedule.OldDate,
                        Id = schedule.Id
                    };
                    reschedules.Add(sched);
                }
            }

            var sortedPrice = args.Pricings.OrderBy(p => p.Price).ToList();
            var stringPrice = sortedPrice.Count > 1 ? string.Format("PHP {0} - {1}", sortedPrice.First().Price, sortedPrice.Last().Price) :
                string.Format("PHP {0}", sortedPrice.First().Price);

            var entity = new Cinnamon.Framework.ApiCommand.ApiData.Activity.Request.UpdateOteActivityArgs {
                Activity = new Framework.ApiCommand.ApiData.Activity.Request.UpdateOteActivityArgs.UpdateOteActivity {
                    BarangayCode          = args.Activity.BarangayCode ?? string.Empty,
                    BarangayName          = args.Activity.BarangayName ?? string.Empty,
                    CategoryId            = args.Activity.CategoryId,
                    CityName              = args.Activity.CityName ?? string.Empty,
                    CityNumber            = args.Activity.CityNumber ?? string.Empty,
                    Description           = htmlSanitizer.Sanitize(args.Activity.Description),
                    EventName             = args.Activity.EventName,
                    ExperienceTypeId      = args.Activity.ExperienceTypeId,
                    Handler               = handler,
                    HouseNo               = args.Activity.HouseNo,
                    Id                    = args.Activity.Id,
                    IsPublished           = args.Activity.IsPublished,
                    PinnedLocation        = args.Activity.PinnedLocation ?? string.Empty,
                    PostalCode            = args.Activity.PostalCode ?? string.Empty,
                    Recurrence            = args.Activity.Recurrence,
                    RegionCode            = args.Activity.RegionCode ?? string.Empty,
                    RegionName            = args.Activity.RegionName ?? string.Empty,
                    ScheduleFrom          = args.Activity.ScheduleFrom,
                    ScheduleTo            = args.Activity.ScheduleTo,
                    StringPrice           = stringPrice,
                    IsComingSoon          = args.Activity.IsComingSoon,
                    RecurrenceDateEnd     = args.Activity.DurationEnd ?? args.Activity.ScheduleTo,
                    RecurrenceDateStart   = args.Activity.DurationStart ?? args.Activity.ScheduleFrom,
                    RepeatEvery           = args.Activity.DurationEvery ?? 0,
                    SelectedDays          = args.Activity.WeekString ?? String.Empty,
                    ExtraOptions          = extraOptionsForMonthlyRecurring ?? String.Empty,
                    EventDurationCount    = args.Activity.EventDurationCount,
                    EventDurationTimeUnit = args.Activity.EventDurationTimeUnit,
                    EventTicketLimit = args.Activity.EventTicketLimit
                },
                Pricings = args.Pricings.Select(p => {
                    return new Framework.ApiCommand.ApiData.Activity.Request.UpdateOteActivityArgs.UpdateOtePricing {
                        Description = p.Description,
                        Id = p.Id,
                        IsAbsorbFees = p.IsAbsorbFees,
                        MaxSlots = p.MaxSlots,
                        Price = p.Price,
                        Name = p.Name
                    };
                }).ToList(),
                Dates = dateItems.Select(d => {
                    return new Framework.ApiCommand.ApiData.Activity.Request.UpdateOteActivityArgs.UpdateOteDate
                    {
                        Date = d.Date,
                        DateEnd = d.DateEnd,
                        DateStart = d.DateStart
                    };
                }).ToList(),
                DateOverrides = args.DateOverrides is not null ?
                    args.DateOverrides.Select(d => {
                        return new Framework.ApiCommand.ApiData.Activity.Request.UpdateOteActivityArgs.UpdateOteDateOverride
                        {
                            Date = d.Date,
                            DateEnd = d.Date.Date.Add(d.TimeEnd),
                            DateStart = d.Date.Date.Add(d.TimeStart)
                        };
                    }).ToList() : null,
                OnlineEvents = args.OnlineEvents is not null ? args.OnlineEvents.Select(p => {
                    return new Framework.ApiCommand.ApiData.Activity.Request.UpdateOteActivityArgs.UpdateOteOnlineEvent
                    {
                        Id                        = p.Id,
                        Title                     = p.Title,
                        Description               = p.Description,
                        VideoLink                 = p.VideoLink,
                        TicketRestriction         = p.TicketRestriction,
                    };
                }).ToList() : null,
                RecreateSchedule = reCreateSchedule,
                OteReschedules = args.OteReschedules is not null ? reschedules.Select(s => new Framework.ApiCommand.ApiData.Activity.Request.UpdateOteActivityArgs.OteReschedule {
                    DateEnd = s.DateEnd,
                    DateStart = s.DateStart,
                    NewDate = s.NewDate,
                    OldDate = s.OldDate,
                    Id = s.Id
                }).ToList() : null
            };
            var updateOte = await activityData.UpdateOteActivity(entity);
            if(!updateOte.Succeeded || updateOte.Result is null || !updateOte.Result.IsSuccess)
            {
                return AppResult<OteUpdateResult>.CreateFailed(new ApplicationException(updateOte.Result?.ErrorInfo?.Message), updateOte.Message);
            }

            return AppResult<OteUpdateResult>.CreateSucceeded(new OteUpdateResult {Id = updateOte.Result.Result.Id}, "One time event successfully updated.");
        }
        catch (Exception ex)
        {
            return AppResult<OteUpdateResult>.CreateFailed(ex, "An error occured in OteUpdateHandler");
        }
    }
}