using Cinnamon.Framework.ApiCommand.ApiData.DTO.DirectStudent;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IDirectStudentRepository 
{
    Task<AppResult<IEnumerable<DirectStudentDTO>>> CreateDirectStudents(IEnumerable<DirectStudentDTO> directStudents);

    Task<AppResult<IEnumerable<DirectStudentSessionDTO>>> GetDirectStudents(int? count, int? skip, int? activityId, int? scheduleId, string? status);
}