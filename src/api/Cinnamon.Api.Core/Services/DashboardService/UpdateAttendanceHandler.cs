using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;
public class UpdateAttendanceHandler: IUpdateAttendanceHandler
{
	private readonly IStudentAttendanceData studentAttendanceData;
	public UpdateAttendanceHandler(IStudentAttendanceData studentAttendanceData)
	{
		this.studentAttendanceData = studentAttendanceData;
	}

	public AppResult<UpdateAttendanceResult> Execute(UpdateAttendanceArgs args)
	{
		try
		{
			return ExecuteAsync(args).Result;
		}
		catch (Exception ex)
		{
			return AppResult<UpdateAttendanceResult>.CreateFailed(ex, "An error occured in UpdateAttendanceHandler");
		}
	}

	public async Task<AppResult<UpdateAttendanceResult>> ExecuteAsync(UpdateAttendanceArgs args)
	{
		try
		{
			var updated = await studentAttendanceData.UpdateStudentAttendance(new Framework.ApiCommand.ApiData.StudentAttendance.Request.UpdateStudentAttendanceArgs
			{
				AttendanceId = args.Id,
				Date         = args.Date,
				IsPresent    = args.IsPresent
			});
			if (!updated.Succeeded || updated.Result == null || !updated.Result.IsSuccess)
			{
				return AppResult<UpdateAttendanceResult>.CreateFailed(new ApplicationException(updated.Result?.ErrorInfo?.Message), updated.Message);
			}
			if (updated.Succeeded && !updated.Result.IsSuccess)
			{
				return AppResult<UpdateAttendanceResult>.CreateFailed(
					new ApplicationException(updated.Result.ErrorInfo?.Message), "An error occured in UpdateAttendanceHandler");
			}
			return AppResult<UpdateAttendanceResult>.CreateSucceeded(new UpdateAttendanceResult
			{
				Id= updated.Result.Result.Id,
				IsPresent= updated.Result.Result.IsPresent,
				Date = updated.Result.Result.Date,
			}, "Successfully update student attendance");
		}
		catch (Exception ex)
		{
			return AppResult<UpdateAttendanceResult>.CreateFailed(ex, "An error occured in UpdateAttendanceHandler");
		}
	}
}
