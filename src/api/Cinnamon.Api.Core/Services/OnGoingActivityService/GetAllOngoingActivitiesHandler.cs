using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService;

public class GetAllOngoingActivitiesHandler: IGetAllOngoingActivitiesHandler
{
	private readonly IStudentData studentData;
	public GetAllOngoingActivitiesHandler(IStudentData studentData)
	{
		this.studentData = studentData;
	}

    public AppResult<GetAllOngoingActivitiesResult> Execute(GetAllOngoingActivitiesArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetAllOngoingActivitiesResult>.CreateFailed(ex, "An error occured in GetAllOngoingActivitiesHandler");
        }
    }

    public async Task<AppResult<GetAllOngoingActivitiesResult>> ExecuteAsync(GetAllOngoingActivitiesArgs args)
    {
        try
        {
            var result = await studentData.GetAllStudents(new Framework.ApiCommand.ApiData.Student.Request.GetAllStudentArgs { });
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetAllOngoingActivitiesResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetAllOngoingActivitiesResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetAllOngoingActivitiesHandler");
            }
            return AppResult<GetAllOngoingActivitiesResult>.CreateSucceeded(new GetAllOngoingActivitiesResult
            {
                Students = result.Result.Result.Select(e =>
                {
                    return new GetAllOngoingActivitiesResult.Student
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
                        ExpirationStartDate= e.ExpirationStartDate,
                        ExpirationEndDate= e.ExpirationEndDate
                    };
                })
            }, "Successfully get students");
        }
        catch (Exception ex)
        {
            return AppResult<GetAllOngoingActivitiesResult>.CreateFailed(ex, "An error occured in GetAllOngoingActivitiesHandler");
        }
    }
}

