using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService;
public class GetEnrolledStudentsHandler: IGetEnrolledStudentsHandler
{
    private readonly IStudentData studentData;
    public GetEnrolledStudentsHandler(IStudentData studentData)
    {
        this.studentData = studentData;
    }

    public AppResult<GetEnrolledStudentsResult> Execute(GetEnrolledStudentsArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetEnrolledStudentsResult>.CreateFailed(ex, "An error occured in GetEnrolledStudentsHandler");
        }
    }

    public async Task<AppResult<GetEnrolledStudentsResult>> ExecuteAsync(GetEnrolledStudentsArgs args)
    {
        try
        {
            var result = await studentData.GetEnrolledStudents(args.ActivityId);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetEnrolledStudentsResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetEnrolledStudentsResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetEnrolledStudentsHandler");
            }
            return AppResult<GetEnrolledStudentsResult>.CreateSucceeded(new GetEnrolledStudentsResult
            {
                Students = result.Result.Result.Select(e =>
                {
                    return new GetEnrolledStudentsResult.Student
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
                        ExpirationEndDate = e.ExpirationEndDate
                    };
                })
            }, "Successfully get enrolled students");
        }
        catch (Exception ex)
        {
            return AppResult<GetEnrolledStudentsResult>.CreateFailed(ex, "An error occured in GetEnrolledStudentsHandler");
        }
    }
}
