using Cinnamon.Framework.ApiCommand.ApiData.DTO.Student;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IStudentRepository 
{
    Task<AppResult<StudentDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<StudentDTO>>> GetAllAsync(int? count, int? skip, int? activityId, int? scheduleId, string? status);
    Task<AppResult<IEnumerable<StudentDTO>>> GetAllAsync();
    Task<AppResult<StudentDTO>> Create(int customerId, int familyMemberId, int activityId, int scheduleId, string name, string studentNo, 
        int numberOfSessions, int sessionsAttended, int numberOfBacktracking, DateTime expirationStartDate, DateTime ExpirationEndDate, 
        int ongoingActivityId, string remarks = "", string status = "ACTIVE");
    Task<AppResult<IEnumerable<StudentDTO>>> Create(int customerId, int activityId, int scheduleId,int numberOfSessions, 
        int sessionsAttended, int numberOfBacktracking, DateTime expirationStartDate, DateTime ExpirationEndDate, IEnumerable<CreateManyStudentDTO> familyMembers, 
        int ongoingActivityId, string remarks = "", string status = "ACTIVE");
    Task<AppResult<StudentDTO>> Update(int studendId, string? name, string? studentNo, int? numberOfSessions, 
        int? sessionsAttended, int? numberOfBacktracking, string? remarks, string? status, DateTime? expirationStartDate, DateTime? ExpirationEndDate);
    Task<AppResult<IEnumerable<StudentDTO>>> GetEnrolledStudent(int ActivityId);
}