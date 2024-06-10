using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;
using Cinnamon.Framework.Common;

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

                    session.NumberOfSessions = schedule.PerUnit2;
                }
            }

            var result = await directStudentData.UpdateDirectStudent(new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs {
                UpdateStudentInfo = new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudentInfo {
                    BirthMonth = args.DirectStudentInfo.BirthMonth,
                    BirthYear = args.DirectStudentInfo.BirthYear,
                    Gender = args.DirectStudentInfo.Gender,
                    Id = args.DirectStudentInfo.StudentId,
                    Name = args.DirectStudentInfo.Name
                },
                UpdateStudentSessions = args.DirectStudentSessions.Select(s => new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudentSession {
                    ActivityId = s.ActivityId,
                    Id = s.Id,
                    Name = s.Name,
                    NumberOfSessions = s.NumberOfSessions,
                    Remarks = s.Remarks,
                    ScheduleId = s.ScheduleId,
                    StudentNo = s.StudentNo,
                    DirectStudentPayment = new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudentPayment {
                        Amount = s.StudentPayment?.Amount,
                    },
                })
            });

            if(!result.Succeeded || result.Result is null || !result.Result.IsSuccess)
            {
                return AppResult<UpdateDirectStudentResult>.CreateFailed(new ApplicationException(result.Result?.ErrorInfo?.Message), result.Message);
            }

            return AppResult<UpdateDirectStudentResult>.CreateSucceeded(new UpdateDirectStudentResult {
                BirthMonth = args.DirectStudentInfo.BirthMonth ?? string.Empty,
                BirthYear = args.DirectStudentInfo.BirthYear ?? 0,
                Gender = args.DirectStudentInfo.Gender ?? string.Empty,
                Id = args.DirectStudentInfo.StudentId,
                Name = args.DirectStudentInfo.Name ?? string.Empty,
            }, "Successfully update student id");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateDirectStudentResult>.CreateFailed(ex, "An error occured in UpdateDirectStudentHandler.");
        }
    }
}