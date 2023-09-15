using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService;

public class GetAttendanceByFamilyIdHandler : IGetAttendanceByFamilyIdHandler
{
    private readonly IStudentAttendanceData studentAttendanceData;
    public GetAttendanceByFamilyIdHandler(IStudentAttendanceData studentAttendanceData)
    {
        this.studentAttendanceData = studentAttendanceData;
    }

    public AppResult<GetAttendanceByFamilyIdResult> Execute(GetAttendanceByFamilyIdArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetAttendanceByFamilyIdResult>.CreateFailed(ex, "An error occured in GetAttendanceByFamilyIdHandler");
        }
    }

    public async Task<AppResult<GetAttendanceByFamilyIdResult>> ExecuteAsync(GetAttendanceByFamilyIdArgs args)
    {
        try
        {
            var attendanceResReLoad = await studentAttendanceData.GetStudentAttendanceByFamilyId(new Framework.ApiCommand.ApiData.StudentAttendance.Request.GetStudentAttendanceByFamilyIdArgs
            {
                familyId = args.FamilyId
            });
            if (!attendanceResReLoad.Succeeded || attendanceResReLoad.Result == null || !attendanceResReLoad.Result.IsSuccess)
            {
                return AppResult<GetAttendanceByFamilyIdResult>.CreateFailed(new ApplicationException(attendanceResReLoad.Result?.ErrorInfo?.Message), attendanceResReLoad.Message);
            }
            return AppResult<GetAttendanceByFamilyIdResult>.CreateSucceeded(new GetAttendanceByFamilyIdResult
            {
                studentAttendaces = attendanceResReLoad.Result.Result.Select(s =>
                {
                    return new GetAttendanceByFamilyIdResult.StudentAttendace
                    {
                        Id               = s.Student.Id,
                        AttendanceDate   = s.Date,
                        IsPresent        = s.IsPresent,
                        ActivityId       = s.Student.ActivityId,
                        CustomerId       = s.Student.CustomerId,
                        FamilyId         = s.Student.FamilyMemberId,
                        ScheduleId       = s.Student.ScheduleId,
                        NumberOfSessions = s.Student.NumberOfSessions,
                        SessionsAttended = s.Student.SessionsAttended,
                        StudentName      = s.Student.Name 
                    };
                })
            }, "Successfully get student attendance");

        }
        catch (Exception ex)
        {
            return AppResult<GetAttendanceByFamilyIdResult>.CreateFailed(ex, "An error occured in GetCompletedStudentsByIdHandler");
        }
    }
}
