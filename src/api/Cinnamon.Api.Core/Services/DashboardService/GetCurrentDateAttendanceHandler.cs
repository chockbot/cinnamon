using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;

public class GetCurrentDateAttendanceHandler : IGetCurrentDateAttendanceHandler
{
    private readonly IGetStudentAttendanceHandler getStudentAttendanceHandler;

    public GetCurrentDateAttendanceHandler(IGetStudentAttendanceHandler getStudentAttendanceHandler)
    {
        this.getStudentAttendanceHandler = getStudentAttendanceHandler;
    }

    public AppResult<GetCurrentDateAttendanceResult> Execute(GetCurrentDateAttendanceArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetCurrentDateAttendanceResult>.CreateFailed(ex, "An error occured in GetCurrentDateAttendanceHandler");
        }
    }

    public async Task<AppResult<GetCurrentDateAttendanceResult>> ExecuteAsync(GetCurrentDateAttendanceArgs args)
    {
        try
        {
            var result = await getStudentAttendanceHandler.ExecuteAsync(new GetStudentAttendanceArgs {
                ActivityId = args.ActivityId,
                Date = DateTime.Now.Date,
                ScheduleId = args.ScheduleId,
                ForceCreate = true,
            });
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<GetCurrentDateAttendanceResult>.CreateFailed(result.Error.Exception, result.Message);
            }

            return AppResult<GetCurrentDateAttendanceResult>.CreateSucceeded(new GetCurrentDateAttendanceResult {
                StudentAttendaces = result.Result.StudentAttendaces.Where(s => s.SessionsAttended < s.NumberOfSessions).Select(s => {
                    return new GetCurrentDateAttendanceResult.StudentAttendace {
                        ActivityDescription = s.ActivityDescription,
                        ActivityId = s.ActivityId,
                        ActivityTitle = s.ActivityTitle,
                        IsPresent = s.IsPresent,
                        ScheduleDescription = s.ScheduleDescription,
                        ScheduleId = s.ScheduleId,
                        ScheduleTitle = s.ScheduleTitle,
                        StudentId = s.StudentId,
                        NumberOfSessions = s.NumberOfSessions,
                        SessionsAttended = s.SessionsAttended,
                        Status = s.Status,
                        StudentName = s.StudentName,
                        StudentNo = s.StudentNo,
                        AttendanceDate = s.AttendanceDate,
                        Id = s.Id,
                        Remarks = s.Remarks,
                        ExpirationDateEnd = s.ExpirationDateEnd,
                        ExpirationDateStart = s.ExpirationDateStart,
                        StudentType = s.StudentType
                    };
                })
            }, "Successfully get current date student attendance");
        }
        catch (Exception ex)
        {
            return AppResult<GetCurrentDateAttendanceResult>.CreateFailed(ex, "An error occured in GetCurrentDateAttendanceHandler");
        }
    }
}