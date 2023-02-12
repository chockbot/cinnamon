using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Request;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;

public class CreateStudentAttendanceHandler: ICreateStudentAttendanceHandler
{
    private readonly IStudentAttendanceData studentAttendanceData;
	public CreateStudentAttendanceHandler(IStudentAttendanceData studentAttendanceData)
	{
	  this.studentAttendanceData = studentAttendanceData;
	}

    public AppResult<CreateStudentAttendanceResult> Execute(CreateStudentAttendanceArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CreateStudentAttendanceResult>.CreateFailed(ex, "An error occured in CreateActivityHandler");
        }
    }

    public async Task<AppResult<CreateStudentAttendanceResult>> ExecuteAsync(CreateStudentAttendanceArgs args)
    {
        try
        {
            var createAttendance = await studentAttendanceData.CreateStudentAttendance(new CreateStudentAttendaceArgs
            {
                Date = args.AttendanceDate,
                IsPresent = args.IsPresent,
                StudentId = args.StudentId
            });

            if (!createAttendance.Succeeded)
            {
                return AppResult<CreateStudentAttendanceResult>.CreateFailed(createAttendance.Error.Exception, createAttendance.Message);
            }

            if (createAttendance.Result == null)
            {
                return AppResult<CreateStudentAttendanceResult>.CreateFailed(
                    new ApplicationException("An error occured in SubmitRegisterHandler"), "An error occured in CreateActivityHandler");
            }

            var created = createAttendance.Result.Result;

            return AppResult<CreateStudentAttendanceResult>.CreateSucceeded(new CreateStudentAttendanceResult
            {
                AttendanceDate  = created.Date,
                StudentId       = created.StudentId,
                IsPresent       = created.IsPresent,
            }, "Successfully registered");
        }
        catch (Exception ex)
        {
            return AppResult<CreateStudentAttendanceResult>.CreateFailed(ex, "An error occured in CreateActivityHandler");
        }
    }
}
