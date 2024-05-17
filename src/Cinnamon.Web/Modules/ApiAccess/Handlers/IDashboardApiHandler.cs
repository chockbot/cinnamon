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
    Task<AppResult<GetAllStudentAttendanceByIdResult>> GetAllStudentAttendanceById(GetAllStudentAttendanceByIdArgs args, string token);
    Task<AppResult<CreateStudentAttendanceResult>> CreateStudentAttendance(CreateStudentAttendanceArgs args, string token);
    Task<AppResult<UpdateAttendanceResult>> UpdateAttendance(UpdateAttendanceArgs args, string token);
    Task<AppResult<GetAllBadgesResult>> GetAllBadge();
    Task<AppResult<GetAllStudentAttendanceResult>> GetAllStudentsAttendance(GetAllStudentAttendanceArgs args, string token);
    Task<AppResult<GetCompletedStudentsResult>> GetCompletedStudents(GetCompletedStudetnsArgs args, string token);
    Task<AppResult<GetOTEByProviderResult>> GetOTEByProvider(GetOTEByProviderArgs args, string token);
    Task<AppResult<GetOTEByActivityIdResult>> GetOTEByActivityId(GetOTEByActivityIdArgs args, string token);
    Task<AppResult<GetTicketDetailsResult>> GetTicketDetails(GetTicketDetailsArgs args, string token);
    Task<AppResult<UpdateOTETicketResult>> UpdateOTETicket(UpdateOTETicketArgs args, string token);
    Task<AppResult<GetDisbursementByProviderResult>> GetDisbursementByProvider(GetDisbursementByProviderArgs args, string token);
    Task<AppResult<GetEnrolledStudentsByProviderResult>> GetEnrolledStudentsByProvider(GetEnrolledStudentsByProviderArgs args, string token);
    Task<AppResult<CreateDirectStudentsResult>> CreateDirectStudents(CreateDirectStudentsArgs args, string token);
} 
