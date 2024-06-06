using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Result;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DirectStudentService;

public class GetDirectStudentByIdHandler : IGetDirectStudentByIdHandler
{
    private readonly IDirectStudentData directStudentData;
    public GetDirectStudentByIdHandler(IDirectStudentData directStudentData)
    {
        this.directStudentData = directStudentData;
    }

    public AppResult<GetDirectStudentByIdResult> Execute(GetDirectStudentByIdArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetDirectStudentByIdResult>.CreateFailed(ex, "An error occured in GetDirectStudentByIdHandler");
        }
    }

    public async Task<AppResult<GetDirectStudentByIdResult>> ExecuteAsync(GetDirectStudentByIdArgs args)
    {
        try
        {
            var attendance = await directStudentData.GetDirectStudentsById(new Framework.ApiCommand.ApiData.DirectStudent.Request.GetDirectStudentByIdArgs
            {
                IncludeStudent = true,
                StudentId = args.StudentId,
                ScheduleIds = new int[] { args.ScheduleId },
                ActivityIds = new int[] { args.ActivityId }
            });
            if (!attendance.Succeeded || attendance.Result == null || !attendance.Result.IsSuccess)
            {
                return AppResult<GetDirectStudentByIdResult>.CreateFailed(new ApplicationException(attendance.Result?.ErrorInfo?.Message), attendance.Message);
            }
            return AppResult<GetDirectStudentByIdResult>.CreateSucceeded(new GetDirectStudentByIdResult
            {
                StudentAttendaces = attendance.Result.Result.Select(s => {
                    return new GetDirectStudentByIdResult.StudentAttendace
                    {
                        IsPresent            = s.IsPresent,
                        StudentId            = s.StudentId,
                        NumberOfSessions     = s.Student.NumberOfSessions,
                        SessionsAttended     = s.Student.SessionsAttended,
                        Status               = s.Student.Status,
                        StudentName          = s.Student.Name,
                        StudentNo            = s.Student.StudentNo,
                        ScheduleId           = s.Student.ScheduleId,
                        AttendanceDate       = s.Date,
                        Id                   = s.Id,
                        Remarks              = s.Student.Remarks,
                        ActivityId           = s.Student.ActivityId,
                    };
                })
            }, "Successfullt get student attendance");
        }
        catch (Exception ex)
        {
            return AppResult<GetDirectStudentByIdResult>.CreateFailed(ex, "An error occured in GetDirectStudentByIdHandler");
        }
    }
}
