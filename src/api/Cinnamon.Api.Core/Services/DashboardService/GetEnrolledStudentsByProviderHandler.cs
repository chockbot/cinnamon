using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;

public class GetEnrolledStudentsByProviderHandler : IGetEnrolledStudentsByProviderHandler
{
    private readonly IStudentData studentData;
    public GetEnrolledStudentsByProviderHandler(IStudentData studentData)
    {
        this.studentData = studentData;
    }

    public AppResult<GetEnrolledStudentsByProviderResult> Execute(GetEnrolledStudentsByProviderArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetEnrolledStudentsByProviderResult>.CreateFailed(ex, "An error occured in GetEnrolledStudentsByProviderHandler");
        }
    }

    public async Task<AppResult<GetEnrolledStudentsByProviderResult>> ExecuteAsync(GetEnrolledStudentsByProviderArgs args)
    {
        try
        {
            var result = await studentData.GetEnrolledStudentsByProvider(new Framework.ApiCommand.ApiData.Student.Request.GetEnrolledStudentsArgs
            {
                CountPerPage = args.CountPerPage,
                PageIndex    = args.PageIndex,
                ProviderId   = args.ProviderId,
                SearchBy     = args.SearchBy,
                SearchValue  = args.SearchValue,
                ActivityId   = args.ActivityId
            });
            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetEnrolledStudentsByProviderResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetEnrolledStudentsByProviderResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetEnrolledStudentsHandler");
            }
            return AppResult<GetEnrolledStudentsByProviderResult>.CreateSucceeded(new GetEnrolledStudentsByProviderResult
            {
                EnrolledStudentsList = result.Result.Result.Select(e =>
                {
                    return new GetEnrolledStudentsByProviderResult.EnrolledStudents
                    {
                        Id                  = e.Id,
                        ActivityId          = e.ActivityId,
                        Name                = e.Name,
                        Remarks             = e.Remarks,
                        ScheduleId          = e.ScheduleId,
                        SessionsAttended    = e.SessionsAttended,
                        NumberOfSessions    = e.NumberOfSessions,
                        StudentNo           = e.StudentNo,
                        ActivityTitle       = e.ActivityTitle,
                        ExpirationEndDate   = e.ExpirationEndDate,
                        ExpirationStartDate = e.ExpirationStartDate,
                        HasExpiration       = e.HasExpiration,
                        LastAttendance      = e.LastAttendance,
                        StudentType         = e.StudentType
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
            return AppResult<GetEnrolledStudentsByProviderResult>.CreateFailed(ex, "An error occured in GetEnrolledStudentsHandler");
        }
    }
}
