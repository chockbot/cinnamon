using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Student;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IDirectStudentSession : IGenericEntity<DirectStudentSession>
{
    Task<AppResult<int>> OngoingStudentCount(int activityId);
    Task<AppResult<int>> CompletedStudentCount(int activityId);
    Task<AppResult<IEnumerable<DirectStudentSession>>> StudentSessions(int? studentId, 
        bool? ongoingSessions = false, bool? completedSessions = false);

    Task<AppResult<IEnumerable<ExpiredStudentDTO>>> ExpiringStudents();
}