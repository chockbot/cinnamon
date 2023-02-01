using Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Request;
using Cinnamon.Framework.ApiCommand.ApiData.StudentAttendance.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IStudentAttendanceData
{
    Task<AppResult<GetStudentAttendanceResult>> GetStudentAttendanceById(int id);
    Task<AppResult<GetAllStudentAttendanceResult>> GetAllStudentAttendance(GetAllStudentAttendanceArgs args);
    Task<AppResult<CreateStudentAttendanceResult>> CreateStudentAttendance(CreateStudentAttendaceArgs args);
    Task<AppResult<CreateManyStudentAttendanceResult>> CreateManyStudentAttendance(CreateManyStudentAttendanceArgs args);
    Task<AppResult<UpdateStudentAttendanceResult>> UpdateStudentAttendance(UpdateStudentAttendanceArgs args);
    Task<AppResult<UpdateManyStudentAttendanceResult>> UpdateManyStudentAttendance(UpdateManyStudentAttendanceArgs args);
}
