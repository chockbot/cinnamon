using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService;

public class GetStudentLastAttendanceHandler : IGetStudentLastAttendanceHandler
{
    private readonly IStudentAttendanceData studentAttendanceData;

    public GetStudentLastAttendanceHandler(IStudentAttendanceData studentAttendanceData)
    {
        this.studentAttendanceData = studentAttendanceData;
    }

    public AppResult<GetStudentLastAttendanceResult> Execute(GetStudentLastAttendanceArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetStudentLastAttendanceResult>.CreateFailed(ex, "An error occured in GetStudentLastAttendanceHandler");
        }
    }

    public async Task<AppResult<GetStudentLastAttendanceResult>> ExecuteAsync(GetStudentLastAttendanceArgs args)
    {
        try
        {
            var attendanceResReLoad = await studentAttendanceData.GetStudentLastAttendance(new Framework.ApiCommand.ApiData.StudentAttendance.Request.GetStudentLastAttendanceArgs
            {
                Id = args.Id,
                ActivityId = args.ActivityId,
                ScheduleId = args.ScheduleId
            });

            if (!attendanceResReLoad.Succeeded || attendanceResReLoad.Result == null || !attendanceResReLoad.Result.IsSuccess)
            {
                return AppResult<GetStudentLastAttendanceResult>.CreateFailed(new ApplicationException(attendanceResReLoad.Result?.ErrorInfo?.Message), attendanceResReLoad.Message);
            }

            if (attendanceResReLoad.Succeeded && !attendanceResReLoad.Result.IsSuccess)
            {
                return AppResult<GetStudentLastAttendanceResult>.CreateFailed(
                    new ApplicationException(attendanceResReLoad.Result.ErrorInfo?.Message), "An error occured in GetStudentLastAttendanceHandler");
            }

            var attendance = attendanceResReLoad.Result.Result;

            var attendanceEntity = new GetStudentLastAttendanceResult
            {
                Id              = attendance.Id,
                StudentId       = attendance.StudentId,
                AttendanceDate  = attendance.Date,
                IsPresent       = attendance.IsPresent,
                ScheduleId      = attendance.Student.ScheduleId,
                ActivityId      = attendance.Student.ActivityId
            };

            return AppResult<GetStudentLastAttendanceResult>.CreateSucceeded(attendanceEntity, "Successfully get activity");
        }
        catch (Exception ex)
        {
            return AppResult<GetStudentLastAttendanceResult>.CreateFailed(ex, "An error occured in GetCompletedStudentsByIdHandler");
        }
    }
}
