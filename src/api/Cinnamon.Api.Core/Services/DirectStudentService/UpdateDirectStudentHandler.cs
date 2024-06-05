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

            if(args.BirthYear.HasValue && args.BirthYear.Value > minYear)
            {
                return AppResult<UpdateDirectStudentResult>.CreateFailed(
                    new ApplicationException("Students must be at least 4 years old."), "Students must be at least 4 years old.");
            }

            var directStudentRes = await directStudentHandler.ExecuteAsync(new DirectStudentArgs {
                StudentId = args.StudentId
            });
            if(!directStudentRes.Succeeded || directStudentRes.Result is null)
            {
                return AppResult<UpdateDirectStudentResult>.CreateFailed(new ApplicationException(directStudentRes.Message), directStudentRes.Message);
            }

            int? numberOfSessions = null;
            if(args.ActivityId.HasValue || args.ScheduleId.HasValue)
            {
                var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                    ActivityId = args.ActivityId ?? 0,
                    IncludeAtivitySchedules = true,
                    IncludeCustomer = true
                });
                if(!activityRes.Succeeded || activityRes.Result is null)
                {
                    return AppResult<UpdateDirectStudentResult>.CreateFailed( new ApplicationException(activityRes.Message), activityRes.Message);
                }

                var profleRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
                if(!profleRes.Succeeded || profleRes.Result is null)
                {
                    return AppResult<UpdateDirectStudentResult>.CreateFailed( new ApplicationException(profleRes.Message), profleRes.Message);
                }

                if(profleRes.Result.Id != activityRes.Result.Owner?.Id)
                {
                    return AppResult<UpdateDirectStudentResult>.CreateFailed( 
                        new ApplicationException("Action not allowed. Invalid request."), "Action not allowed. Invalid request.");
                }

                var schedule = activityRes.Result.ActivitySchedules.FirstOrDefault(s => s.Id == args.ScheduleId);
                if(schedule is null)
                {
                    return AppResult<UpdateDirectStudentResult>.CreateFailed( 
                        new ApplicationException("Action not allowed. Invalid request."), "Action not allowed. Invalid request.");
                }

                numberOfSessions = schedule.PerUnit2;
            }

                var result = await directStudentData.UpdateDirectStudent(new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs {
                UpdateDirectStudentData = new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudent {
                    UpdateDirectStudentInfo = new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudentInfo {
                        BirthMonth = args.BirthMonth,
                        BirthYear = args.BirthYear,
                        Gender = args.Gender,
                        Name = args.Name,
                        Id = args.StudentId
                    },
                    UpdateDirectStudentPayment = new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudentPayment {
                        Amount = args.Amount
                    },
                    UpdateDirectStudentSession = new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateDirectStudentArgs.UpdateDirectStudentSession {
                        ActivityId = args.ActivityId,
                        ScheduleId = args.ScheduleId,
                        Name = args.Name,
                        NumberOfSessions = numberOfSessions,
                        Remarks = args.Remarks,
                        StudentNo = args.StudentNo,
                    }
                }
            });

            if(!result.Succeeded || result.Result is null || !result.Result.IsSuccess)
            {
                return AppResult<UpdateDirectStudentResult>.CreateFailed(new ApplicationException(result.Result?.ErrorInfo?.Message), result.Message);
            }

            return AppResult<UpdateDirectStudentResult>.CreateSucceeded(new UpdateDirectStudentResult {
                ActivityId = args.ActivityId ?? 0,
                Amount = args.Amount ?? 0,
                BirthMonth = args.BirthMonth ?? string.Empty,
                BirthYear = args.BirthYear ?? 0,
                Gender = args.Gender ?? string.Empty,
                Id = args.StudentId,
                Name = args.Name ?? string.Empty,
                ScheduleId = args.ScheduleId ?? 0,
                Remarks = args.Remarks ?? string.Empty,
            }, "Successfully update student id");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateDirectStudentResult>.CreateFailed(ex, "An error occured in UpdateDirectStudentHandler.");
        }
    }
}