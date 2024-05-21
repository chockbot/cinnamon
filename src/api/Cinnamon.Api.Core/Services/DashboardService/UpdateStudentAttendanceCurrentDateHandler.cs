using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;

public class UpdateStudentAttendanceCurrentDateHandler : IUpdateStudentAttendanceCurrentDateHandler
{
    private readonly IUpdateStudentAttendanceHandler updateStudentAttendanceHandler;

    public UpdateStudentAttendanceCurrentDateHandler(IUpdateStudentAttendanceHandler updateStudentAttendanceHandler)
    {
        this.updateStudentAttendanceHandler = updateStudentAttendanceHandler;
    }

    public AppResult<UpdateStudentAttendanceCurrentDateResult> Execute(UpdateStudentAttedanceCurrentDateArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateStudentAttendanceCurrentDateResult>.CreateFailed(ex, "An error occured in UpdateStudentAttendanceCurrentDateHandler");
        }
    }

    public async Task<AppResult<UpdateStudentAttendanceCurrentDateResult>> ExecuteAsync(UpdateStudentAttedanceCurrentDateArgs args)
    {
        try
        {
            var updatedStudentRes = await updateStudentAttendanceHandler.ExecuteAsync(new UpdateStudentAttendanceArgs {
                Date = DateTime.Now,
                Students = args.Students.Select(s => {
                    return new UpdateStudentAttendanceArgs.StudentDetails {
                        ActivityId = s.ActivityId,
                        IsPresent = s.IsPresent,
                        ScheduleId = s.ScheduleId,
                        StudentId = s.StudentId,
                        StudentType = s.StudentType
                    };
                })
            });
            if(!updatedStudentRes.Succeeded || updatedStudentRes.Result == null)
            {
                return AppResult<UpdateStudentAttendanceCurrentDateResult>.CreateFailed(updatedStudentRes.Error.Exception, updatedStudentRes.Message);
            }

            return AppResult<UpdateStudentAttendanceCurrentDateResult>.CreateSucceeded(new UpdateStudentAttendanceCurrentDateResult {
                StudentAttendaces = updatedStudentRes.Result.StudentAttendaces.Select(s => {
                    return new UpdateStudentAttendanceCurrentDateResult.UpdatedStudentDetails {
                        ActivityId = s.ActivityId,
                        IsPresent = s.IsPresent,
                        ScheduleId = s.ScheduleId,
                        StudentId = s.StudentId
                    };
                })
            }, "Successfully update student attendance");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateStudentAttendanceCurrentDateResult>.CreateFailed(ex, "An error occured in UpdateStudentAttendanceCurrentDateHandler");
        }
    }
}