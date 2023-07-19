using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IStudentAttendance : IGenericEntity<StudentAttendance>
{
    Task<AppResult<IEnumerable<StudentAttendance>>> UpdateStudentAttendance(DateTime date, IEnumerable<StudentAttendance> attendances);

    Task<AppResult<StudentAttendance>> GetLastStudentAttendance(int id, int activityId, int scheduleId);
}