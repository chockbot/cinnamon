using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Handlers;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DirectStudentService;
public class UpdateStudentAttendanceHandler : IUpdateStudentAttendanceHandler
{
    private readonly IMapper mapper;
    private readonly IDirectStudentData directStudentData;
    private readonly IStudentAttendanceData studentAttendanceData;
    private readonly IGetOwnedActivitiesHandler getOwnedActivitiesHandler;
    public UpdateStudentAttendanceHandler(IGetOwnedActivitiesHandler getOwnedActivitiesHandler, IDirectStudentData directStudentData, 
        IStudentAttendanceData studentAttendanceData,IMapper mapper)
    {
        this.getOwnedActivitiesHandler = getOwnedActivitiesHandler;
        this.studentAttendanceData = studentAttendanceData;
        this.directStudentData = directStudentData;
        this.mapper = mapper;
    }

    public AppResult<UpdateStudentAttendanceResult> Execute(UpdateStudentAttendanceArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<UpdateStudentAttendanceResult>> ExecuteAsync(UpdateStudentAttendanceArgs args)
    {
        try
        {
            var updateDirectStudentRes = await directStudentData.UpdateStudentAttendance(new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateStudentAttendanceBulkArgs
            {
                Date = args.Date.Date,
                StudentAttendances = args.Students.Select(s => new Framework.ApiCommand.ApiData.DirectStudent.Request.UpdateStudentAttendanceBulkArgs.UpdateStudentAttendance
                {
                    DirectStudentSessionId = s.StudentId,
                    IsPresent = s.IsPresent
                })
            });
            if (!updateDirectStudentRes.Succeeded || updateDirectStudentRes.Result is null || !updateDirectStudentRes.Result.IsSuccess)
            {
                return AppResult<UpdateStudentAttendanceResult>.CreateFailed(
                    new ApplicationException(updateDirectStudentRes.Result?.ErrorInfo?.Message), updateDirectStudentRes.Message);
            }

            return AppResult<UpdateStudentAttendanceResult>.CreateSucceeded(new UpdateStudentAttendanceResult
            {
                StudentAttendaces = updateDirectStudentRes.Result.Result.Select(s => {
                    return new UpdateStudentAttendanceResult.UpdatedStudentDetails
                    {
                        Date = s.Date,
                        IsPresent = s.IsPresent,
                        StudentId = s.DirectStudentSessionId
                    };
                })
            }, "Successfully update student attendance");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateStudentAttendanceResult>.CreateFailed(ex, "An error occured in UpdateStudentAttendanceHandler");
        }
    }
}
