using Cinnamon.Framework.ApiCommand.ApiData.Reviews.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Reviews.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IReviewsData
{
    Task<AppResult<GetReviewResult>> GetReviewById(int id);
    Task<AppResult<GetAllReviewsResult>> GetAllReviews(GetAllReviewsArgs args);
    Task<AppResult<CreatedReviewResult>> CreateReview(CreateReviewArgs args);
    Task<AppResult<UpdatedReviewResult>> UpdateReview(UpdateReviewArgs args);
    Task<AppResult<GetReviewsByMakerIdResult>> GetReviewsByMakerId(GetReviewsByMakerIdArgs args);
    Task<AppResult<GetReviewsByCustomerIdResult>> GetReviewsByCustomerId(GetReviewsByCustomerIdArgs args);
    Task<AppResult<GetReviewsByActivityIdResult>> GetReviewsByActivityId(GetReviewsByActivityIdArgs args);
}
