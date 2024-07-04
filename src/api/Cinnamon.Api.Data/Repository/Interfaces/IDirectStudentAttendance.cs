using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IDirectStudentAttendance : IGenericEntity<DirectStudentAttendance>
{
    Task<AppResult<IEnumerable<DirectStudentAttendance>>> UpdateStudentAttendance(IEnumerable<DirectStudentAttendance> studentAttendances, DateTime date);
}