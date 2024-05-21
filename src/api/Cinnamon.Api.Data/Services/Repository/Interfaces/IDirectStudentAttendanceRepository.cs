using Cinnamon.Framework.ApiCommand.ApiData.DTO.DirectStudent;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.StudentAttendance;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IDirectStudentAttendanceRepository 
{
    Task<AppResult<IEnumerable<StudentAttendanceDTO>>> GetAllAsync(int? count, int? skip, DateTime? date = null, 
        bool? includeStudent = false, IEnumerable<int>? activityIds = null, IEnumerable<int>? scheduleIds = null);

    Task<AppResult<IEnumerable<DirectStudentAttendanceDTO>>> CreateDirectStudentAttendances(IEnumerable<DirectStudentAttendanceDTO> attendances);

    Task<AppResult<IEnumerable<DirectStudentAttendanceDTO>>> UpdateStudentAttendances(IEnumerable<DirectStudentAttendanceDTO> attendances, DateTime date);
}