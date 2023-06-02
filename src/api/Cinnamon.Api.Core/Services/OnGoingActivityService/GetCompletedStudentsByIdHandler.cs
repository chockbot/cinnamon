using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService;
public class GetCompletedStudentsByIdHandler : IGetCompletedStudentsByIdHandler
{
    private readonly IStudentData studentData;

    public GetCompletedStudentsByIdHandler(IStudentData studentData)
    {
        this.studentData = studentData;
    }

    public AppResult<GetCompletedStudentsByIdResult> Execute(GetCompletedStudentsByIdArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetCompletedStudentsByIdResult>.CreateFailed(ex, "An error occured in GetCompletedStudentsByIdHandler");
        }
    }

    public async Task<AppResult<GetCompletedStudentsByIdResult>> ExecuteAsync(GetCompletedStudentsByIdArgs args)
    {
        try
        {
            var attendanceResReLoad = await studentData.GetCompletedStudentsById(new Framework.ApiCommand.ApiData.Student.Request.GetCompletedStudentsByIdArgs
            {
                CustomerId = args.CustomerId
            });
            if (!attendanceResReLoad.Succeeded || attendanceResReLoad.Result == null || !attendanceResReLoad.Result.IsSuccess)
            {
                return AppResult<GetCompletedStudentsByIdResult>.CreateFailed(new ApplicationException(attendanceResReLoad.Result?.ErrorInfo?.Message), attendanceResReLoad.Message);
            }
            return AppResult<GetCompletedStudentsByIdResult>.CreateSucceeded(new GetCompletedStudentsByIdResult
            {
                Student = attendanceResReLoad.Result.Result.Select(s =>
                {
                    return new GetCompletedStudentsByIdResult.Students
                    {
                        StudentId = s.Id,
                        ActivityId = s.ActivityId,
                        ScheduleId = s.ScheduleId,
                        NumberOfSessions = s.NumberOfSessions,
                        SessionsAttended = s.SessionsAttended,
                        StudentName = s.Name,
                    };
                })
            }, "Successfully get student attendance");

        }
        catch (Exception ex)
        {
            return AppResult<GetCompletedStudentsByIdResult>.CreateFailed(ex, "An error occured in GetCompletedStudentsByIdHandler");
        }
    }
}
