using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService;
public class GetAllStudentsByIdHandler : IGetAllStudentsByIdHandler
{
    private readonly IStudentData studentData;
    public GetAllStudentsByIdHandler(IStudentData studentData)
    {
        this.studentData = studentData;
    }

    public AppResult<GetAllStudentsByIdResult> Execute(GetAllStudentsByIdArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetAllStudentsByIdResult>.CreateFailed(ex, "An error occured in GetAllStudentsByIdHandler");
        }
    }

    public async Task<AppResult<GetAllStudentsByIdResult>> ExecuteAsync(GetAllStudentsByIdArgs args)
    {
        try
        {
            var attendanceResReLoad = await studentData.GetAllStudentsById(new Framework.ApiCommand.ApiData.Student.Request.GetAllStudentsByIdArgs
            {
                CustomerId = args.CustomerId
            });
            if (!attendanceResReLoad.Succeeded || attendanceResReLoad.Result == null || !attendanceResReLoad.Result.IsSuccess)
            {
                return AppResult<GetAllStudentsByIdResult>.CreateFailed(new ApplicationException(attendanceResReLoad.Result?.ErrorInfo?.Message), attendanceResReLoad.Message);
            }
            return AppResult<GetAllStudentsByIdResult>.CreateSucceeded(new GetAllStudentsByIdResult
            {
                Student = attendanceResReLoad.Result.Result.Select(e =>
                {
                    return new GetAllStudentsByIdResult.Students
                    {
                        Id = e.Id,
                        ActivityId = e.ActivityId,
                        CustomerId = e.CustomerId,
                        Name = e.Name,
                        NumberOfSessions = e.NumberOfSessions,
                        Remarks = e.Remarks,
                        ScheduleId = e.ScheduleId,
                        SessionsAttended = e.SessionsAttended,
                        Status = e.Status,
                        StudentNo = e.StudentNo,
                        ExpirationStartDate = e.ExpirationStartDate,
                        ExpirationEndDate = e.ExpirationEndDate,
                        HasReview = e.HasReview
                    };
                })
            }, "Successfully get student attendance");

        }
        catch (Exception ex)
        {
            return AppResult<GetAllStudentsByIdResult>.CreateFailed(ex, "An error occured in GetCompletedStudentsByIdHandler");
        }
    }
}
