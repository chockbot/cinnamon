using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Extensions.DateTimeExtension;
using Cinnamon.Framework.Helpers;
using Cinnamon.Framework.Enums;
using Ganss.XSS;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OteCreateHandler : IOteCreateHandler
{
    private readonly IActivityData activityData;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IGenerateActivityHandler generateActivityHandler;
    private readonly ICustomerData customerData;
    private readonly HtmlSanitizer htmlSanitizer;
    private readonly GenerateRecurrenceDate recurrenceDateHelper;
    private readonly ISaveEmailTemplateHandler saveEmailTemplateHandler;
    private readonly IProviderCustomQuestionData providerCustomQuestionData;
    private readonly ISeatPlanData seatPlanData;

    public OteCreateHandler(IActivityData activityData, IGetProfileHandler getProfileHandler,
        IGenerateActivityHandler generateActivityHandler, ICustomerData customerData,
        ISaveEmailTemplateHandler saveEmailTemplateHandler, IProviderCustomQuestionData providerCustomQuestionData,
        ISeatPlanData seatPlanData)
    {
        this.activityData = activityData;
        this.getProfileHandler = getProfileHandler;
        this.generateActivityHandler = generateActivityHandler;
        this.customerData = customerData;
        this.recurrenceDateHelper = new();
        this.saveEmailTemplateHandler = saveEmailTemplateHandler;
        this.providerCustomQuestionData = providerCustomQuestionData;
        this.seatPlanData = seatPlanData;

        this.htmlSanitizer = new 
            HtmlSanitizer(
                allowedTags: new string[] {"p","strong", "em", "ul", "ol", "li", "br"});
    }

    public AppResult<OteCreateResult> Execute(OteCreateArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<OteCreateResult>.CreateFailed(ex, "An error occured in OteCreateHandler");
        }
    }

    public async Task<AppResult<OteCreateResult>> ExecuteAsync(OteCreateArgs args)
    {
        try
        {
            var isEmptyPricelist = args.Pricings.Count() == 0;
            if(isEmptyPricelist)
            {
                return AppResult<OteCreateResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request.");
            }

            // check if reserve seat type and check if selected template is valid
            int seatPlanTemplateId = 0;
            string seatPlanPayload = string.Empty;
            if(args.Activity.ReserveSeat)
            {
                var seatPlanRes = await seatPlanData.GetSeatPlanTemplateByIdAsync(args.Activity.SeatPlanTemplateId);
                if(!seatPlanRes.Succeeded || seatPlanRes.Result is null || !seatPlanRes.Result.IsSuccess)
                {
                    return AppResult<OteCreateResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request.");
                }
                var seatPlan = seatPlanRes.Result.Result;
                seatPlanPayload = seatPlan.Payload;
                seatPlanTemplateId = seatPlan.Id;
            }

            var currentUser = await this.getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!currentUser.Succeeded || currentUser.Result is null)
            {
                return AppResult<OteCreateResult>.CreateFailed(new ApplicationException(currentUser.Message), currentUser.Message);
            }

            var activity = args.Activity;

            var generateHandlerRes = await generateActivityHandler.ExecuteAsync(new GenerateActivityHandlerArgs {
                ActivityName = activity.EventName
            });
            if(!generateHandlerRes.Succeeded || generateHandlerRes.Result is null)
            {
                return AppResult<OteCreateResult>.CreateFailed(new ApplicationException(generateHandlerRes.Message), generateHandlerRes.Message);
            }

            // validate accepted event duration unit time
            string[] timeUnits = {"hrs", "days", "weeks", "months"};
            if(!timeUnits.Any(t => t == args.Activity.EventDurationTimeUnit))
            {
                return AppResult<OteCreateResult>.CreateFailed(new ApplicationException("Invalid request."), "Invalid request.");
            }

            // validate and check dateStart and dateEnd for recurreing schedules
            if(args.Activity.Recurrence.ToLower() != "do-not-repeat")
            {
                if(args.Activity.DurationStart is null || args.Activity.DurationEnd is null || args.Activity.DurationEvery is null)
                {
                    return AppResult<OteCreateResult>.CreateFailed(
                        new ApplicationException("Invalid arguments for recurring dates."), "Invalid arguments for recurring dates.");
                }
            }

            // validate and check for weekly and every-weekday recurring schedules
            var recurrence = args.Activity.Recurrence.ToLower();
            if(recurrence == "every-weekday" || recurrence == "weekly")
            {
                if(string.IsNullOrEmpty(args.Activity.WeekString))
                {
                    return AppResult<OteCreateResult>.CreateFailed(
                        new ApplicationException("Invalid arguments for weekly recurring."), "Invalid arguments for weekly recurring.");
                }

                var acceptedDays = new string[] {"MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN"};
                var dayArray = args.Activity.WeekString.Split("|");
                
                foreach(var day in dayArray)
                {
                    if(!acceptedDays.Any(d => d == day))
                    {
                        return AppResult<OteCreateResult>.CreateFailed(
                            new ApplicationException("Invalid arguments for weekly recurring."), "Invalid arguments for weekly recurring.");
                    }
                }
            }

            // string format Type|Day|Week|Day
            // example 1|2|first|sunday, or 2|0|first|sunday
            string extraOptionsForMonthlyRecurring = string.Empty;

            // validate and check for monthly recurring schedules
            if(args.Activity.Recurrence.ToLower() == "monthly")
            {
                if(args.Activity.MonthSelection is null || args.Activity.MonthRepeat is null || 
                    args.Activity.MonthDay is null || args.Activity.OnDayDate is null)
                {
                    return AppResult<OteCreateResult>.CreateFailed(
                        new ApplicationException("Invalid arguments for monthly recurring."), "Invalid arguments for monthly recurring.");
                }

                if(args.Activity.MonthSelection != 1 && args.Activity.MonthSelection != 2)
                {
                    return AppResult<OteCreateResult>.CreateFailed(
                        new ApplicationException("Invalid arguments for monthly recurring."), "Invalid arguments for monthly recurring.");
                }

                var acceptedRepeats = new string[] {"first", "second", "third", "fourth", "last"};
                if(!acceptedRepeats.Any(r => r == args.Activity.MonthRepeat))
                {
                    return AppResult<OteCreateResult>.CreateFailed(
                        new ApplicationException("Invalid arguments for monthly recurring."), "Invalid arguments for monthly recurring.");
                }

                var acceptedDays = new string[] {"monday", "tuesday", "wednesday", "thursday", "friday", "saturday", "sunday"};
                if(!acceptedDays.Any(d => d == args.Activity.MonthDay))
                {
                    return AppResult<OteCreateResult>.CreateFailed(
                        new ApplicationException("Invalid arguments for monthly recurring."), "Invalid arguments for monthly recurring.");
                }

                extraOptionsForMonthlyRecurring = $"{args.Activity.MonthSelection}|{args.Activity.OnDayDate}|{args.Activity.MonthRepeat}|{args.Activity.MonthDay}";
            }

            var every = args.Activity.DurationEvery ?? 0;
            var dateStart = args.Activity.DurationStart ?? DateTime.Now;
            var dateEnd = args.Activity.DurationEnd ?? DateTime.Now;

            var timeStart = args.Activity.ScheduleFrom.TimeOfDay;
            var timeDuration = args.Activity.ScheduleTo - args.Activity.ScheduleFrom;

            var dateItems = args.Activity.Recurrence switch {
                "every-weekday" => recurrenceDateHelper.GenerateWeekday(every, dateStart, dateEnd, timeDuration, timeStart),
                "daily" => recurrenceDateHelper.GenerateDaily(every, dateStart, dateEnd, timeDuration, timeStart),
                "weekly" => recurrenceDateHelper.GenerateWeekly(every, dateStart, dateEnd, timeDuration, timeStart, args.Activity.WeekString ?? string.Empty),
                "monthly" => recurrenceDateHelper.GenerateMonthly(every, dateStart, dateEnd, timeDuration, timeStart, args.Activity.MonthSelection ?? 0, 
                args.Activity.OnDayDate ?? 1, args.Activity.MonthRepeat ?? string.Empty, args.Activity.MonthDay ?? string.Empty),
                _ => recurrenceDateHelper.GenerateNoRepeat(args.Activity.ScheduleFrom, args.Activity.ScheduleTo)
            };

            // override dates
            if(args.DateOverrides is not null)
            {
                var dictionaryDates = args.DateOverrides.ToDictionary(d => d.Date.Date);

                foreach(var item in dateItems)
                {
                    if(dictionaryDates.ContainsKey(item.Date.Date))
                    {
                        var date = dictionaryDates[item.Date.Date];
                        item.DateStart = item.DateStart.Date.Add(date.TimeStart);
                        item.DateEnd = item.DateStart.Date.Add(date.TimeEnd);
                    }
                }
            }

            var sortedPrice = args.Pricings.OrderBy(p => p.Price).ToList();
            var stringPrice = sortedPrice.Count > 1 ? string.Format("PHP {0} - {1}", sortedPrice.First().Price, sortedPrice.Last().Price) :
                string.Format("PHP {0}", sortedPrice.First().Price);

            var createOteRes = await activityData.CreateOteActivity(new Framework.ApiCommand.ApiData.Activity.Request.CreateOteActivityArgs {
                Activity = new Framework.ApiCommand.ApiData.Activity.Request.CreateOteActivityArgs.OteActivity {
                    BarangayCode             = activity.BarangayCode ?? string.Empty,
                    BarangayName             = activity.BarangayName ?? string.Empty,
                    CityName                 = activity.CityName ?? string.Empty,
                    CityNumber               = activity.CityNumber ?? string.Empty,
                    CustomerId               = currentUser.Result.Id,
                    Description              = htmlSanitizer.Sanitize(activity.Description),
                    EventName                = activity.EventName,
                    ExperienceCreationTypeId = activity.ExperienceCreationTypeId,
                    ExperienceTypeId         = activity.ExperienceTypeId,
                    CategoryId               = activity.CategoryId,
                    Handler                  = generateHandlerRes.Result.GeneratedHandler,
                    HouseNo                  = activity.HouseNo ?? string.Empty,
                    IsPublished              = activity.IsPublished,
                    PinnedLocation           = activity.PinnedLocation ?? string.Empty,
                    PostalCode               = activity.PostalCode ?? string.Empty,
                    Recurrence               = activity.Recurrence,
                    RegionCode               = activity.RegionCode ?? string.Empty,
                    RegionName               = activity.RegionName ?? string.Empty,
                    ScheduleFrom             = activity.ScheduleFrom,
                    ScheduleTo               = activity.ScheduleTo,
                    StringPrice              = stringPrice,
                    IsComingSoon             = args.Activity.IsComingSoon,
                    RecurrenceDateEnd        = args.Activity.DurationEnd ?? args.Activity.ScheduleTo,
                    RecurrenceDateStart      = args.Activity.DurationStart ?? args.Activity.ScheduleFrom,
                    RepeatEvery              = args.Activity.DurationEvery ?? 0,
                    SelectedDays             = args.Activity.WeekString ?? String.Empty,
                    ExtraOptions             = extraOptionsForMonthlyRecurring ?? String.Empty,
                    EventDurationCount       = args.Activity.EventDurationCount,
                    EventDurationTimeUnit    = args.Activity.EventDurationTimeUnit,
                    EventTicketLimit         = args.Activity.EventTicketLimit,
                    IsOpen                   = args.Activity.IsOpen,
                    IsCapacity               = args.Activity.IsCapacity,
                    CapacityCount            = args.Activity.CapacityCount,
                    EmailFeedbackDays        = args.Activity.EmailFeedbackDays,
                    EmailReminderDays        = args.Activity.EmailReminderDays,
                    ReserveSeat              = args.Activity.ReserveSeat,
                    SeatPlanTemplateId       = seatPlanTemplateId,
                    SeatPlanPayload          = seatPlanPayload,
                },
                Pricings = args.Pricings.Select(p => {
                    return new Framework.ApiCommand.ApiData.Activity.Request.CreateOteActivityArgs.OtePricing {
                        Description = p.Description,
                        IsAbsorbFees = p.IsAbsorbFees,
                        MaxSlots = p.MaxSlots,
                        Price = p.Price,
                        Name = p.Name,
                        IsUnlimited = p.IsUnlimited,
                        RequiredApproval = p.RequiredApproval
                    };
                }).ToList(),
                Dates = dateItems.Select(d => {
                    return new Framework.ApiCommand.ApiData.Activity.Request.CreateOteActivityArgs.OteDate {
                        Date = d.Date,
                        DateEnd = d.DateEnd,
                        DateStart = d.DateStart
                    };
                }).ToList(),
                DateOverrides = args.DateOverrides is not null ? 
                    args.DateOverrides.Select(d => {
                        return new Framework.ApiCommand.ApiData.Activity.Request.CreateOteActivityArgs.OteDateOverride {
                            Date = d.Date,
                            DateEnd = d.Date.Date.Add(d.TimeEnd),
                            DateStart = d.Date.Date.Add(d.TimeStart)
                        };
                    }).ToList() : null,
                OnlineEvents = args.OteOnlineEvents is not null ?
                    args.OteOnlineEvents.Select(s => {
                        return new Framework.ApiCommand.ApiData.Activity.Request.CreateOteActivityArgs.OteOnlineEvent
                        {
                            Title = s.Title,
                            Description = s.Description,
                            VideoLink = s.VideoLink,
                            TicketRestriction = s.TicketRestriction,
                        };
                    }).ToList() : null,
            });
            if(!createOteRes.Succeeded || createOteRes.Result is null || !createOteRes.Result.IsSuccess)
            {
                return AppResult<OteCreateResult>.CreateFailed(new ApplicationException(createOteRes.Message), createOteRes.Message);
            }

            // for custom questions
            if(args.Questions is not null && args.Questions.Count() > 0)
            {
                var questionsRes = args.Questions.Select(q => providerCustomQuestionData.CreateCustomQuestion(new Framework.ApiCommand.ApiData.ProviderCustomQuestion.Request.CreateCustomQuestionArgs {
                    ActivityId = createOteRes.Result.Result.Id,
                    FieldLabel = q.Question,
                    FieldType = q.FieldType,
                    ProviderId = currentUser.Result.Id,
                    Required = q.Required
                }));

                // dont check the result if error or success
                await Task.WhenAll(questionsRes);
            }
            
            var createReminderContent = saveEmailTemplateHandler.ExecuteAsync(new SaveEmailTemplateArgs {
                ActivityId = createOteRes.Result.Result.Id,
                Body = args.Activity.ReminderBody ?? string.Empty,
                ProviderId = currentUser.Result.Id,
                Subject = args.Activity.ReminderSubject ?? string.Empty,
                TemplateType = EmailTemplateType.OteReminder
            });

            var createFeedbackContent = saveEmailTemplateHandler.ExecuteAsync(new SaveEmailTemplateArgs {
                ActivityId = createOteRes.Result.Result.Id,
                Body = args.Activity.FeedbackBody ?? string.Empty,
                ProviderId = currentUser.Result.Id,
                Subject = args.Activity.FeedbackSubject ?? string.Empty,
                TemplateType = EmailTemplateType.OteThankYou
            });

            var createCustomPending = saveEmailTemplateHandler.ExecuteAsync(new SaveEmailTemplateArgs {
                ActivityId = createOteRes.Result.Result.Id,
                Body = args.Activity.CustomPendingBody ?? string.Empty,
                ProviderId = currentUser.Result.Id,
                Subject = string.Empty,
                TemplateType = EmailTemplateType.OtePending
            });

            var createCustomAccept = saveEmailTemplateHandler.ExecuteAsync(new SaveEmailTemplateArgs {
                ActivityId = createOteRes.Result.Result.Id,
                Body = args.Activity.CustomAcceptedBody ?? string.Empty,
                ProviderId = currentUser.Result.Id,
                Subject = string.Empty,
                TemplateType = EmailTemplateType.OteConfirmed
            });

            var createCustomDeclined = saveEmailTemplateHandler.ExecuteAsync(new SaveEmailTemplateArgs {
                ActivityId = createOteRes.Result.Result.Id,
                Body = args.Activity.CustomDeclinedBody ?? string.Empty,
                ProviderId = currentUser.Result.Id,
                Subject = string.Empty,
                TemplateType = EmailTemplateType.OteDeclined
            });

            // dont check the result if error or success
            await Task.WhenAll(createReminderContent, createFeedbackContent, createCustomPending, createCustomAccept, createCustomDeclined);

            // update customer status to maker
            if(!currentUser.Result.IsMaker)
            {
                var updateCustomerRes = await customerData.UpdateCustomer(new Framework.ApiCommand.ApiData.Customer.Request.UpdateCustomerArgs {
                    CustomerId = currentUser.Result.Id,
                    IsMaker = true
                });
                if(!updateCustomerRes.Succeeded || updateCustomerRes.Result is null || !updateCustomerRes.Result.IsSuccess)
                {
                    return AppResult<OteCreateResult>.CreateFailed(new ApplicationException(updateCustomerRes.Message), updateCustomerRes.Message);
                }
            }

            return AppResult<OteCreateResult>.CreateSucceeded(new OteCreateResult {Id = createOteRes.Result.Result.Id}, "One time event successfully created.");
        }
        catch (Exception ex)
        {
            return AppResult<OteCreateResult>.CreateFailed(ex, "An error occured in OteCreateHandler");
        }
    }

}