using Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.DirectStudent.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers;

public interface IDirectStudentApiHandler 
{
    Task<AppResult<DirectStudentInfoReult>> DirectStudents(DirectStudentInfoArgs args, string token);

    Task<AppResult<UpdateStudentResult>> UpdateStudent(UpdateStudentArgs args, string token);

    Task<AppResult<DirectStudentResult>> DirectStudent(int studentId, string token);

    Task<AppResult<StudentSessionsResult>> StudentSessions(StudentSessionsArgs args, int studentId, string token);

    Task<AppResult<GetDirectStudentByIdResult>> GetDirectStudentById(GetDirectStudentByIdArgs args, string token);

    Task<AppResult<CreateDirectStudentAttendanceResult>> CreateDirectStudentAttendance(CreateDirectStudentAttendanceArgs args, string token);

    Task<AppResult<UpdateDirectStudentAttendanceResult>> UpdateDirectStudentAttendance(UpdateDirectStudentAttendanceArgs args, string token);
}