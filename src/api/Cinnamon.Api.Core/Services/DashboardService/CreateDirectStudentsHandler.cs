using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Helpers;

namespace Cinnamon.Api.Core.Services.DashboardService;

public class CreateDirectStudentsHandler : ICreateDirectStudentsHandler
{
    private readonly IDirectStudentData directStudentData;
    private readonly IMapper mapper;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IGetProfileHandler getProfileHandler;
    
    public CreateDirectStudentsHandler(IDirectStudentData directStudentData, IMapper mapper,
        IGetActivityHandler getActivityHandler, IGetProfileHandler getProfileHandler)
    {
        this.mapper            = mapper;
        this.directStudentData = directStudentData;
        this.getActivityHandler = getActivityHandler;
        this.getProfileHandler = getProfileHandler;
    }

    public AppResult<CreateDirectStudentsResult> Execute(CreateDirectStudentsArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CreateDirectStudentsResult>.CreateFailed(ex, "An error occured in CreateDirectStudentsHandler");
        }
    }

    public async Task<AppResult<CreateDirectStudentsResult>> ExecuteAsync(CreateDirectStudentsArgs args)
    {
        try
        {
            var minYear = DateTime.Now.Year - 4;

            if(args.CreateDirectStudents.Any(s => s.CreateDirectStudentInfo.BirthYear > minYear))
            {
                return AppResult<CreateDirectStudentsResult>.CreateFailed(
                    new ApplicationException("Students must be at least 4 years old."), "Students must be at least 4 years old.");
            }

            var profileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!profileRes.Succeeded || profileRes.Result is null)
            {
                return AppResult<CreateDirectStudentsResult>.CreateFailed(new ApplicationException(profileRes.Message), profileRes.Message);
            }
            var profile = profileRes.Result;

            Dictionary<int, ActivityService.Interactors.Results.GetActivityResult> fetchActivities = new ();
            List<Framework.ApiCommand.ApiData.DirectStudent.Request.CreateDirectStudentsArgs.CreateDirectStudent> students = new();

            foreach(var student in args.CreateDirectStudents)
            {
                if(!fetchActivities.ContainsKey(student.CreateDirectStudentSession.ActivityId))
                {
                    var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                        ActivityId = student.CreateDirectStudentSession.ActivityId,
                        IncludeAtivitySchedules = true,
                        IncludeCustomer = true
                    });
                    if(!activityRes.Succeeded || activityRes.Result is null)
                    {
                        return AppResult<CreateDirectStudentsResult>.CreateFailed(new ApplicationException(activityRes.Message), activityRes.Message);
                    }
                    fetchActivities.Add(student.CreateDirectStudentSession.ActivityId, activityRes.Result);
                }

                var activity = fetchActivities[student.CreateDirectStudentSession.ActivityId];
                var schedule = activity.ActivitySchedules.FirstOrDefault(s => s.Id == student.CreateDirectStudentSession.ScheduleId);

                if(schedule is null)
                {
                    return AppResult<CreateDirectStudentsResult>.CreateFailed(new ApplicationException("Invalid selected schedule."), "Invalid selected schedule.");
                }

                if(activity.Owner?.Id != profile.Id)
                {
                    return AppResult<CreateDirectStudentsResult>.CreateFailed(new ApplicationException("Invalid selected activity."), "Invalid selected activity.");
                }

                //Check if Schedule has Start Expiration Date
                DateTime endExpiration = DateTime.MinValue;
                DateTime startExpiration = schedule.StartDate ?? DateTime.MinValue;

                if(schedule.HasExpiration == 1 && schedule.IsSetSession && 
                    !(student.CreateDirectStudentSession.Period == "currentperiod" || student.CreateDirectStudentSession.Period == "nextperiod")) 
                {
                    return AppResult<CreateDirectStudentsResult>.CreateFailed(
                        new ApplicationException("Selected schedule has expired session. Please choose a valid period."), "Selected schedule has expired session. Please choose a valid period.");
                }

                if (schedule.HasExpiration == 1 && schedule.IsSetSession)
                {
                    var datePeriod = DateNextPeriod.CreateRecurring(startExpiration, schedule.SessionName);

                    if(student.CreateDirectStudentSession.Period == "currentperiod")
                    {
                        startExpiration = datePeriod.PeriodStart.Date;
                        endExpiration = datePeriod.PeriodEnd;
                    }
                    else if (student.CreateDirectStudentSession.Period == "nextperiod")
                    {
                        datePeriod.NextPeriod();
                        startExpiration = datePeriod.PeriodStart.Date;
                        endExpiration = datePeriod.PeriodEnd;
                    }
                }

                students.Add(new Framework.ApiCommand.ApiData.DirectStudent.Request.CreateDirectStudentsArgs.CreateDirectStudent {
                    CreateDirectStudentInfo = new Framework.ApiCommand.ApiData.DirectStudent.Request.CreateDirectStudentsArgs.CreateDirectStudentInfo {
                        BirthMonth = student.CreateDirectStudentInfo.BirthMonth,
                        BirthYear = student.CreateDirectStudentInfo.BirthYear,
                        Gender = student.CreateDirectStudentInfo.Gender,
                        Name = student.CreateDirectStudentInfo.Name,
                        ProviderId = profile.Id,
                        Id = student.CreateDirectStudentInfo.Id
                    },
                    CreateDirectStudentPayment = new Framework.ApiCommand.ApiData.DirectStudent.Request.CreateDirectStudentsArgs.CreateDirectStudentPayment {
                        Amount = student.CreateDirectStudentPayment.Amount
                    },
                    CreateDirectStudentSession = new Framework.ApiCommand.ApiData.DirectStudent.Request.CreateDirectStudentsArgs.CreateDirectStudentSession {
                        ActivityId = student.CreateDirectStudentSession.ActivityId,
                        ExpirationDateEnd = endExpiration,
                        ExpirationDateStart = startExpiration,
                        Name = student.CreateDirectStudentSession.Name,
                        NumberOfSessions = schedule.PerUnit2,
                        Remarks = student.CreateDirectStudentSession.Remarks ?? string.Empty,
                        ScheduleId = schedule.Id,
                        SessionsAttended = 0,
                        Status = "ACTIVE",
                        StudentNo = "00"
                    }
                });
            }

            var result = await directStudentData.CreateDirectStudents(new Framework.ApiCommand.ApiData.DirectStudent.Request.CreateDirectStudentsArgs
            {
                CreateDirectStudents = students
            });

            if (!result.Succeeded || result.Result == null || !result.Result.IsSuccess)
            {
                return AppResult<CreateDirectStudentsResult>.CreateFailed(new ApplicationException(result.Result?.ErrorInfo?.Message), result.Message);
            }
            
            var directStudentRes = mapper.Map<IEnumerable<CreateDirectStudentsResult.CreateDirectStudent>>(result.Result.Result);
            return AppResult<CreateDirectStudentsResult>.CreateSucceeded(
                new CreateDirectStudentsResult { CreateDirectStudents = directStudentRes}, "Successfully created direct students");
        }
        catch (Exception ex)
        {
            return AppResult<CreateDirectStudentsResult>.CreateFailed(ex, "An error occured in CreateDirectStudentsHandler");
        }
    }
}
