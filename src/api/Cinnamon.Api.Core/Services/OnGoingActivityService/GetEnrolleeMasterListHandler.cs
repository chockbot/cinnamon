using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService;

public class GetEnrolleeMasterListHandler : IGetEnrolleeMasterListHandler
{
    private readonly IStudentData studentData;
    public GetEnrolleeMasterListHandler(IStudentData studentData)
    {
        this.studentData = studentData;
    }

    public AppResult<GetEnrolleeMasterListResult> Execute(GetEnrolleeMasterListArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetEnrolleeMasterListResult>.CreateFailed(ex, "An error occurred in GetEnrolleeMasterListHandler");
        }
    }

    public async Task<AppResult<GetEnrolleeMasterListResult>> ExecuteAsync(GetEnrolleeMasterListArgs args)
    {
        try
        {
            var result = await studentData.GetEnrolleeMasters(new Framework.ApiCommand.ApiData.Student.Request.GetEnrolleeMasterListArgs
            {
                CountPerPage = args.CountPerPage,
                PageIndex    = args.PageIndex,
                ProviderId   = args.ProviderId
            });
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetEnrolleeMasterListResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetEnrolleeMasterListResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetEnrolledStudentsHandler");
            }
            return AppResult<GetEnrolleeMasterListResult>.CreateSucceeded(new GetEnrolleeMasterListResult
            {
                EnrolleeMasterLists = result.Result.Result.Select(e =>
                {
                    return new GetEnrolleeMasterListResult.EnrolleeMasterList
                    {
                        Id               = e.Id,
                        ActivityId       = e.ActivityId,
                        CustomerId       = e.CustomerId,
                        Name             = e.Name,
                        Remarks          = e.Remarks,
                        ScheduleId       = e.ScheduleId,
                        SessionsAttended = e.SessionsAttended,
                        NumberOfSessions = e.NumberOfSessions,
                        Status           = e.Status,
                        StudentNo        = e.StudentNo,
                        Age              = e.Age,
                        ActivityName     = e.ActivityTitle,
                        Email            = e.Email,
                        Gender           = e.Gender  
                    };
                }),
                ErrorInfo = new Framework.ApiCommand.ApiCore.ErrorInfo
                {
                    Code        = result?.Result?.ErrorInfo?.Code,
                    Description = result?.Result?.ErrorInfo?.Description,
                    Message     = result?.Result?.ErrorInfo?.Message
                },
                Pagination = new Framework.ApiCommand.ApiCore.Pagination
                {
                    PageIndex    = result.Result.Pagination.PageIndex,
                    PerPage      = result.Result.Pagination.PerPage,
                    TotalPages   = result.Result.Pagination.TotalPages,
                    TotalRecords = result.Result.Pagination.TotalRecords
                }
            }, "Successfully get enrolled students");
        }
        catch (Exception ex)
        {
            return AppResult<GetEnrolleeMasterListResult>.CreateFailed(ex, "An error occured in GetEnrolledStudentsHandler");
        }
    }
}
