using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Helpers;

namespace Cinnamon.Api.Core.Services.DirectStudentService;

public class UpdateDirectStudentHandler : IUpdateDirectStudentHandler
{
    private readonly IDirectStudentData directStudentData;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IDirectStudentHandler directStudentHandler;
    private readonly IGetActivityHandler getActivityHandler;

    public UpdateDirectStudentHandler(IGetProfileHandler getProfileHandler, IDirectStudentData directStudentData,
        IDirectStudentHandler directStudentHandler, IGetActivityHandler getActivityHandler)
    {
        this.directStudentData = directStudentData;
        this.getProfileHandler = getProfileHandler;
        this.directStudentHandler = directStudentHandler;
        this.getActivityHandler = getActivityHandler;
    }
    
    public AppResult<UpdateDirectStudentResult> Execute(UpdateDirectStudentArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<UpdateDirectStudentResult>> ExecuteAsync(UpdateDirectStudentArgs args)
    {
        try
        {
            if(args.DirectStudentInfo is not null)
            {
                var minYear = DateTime.Now.Year - 4;

                if(args.DirectStudentInfo.BirthYear.HasValue && args.DirectStudentInfo.BirthYear.Value > minYear)
                {
                    return AppResult<UpdateDirectStudentResult>.CreateFailed(
                        new ApplicationException("Students must be at least 4 years old."), "Students must be at least 4 years old.");
                }

                var directStudentRes = await directStudentHandler.ExecuteAsync(new DirectStudentArgs {
                    StudentId = args.DirectStudentInfo.StudentId
                });
                if(!directStudentRes.Succeeded || directStudentRes.Result is null)
                {
                    return AppResult<UpdateDirectStudentResult>.CreateFailed(new ApplicationException(directStudentRes.Message), directStudentRes.Message);
                }
            }

            var studentSesssions = new List<Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudentSession>();
            if(args.DirectStudentSessions is not null)
            {
                var profleRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
                if(!profleRes.Succeeded || profleRes.Result is null)
                {
                    return AppResult<UpdateDirectStudentResult>.CreateFailed( new ApplicationException(profleRes.Message), profleRes.Message);
                }
                var profile = profleRes.Result;

                Dictionary<int, ActivityService.Interactors.Results.GetActivityResult> fetchActivities = new ();

                foreach (var session in args.DirectStudentSessions)
                {
                    if(session.ActivityId.HasValue && session.ScheduleId.HasValue)
                    {
                        if(!fetchActivities.ContainsKey(session.ActivityId.Value))
                        {
                            var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                                ActivityId = session.ActivityId ?? 0,
                                IncludeAtivitySchedules = true,
                                IncludeCustomer = true
                            });
                            if(!activityRes.Succeeded || activityRes.Result is null)
                            {
                                return AppResult<UpdateDirectStudentResult>.CreateFailed( new ApplicationException(activityRes.Message), activityRes.Message);
                            }

                            fetchActivities.Add(session.ActivityId.Value, activityRes.Result);
                        }

                        var activity = fetchActivities[session.ActivityId.Value];

                        if(profile.Id != activity.Owner?.Id)
                        {
                            return AppResult<UpdateDirectStudentResult>.CreateFailed( 
                                new ApplicationException("Action not allowed. Invalid request."), "Action not allowed. Invalid request.");
                        }

                        var schedule = activity.ActivitySchedules.FirstOrDefault(s => s.Id == session.ScheduleId);
                        if(schedule is null)
                        {
                            return AppResult<UpdateDirectStudentResult>.CreateFailed( 
                                new ApplicationException("Action not allowed. Invalid request."), "Action not allowed. Invalid request.");
                        }

                        // skip schedule expired computation that has expiration but don't have selected period
                        if(schedule.HasExpiration == 1 && string.IsNullOrEmpty(session.Period))
                        {
                            studentSesssions.Add(new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudentSession {
                                ActivityId = session.ActivityId,
                                Id = session.Id,
                                Name = session.Name,
                                NumberOfSessions = schedule.PerUnit2,
                                Remarks = session.Remarks,
                                ScheduleId = session.ScheduleId,
                                StudentNo = session.StudentNo,
                                SessionsAttended = session.SessionsAttended,
                                DirectStudentPayment = session.StudentPayment is null ? null : new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudentPayment {
                                    Amount = session.StudentPayment.Amount,
                                    PaymentDate = session.StudentPayment.PaymentDate
                                }
                            });
                            continue;
                        }

                        //Check if Schedule has Start Expiration Date
                        DateTime endExpiration = DateTime.MinValue;
                        DateTime startExpiration = schedule.StartDate ?? DateTime.MinValue;

                        if(schedule.HasExpiration == 1 && schedule.IsSetSession && 
                            !(session.Period == "currentperiod" || session.Period == "nextperiod")) 
                        {
                            return AppResult<UpdateDirectStudentResult>.CreateFailed(
                                new ApplicationException("Selected schedule has expired session. Please choose a valid period."), "Selected schedule has expired session. Please choose a valid period.");
                        }

                        if (schedule.HasExpiration == 1 && schedule.IsSetSession)
                        {
                            var datePeriod = DateNextPeriod.CreateRecurring(startExpiration, schedule.SessionName);

                            if(session.Period == "currentperiod")
                            {
                                startExpiration = datePeriod.PeriodStart.Date;
                                endExpiration = datePeriod.PeriodEnd;
                            }
                            else if (session.Period == "nextperiod")
                            {
                                datePeriod.NextPeriod();
                                startExpiration = datePeriod.PeriodStart.Date;
                                endExpiration = datePeriod.PeriodEnd;
                            }
                        }

                        studentSesssions.Add(new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudentSession {
                            ActivityId = session.ActivityId,
                            Id = session.Id,
                            Name = session.Name,
                            NumberOfSessions = schedule.PerUnit2,
                            Remarks = session.Remarks,
                            ScheduleId = session.ScheduleId,
                            SessionsAttended = session.SessionsAttended,
                            StudentNo = session.StudentNo,
                            ExpirationDateEnd = endExpiration,
                            ExpirationDateStart = startExpiration,
                            DirectStudentPayment = session.StudentPayment is null ? null : new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudentPayment {
                                Amount = session.StudentPayment.Amount,
                                PaymentDate = session.StudentPayment.PaymentDate
                            }
                        });
                    }
                }
            }

            var result = await directStudentData.UpdateDirectStudent(new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs {
                UpdateStudentInfo = args.DirectStudentInfo is null ? null : new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudentInfo {
                    BirthMonth = args.DirectStudentInfo.BirthMonth,
                    BirthYear = args.DirectStudentInfo.BirthYear,
                    Gender = args.DirectStudentInfo.Gender,
                    Id = args.DirectStudentInfo.StudentId,
                    Name = args.DirectStudentInfo.Name,
                    Email = args.DirectStudentInfo.Email
                },
                UpdateStudentSessions = studentSesssions
            });

            if(!result.Succeeded || result.Result is null || !result.Result.IsSuccess)
            {
                return AppResult<UpdateDirectStudentResult>.CreateFailed(new ApplicationException(result.Result?.ErrorInfo?.Message), result.Message);
            }

            return AppResult<UpdateDirectStudentResult>.CreateSucceeded(new UpdateDirectStudentResult {}, "Successfully update student id");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateDirectStudentResult>.CreateFailed(ex, "An error occured in UpdateDirectStudentHandler.");
        }
    }
}