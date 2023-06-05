using Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Response;
using Cinnamon.Framework.ApiCommand.ApiCore.OnGoingActivities.Request;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers;
public interface IOngoingActivitiesHandler
{
    Task<AppResult<GetAllOngoingActivitiesResult>> GetAllOngoingActivities();

    Task<AppResult<GetOngoingActivityByIdResult>>GetOngoingActivityById(int id);

    Task<AppResult<GetEnrolledStudentsResult>> GetEnrolledStudents(int activityId);

    Task<AppResult<UpdateOngoingActivityResult>> UpdateActivity(UpdateOngoingActivityArgs args);

    Task<AppResult<AddActivityExpirationResult>> AddActivityExpiration(AddActivityExpirationArgs args);

    Task<AppResult<GetCompletedStudentsByIdResult>> GetCompletedStudentsById(GetCompletedStudentsByIdArgs args, string token);

    Task<AppResult<CreateReviewResult>> CreateReview(CreateReviewArgs args, string token);

    Task<AppResult<GetAllStudentsByIdResult>> GetAllStudentsById(GetAllStudentsByIdArgs args, string token);
}
