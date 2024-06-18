using Cinnamon.Framework.ApiCommand.ApiData.DTO.DirectStudent;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Student;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IDirectStudentRepository 
{
    Task<AppResult<IEnumerable<DirectStudentDTO>>> CreateDirectStudents(IEnumerable<DirectStudentDTO> directStudents);

    Task<AppResult<IEnumerable<DirectStudentSessionDTO>>> GetDirectStudents(int? count, int? skip, int? activityId, int? scheduleId, string? status);

    Task<AppResult<IEnumerable<DirectStudentInfoDTO>>> GetDirectStudentsInfo(int? providerId,string? searchValue, int? count, int? skip);

    Task<AppResult<IEnumerable<DirectStudentPaymentDTO>>> GetStudentPaymentByProvider(int? providerId, DateTime? dateFrom);

    Task<AppResult<DirectStudentInfoDTO>> UpdateDirectStudent(DirectStudentInfoDTO? studentInfo, IEnumerable<DirectStudentSessionDTO>? sessions);

    Task<AppResult<DirectStudentDTO>> DirectStudentInfo(int studentId);

    Task<AppResult<IEnumerable<DirectStudentSessionDTO>>> StudentSessions(int? studentId, bool ongoing, bool completed);

    Task<AppResult<IEnumerable<ExpiredStudentDTO>>> ExpiringStudents();
}