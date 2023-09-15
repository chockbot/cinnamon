using Cinnamon.Framework.ApiCommand.ApiData.DTO.StudentAttendance;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IStudentAttendanceRepository 
{
    Task<AppResult<StudentAttendanceDTO>> GetByIdAsync(int id, bool? includeStudent = false);
    Task<AppResult<IEnumerable<StudentAttendanceDTO>>> GetAllAsync(int? count, int? skip, 
        DateTime? date = null, bool? includeStudent = false, IEnumerable<int>? activityIds = null, IEnumerable<int>? scheduleIds = null);
    Task<AppResult<IEnumerable<StudentAttendanceDTO>>> GetAllAsync();
    Task<AppResult<StudentAttendanceDTO>> Create(int studentId, bool isPresent, DateTime date);
    Task<AppResult<IEnumerable<StudentAttendanceDTO>>> Create(IEnumerable<CreateManyAttendanceDTO> students);
    Task<AppResult<StudentAttendanceDTO>> Update(int id, bool? isPresent, DateTime? date);
    Task<AppResult<IEnumerable<StudentAttendanceDTO>>> Update(IEnumerable<UpdateManyStudentDTO> students);
    Task<AppResult<IEnumerable<StudentAttendanceDTO>>> UpdateAttendance(IEnumerable<UpdateAttendanceDTO> students, DateTime date);
    Task<AppResult<IEnumerable<StudentAttendanceDTO>>> GetAttendanceByIdAsync(int id, int activityId,int? count, int? skip,
        DateTime? date = null, bool? includeStudent = false, IEnumerable<int>? scheduleIds = null);
    Task<AppResult<IEnumerable<StudentAttendanceDTO>>> GetCompletedStudents(int? count, int? skip, bool? includeStudent = false, IEnumerable<int>? activityIds = null, IEnumerable<int>? scheduleIds = null);
    Task<AppResult<StudentAttendanceDTO>> GetLastStudentAttendance(int id, int activityId, int scheduleId, bool? includeStudent = false);
    Task<AppResult<IEnumerable<StudentAttendanceDTO>>> GetAttendanceByFamilyId(int familyId);
}