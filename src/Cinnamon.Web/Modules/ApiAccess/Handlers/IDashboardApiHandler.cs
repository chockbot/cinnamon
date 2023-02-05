using Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Dashboard.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers;

public interface IDashboardApiHandler 
{
    Task<AppResult<GetActivitySchedulesResult>> GetActivitySchedules(string token);
    Task<AppResult<GetCurrentAttendanceResult>> GetCurrentAttendance(GetCurrentAttendanceArgs args,string token);
    Task<AppResult<UpdateStudentAttendanceResult>> UpdateStudentAttendances(UpdateStudentAttendnaceArgs args,string token);
    Task<AppResult<GetStudentAttendanceResult>> GetStudentAttendance(GetStudentAttendanceArgs args, string token);
} 
