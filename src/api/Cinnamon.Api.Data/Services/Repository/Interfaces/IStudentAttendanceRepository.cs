using Cinnamon.Framework.ApiCommand.ApiData.DTO.StudentAttendance;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IStudentAttendanceRepository 
{
    Task<AppResult<StudentAttendanceDTO>> GetByIdAsync(int id, bool? includeStudent = false);
    Task<AppResult<IEnumerable<StudentAttendanceDTO>>> GetAllAsync(int? count, int? skip, 
        DateOnly? date = null, bool? includeStudent = false);
    Task<AppResult<IEnumerable<StudentAttendanceDTO>>> GetAllAsync();
    Task<AppResult<StudentAttendanceDTO>> Create(int studentId, bool isPresent, DateOnly date);
    Task<AppResult<IEnumerable<StudentAttendanceDTO>>> Create(IEnumerable<CreateManyAttendanceDTO> students);
    Task<AppResult<StudentAttendanceDTO>> Update(int id, bool? isPresent, DateOnly? date);
    Task<AppResult<IEnumerable<StudentAttendanceDTO>>> Update(IEnumerable<UpdateManyStudentDTO> students);
}