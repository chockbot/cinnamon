using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;
public class GetCompletedStudentsHandler: IGetCompletedStudentsHandler
{
    private readonly IStudentAttendanceData studentAttendanceData;
    private readonly IStudentData studentData;

    public GetCompletedStudentsHandler(IStudentAttendanceData studentAttendanceData, IStudentData studentData)
    {
        this.studentAttendanceData = studentAttendanceData;
        this.studentData = studentData;
    }

    public AppResult<GetCompletedStudentsResult> Execute(GetCompletedStudentsArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetCompletedStudentsResult>.CreateFailed(ex, "An error occured in GetCompletedStudentsHandler");
        }
    }

    public async Task<AppResult<GetCompletedStudentsResult>> ExecuteAsync(GetCompletedStudentsArgs args)
    {
        try
        {
            var attendanceResReLoad = await studentAttendanceData.GetCompletedStudents(new Framework.ApiCommand.ApiData.StudentAttendance.Request.GetCompletedStudentsArgs
            {
                ActivityIds = args.ActivityId,
                IsIncludeStudent = true,
            });
            if (!attendanceResReLoad.Succeeded || attendanceResReLoad.Result == null || !attendanceResReLoad.Result.IsSuccess)
            {
                return AppResult<GetCompletedStudentsResult>.CreateFailed(new ApplicationException(attendanceResReLoad.Result?.ErrorInfo?.Message), attendanceResReLoad.Message);
            }
            return AppResult<GetCompletedStudentsResult>.CreateSucceeded(new GetCompletedStudentsResult
            {
                StudentAttendaces = attendanceResReLoad.Result.Result.Select(s => {
                    return new GetCompletedStudentsResult.StudentAttendace
                    {
                        IsPresent = s.IsPresent,
                        StudentId = s.StudentId,
                        NumberOfSessions = s.Student.NumberOfSessions,
                        SessionsAttended = s.Student.SessionsAttended,
                        Status = s.Student.Status,
                        StudentName = s.Student.Name,
                        StudentNo = s.Student.StudentNo,
                        AttendanceDate = s.Date,
                        Id = s.Id,
                        Remarks = s.Student.Remarks,
                        ActivityId = s.Student.ActivityId,
                        ScheduleId = s.Student.ScheduleId
                    };
                })
            }, "Successfullt get student attendance");

        }
        catch (Exception ex)
        {
            return AppResult<GetCompletedStudentsResult>.CreateFailed(ex, "An error occured in GetCompletedStudentsHandler");
        }
    }
}
