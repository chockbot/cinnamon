using Cinnamon.Framework.ApiCommand.ApiData.DTO.DirectStudent;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IDirectStudentRepository 
{
    Task<AppResult<IEnumerable<DirectStudentDTO>>> CreateDirectStudents(IEnumerable<DirectStudentDTO> directStudents);

    Task<AppResult<IEnumerable<DirectStudentSessionDTO>>> GetDirectStudents(int? count, int? skip, int? activityId, int? scheduleId, string? status);

    Task<AppResult<IEnumerable<DirectStudentInfoDTO>>> GetDirectStudentsInfo(int providerId, int? count, int? skip);

    Task<AppResult<IEnumerable<DirectStudentPaymentDTO>>> GetStudentPaymentByProvider(int? providerId, DateTime? dateFrom);
}