using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IDirectStudentSession : IGenericEntity<DirectStudentSession>
{
    Task<AppResult<int>> OngoingStudentCount(int activityId);
    Task<AppResult<int>> CompletedStudentCount(int activityId);
}