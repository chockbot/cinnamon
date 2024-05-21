using Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;
using Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IDirectStudentData
{
    Task<AppResult<CreateDirectStudentsResult>> CreateDirectStudents(CreateDirectStudentsArgs args);

    Task<AppResult<GetDirectStudentsResult>> GetDirectStudents(GetDirectStudentsArgs args);

    Task<AppResult<StudentAttendanceResult>> StudentAttendance(StudentAttendanceArgs args);

    Task<AppResult<CreateStudentAttendanceResult>> CreateStudentAttendance(CreateStudentAttendanceArgs args);

    Task<AppResult<UpdateStudentAttendanceBulkResult>> UpdateStudentAttendance(UpdateStudentAttendanceBulkArgs args);
}