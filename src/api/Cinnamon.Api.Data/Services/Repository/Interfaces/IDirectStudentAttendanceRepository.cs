using Cinnamon.Framework.ApiCommand.ApiData.DTO.StudentAttendance;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IDirectStudentAttendanceRepository 
{
    Task<AppResult<IEnumerable<StudentAttendanceDTO>>> GetAllAsync(int? count, int? skip, DateTime? date = null, 
        bool? includeStudent = false, IEnumerable<int>? activityIds = null, IEnumerable<int>? scheduleIds = null);
}