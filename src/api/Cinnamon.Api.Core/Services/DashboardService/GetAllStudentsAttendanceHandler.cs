using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;
public class GetAllStudentsAttendanceHandler: IGetAllStudentsAttendanceHandler
{
    private readonly IStudentAttendanceData studentAttendanceData;
    private readonly IStudentData studentData;
    public GetAllStudentsAttendanceHandler(IStudentAttendanceData studentAttendanceData,IStudentData studentData)
    {
        this.studentAttendanceData = studentAttendanceData;
        this.studentData = studentData;
    }

    public AppResult<GetAllStudentsAttendanceResult> Execute(GetAllStudentsAttendanceArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetAllStudentsAttendanceResult>.CreateFailed(ex, "An error occured in GetAllStudentAttendanceHandler");
        }
    }

    public async Task<AppResult<GetAllStudentsAttendanceResult>> ExecuteAsync(GetAllStudentsAttendanceArgs args)
    {
        try
        {
            var attendanceResReLoad = await studentAttendanceData.GetAllStudentAttendance(new Framework.ApiCommand.ApiData.StudentAttendance.Request.GetAllStudentAttendanceArgs
            {
                ActivityIds = new int[] { args.ActivityId },
                IsIncludeStudent = true,
            });
            if (!attendanceResReLoad.Succeeded || attendanceResReLoad.Result == null || !attendanceResReLoad.Result.IsSuccess)
            {
                return AppResult<GetAllStudentsAttendanceResult>.CreateFailed(new ApplicationException(attendanceResReLoad.Result?.ErrorInfo?.Message), attendanceResReLoad.Message);
            }

            return AppResult<GetAllStudentsAttendanceResult>.CreateSucceeded(new GetAllStudentsAttendanceResult
            {
                StudentAttendaces = attendanceResReLoad.Result.Result.Select(s =>
                {
                    return new GetAllStudentsAttendanceResult.StudentAttendace
                    {
                        Id = s.Id,
                        IsPresent = s.IsPresent,
                        NumberOfSessions = s.Student.NumberOfSessions,
                        SessionsAttended = s.Student.SessionsAttended,
                        StudentId = s.StudentId,
                        AttendanceDate = s.Date,
                    };
                })
            }, "Successfully get student attendance");
        }
        catch (Exception ex)
        {
            return AppResult<GetAllStudentsAttendanceResult>.CreateFailed(ex, "An error occured in GetAllStudentAttendanceHandler");
        }
    }
}
