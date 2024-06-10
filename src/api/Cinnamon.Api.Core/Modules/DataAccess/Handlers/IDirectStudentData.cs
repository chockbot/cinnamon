using Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;
using Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IDirectStudentData
{
    Task<AppResult<CreateDirectStudentsResult>> CreateDirectStudents(CreateDirectStudentsArgs args);

    Task<AppResult<GetDirectStudentsResult>> GetDirectStudents(GetDirectStudentsArgs args);

    Task<AppResult<GetDirectStudentByIdResult>> GetDirectStudentsById(GetDirectStudentByIdArgs args);

    Task<AppResult<StudentAttendanceResult>> StudentAttendance(StudentAttendanceArgs args);

    Task<AppResult<CreateDirectStudentAttendanceResult>> CreateStudentAttendance(CreateStudentAttendanceArgs args);

    Task<AppResult<UpdateStudentAttendanceBulkResult>> UpdateStudentAttendance(UpdateStudentAttendanceBulkArgs args);

    Task<AppResult<StudentInfosResult>> StudentInfos(StudentInfosArgs args);

    Task<AppResult<GetDirectStudentsPaymentResult>> GetDirectStudentsPayments(GetDirectStudentsPaymentArgs args);

    Task<AppResult<UpdateDirectStudentResult>> UpdateDirectStudent(UpdateDirectStudentArgs args);

    Task<AppResult<DirectStudentInfoResult>> DirectStudentInfo(int studentId);

    Task<AppResult<StudentSessionsResult>> StudentSessions(StudentSessionsArgs arg, int studentId);
}