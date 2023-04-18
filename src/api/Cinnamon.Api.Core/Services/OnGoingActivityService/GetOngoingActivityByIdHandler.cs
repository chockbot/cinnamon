using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService;
public class GetOngoingActivityByIdHandler: IGetOngoingActivityByIdHandler
{
	private readonly IStudentData studentData;
	public GetOngoingActivityByIdHandler(IStudentData studentData)
	{
		this.studentData = studentData;
	}

    public AppResult<GetOngoingActivityByIdResult> Execute(GetOngoingActivityByIdArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetOngoingActivityByIdResult>.CreateFailed(ex, "An error occured in GetOngoingActivityByIdHandler");
        }
    }

    public async Task<AppResult<GetOngoingActivityByIdResult>> ExecuteAsync(GetOngoingActivityByIdArgs args)
    {
        try
        {
            var result = await studentData.GetStudentById(args.Id, args.ActivityId);
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetOngoingActivityByIdResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetOngoingActivityByIdResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetCustomerByIdHandler");
            }

            return AppResult<GetOngoingActivityByIdResult>.CreateSucceeded(new GetOngoingActivityByIdResult
            {
                ActivityId= result.Result.Result.ActivityId,
                CustomerId= result.Result.Result.CustomerId,
                Id= result.Result.Result.Id,
                Name= result.Result.Result.Name,
                NumberOfSessions = result.Result.Result.NumberOfSessions,
                Remarks = result.Result.Result.Remarks,
                ScheduleId = result.Result.Result.ScheduleId,
                SessionsAttended = result.Result.Result.SessionsAttended,
                NumberOfBacktracking = result.Result.Result.NumberOfBackTracking,
                Status = result.Result.Result.Status,
                StudentNo = result.Result.Result.StudentNo,
                ExpirationStartDate= result.Result.Result.ExpirationStartDate,
                ExpirationEndDate= result.Result.Result.ExpirationEndDate,  
            }, "Successfully getting student information");
        }
        catch (Exception ex)
        {
            return AppResult<GetOngoingActivityByIdResult>.CreateFailed(ex, "An error occured in GetOngoingActivityByIdHandler");
        }
    }
}
