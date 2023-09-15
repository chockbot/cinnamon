using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;

public class GetAllStudentAttendanceByIdHandler: IGetAllStudentAttendanceByIdHandler
{
	private readonly IStudentAttendanceData studentAttendanceData;
	public GetAllStudentAttendanceByIdHandler(IStudentAttendanceData studentAttendanceData)
	{
		this.studentAttendanceData = studentAttendanceData;
	}

    public AppResult<GetAllStudentAttendanceByIdResult> Execute(GetAllStudentAttendanceByIdArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetAllStudentAttendanceByIdResult>.CreateFailed(ex, "An error occured in GetAllStudentAttendanceByIdHandler");
        }
    }

	public async Task<AppResult<GetAllStudentAttendanceByIdResult>> ExecuteAsync(GetAllStudentAttendanceByIdArgs args)
	{
		try
		{
			var attendance = await studentAttendanceData.GetAllStudentAttendanceById(new Framework.ApiCommand.ApiData.StudentAttendance.Request.GetAllStudentAttendanceByIdArgs
			{
				IsIncludeStudent = true,
				Id               = args.StudentId,
				ScheduleIds      = new int[] { args.ScheduleId },
				ActivityId       = args.ActivityId 
			});
			if (!attendance.Succeeded || attendance.Result == null || !attendance.Result.IsSuccess)
			{
				return AppResult<GetAllStudentAttendanceByIdResult>.CreateFailed(new ApplicationException(attendance.Result?.ErrorInfo?.Message), attendance.Message);
			}
			return AppResult<GetAllStudentAttendanceByIdResult>.CreateSucceeded(new GetAllStudentAttendanceByIdResult
			{
				StudentAttendaces = attendance.Result.Result.Select(s => {
					return new GetAllStudentAttendanceByIdResult.StudentAttendace
					{
						IsPresent = s.IsPresent,
						StudentId = s.StudentId,
						NumberOfSessions = s.Student.NumberOfSessions,
						SessionsAttended = s.Student.SessionsAttended,
						Status = s.Student.Status,
						StudentName = s.Student.Name,
						StudentNo = s.Student.StudentNo,
						AttendanceDate = s.Date,
						Id = s.Id,
						Remarks = s.Student.Remarks,
						ActivityId = s.Student.ActivityId,
						ScheduleId = s.Student.ScheduleId,
						NumberOfBackTracking = s.Student.NumberOfBackTracking
					};
				})
			}, "Successfullt get student attendance");
		}
		catch (Exception ex)
		{
			return AppResult<GetAllStudentAttendanceByIdResult>.CreateFailed(ex, "An error occured in GetAllStudentAttendanceByIdHandler");
		}
	}
}
