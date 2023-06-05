using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService;

public class UpdateOngoingActivityHandler: IUpdateOngoingActivityHadler
{
    private readonly IStudentData studentData;
	public UpdateOngoingActivityHandler(IStudentData studentData)
	{
		this.studentData = studentData;
	}

    public AppResult<UpdateOnGoingActivityResult> Execute(UpdateOngoingActivityArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateOnGoingActivityResult>.CreateFailed(ex, "An error occured in UpdateOngoingActivityHandler");
        }
    }

    public async Task<AppResult<UpdateOnGoingActivityResult>> ExecuteAsync(UpdateOngoingActivityArgs args)
    {
        try
        {
            var updated = await studentData.UpdateStudent(new Framework.ApiCommand.ApiData.Student.Request.UpdateStudentArgs
            {
                SessionsAttended = args.SessionsAttended,
                Remarks = args.Remarks,
                Name = args.Name,
                NumberOfSessions = args.NumberOfSessions,
                NumberOfBacktracking = args.NumberOfBacktracking,
                Status = args.Status,
                StudentNo = args.StudentNo,
                StudentId = args.Id,
                ExpirationStartDate = args.ExpirationStartDate,
                ExpirationEndDate = args.ExpirationEndDate,
                HasReview = args.HasReview
            });
            if (!updated.Succeeded || updated.Result == null || !updated.Result.IsSuccess)
            {
                return AppResult<UpdateOnGoingActivityResult>.CreateFailed(new ApplicationException(updated.Result?.ErrorInfo?.Message), updated.Message);
            }
            if (updated.Succeeded && !updated.Result.IsSuccess)
            {
                return AppResult<UpdateOnGoingActivityResult>.CreateFailed(
                    new ApplicationException(updated.Result.ErrorInfo?.Message), "An error occured in UpdateOngoingActivityHandler");
            }
            return AppResult<UpdateOnGoingActivityResult>.CreateSucceeded(new UpdateOnGoingActivityResult
            {
                Id = updated.Result.Result.Id,
                Name = updated.Result.Result.Name,
                NumberOfSessions = updated.Result.Result.NumberOfSessions,
                Remarks = updated.Result.Result.Remarks,
                SessionsAttended = updated.Result.Result.SessionsAttended,
                Status= updated.Result.Result.Status,
                StudentNo = updated.Result.Result.StudentNo,
                ExpirationStartDate = updated.Result.Result.ExpirationStartDate,
                ExpirationEndDate = updated.Result.Result.ExpirationEndDate,
                HasReview = updated.Result.Result.HasReview  
            }, "Successfully update student ongoing activity");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateOnGoingActivityResult>.CreateFailed(ex, "An error occured in UpdateOngoingActivityHandler");
        }
    }
}
