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
        int ongoingActivityId, string remarks = "", string status = "ACTIVE", bool isDisbursement = false, bool hasReview = false);
    Task<AppResult<IEnumerable<StudentDTO>>> Create(int customerId, int activityId, int scheduleId,int numberOfSessions, 
        int sessionsAttended, int numberOfBacktracking, DateTime expirationStartDate, DateTime ExpirationEndDate, IEnumerable<CreateManyStudentDTO> familyMembers, 
        int ongoingActivityId, string remarks = "", string status = "ACTIVE", bool isDisbursement = false, bool hasReview = false);
    Task<AppResult<StudentDTO>> Update(int studendId, string? name, string? studentNo, int? numberOfSessions, 
        int? sessionsAttended, int? numberOfBacktracking, string? remarks, string? status, DateTime? expirationStartDate, DateTime? ExpirationEndDate, bool? hasReview);
    Task<AppResult<IEnumerable<StudentDTO>>> GetEnrolledStudent(int ActivityId);
    Task<AppResult<IEnumerable<DisburseStudentDTO>>> GetStudentsToDisburse(bool isInclusive, bool isExpired);
    Task<AppResult<IEnumerable<StudentDTO>>> UpdateStudentsDisbursementStatus(IEnumerable<int> ids, bool isDisbursement);
    Task<AppResult<IEnumerable<StudentDTO>>> GetCompletedStudentsById(int customerId, int? count, int? skip);
    Task<AppResult<IEnumerable<StudentDTO>>> GetAllStudentsById(int customerId, int? count, int? skip);
    Task<AppResult<IEnumerable<ExpiredStudentDTO>>> ExpiringStudents();
    Task<AppResult<IEnumerable<StudentDTO>>> GetEnrolleeMasterList(int providerId, int? count, int? skip);
    Task<AppResult<IEnumerable<StudentDTO>>> GetEnrolledStudentsByProvider(int? providerId, string searchValue, int searchBy,int activityId, int? count, int? skip);
}