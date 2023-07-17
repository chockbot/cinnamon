using Cinnamon.Framework.ApiCommand.ApiData.DTO.Reviews;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IReviewsRepository
{
    Task<AppResult<ReviewsDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<ReviewsDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<ReviewsDTO>>> GetAllAsync();
    Task<AppResult<ReviewsDTO>> CreateReviewAsync(int CustomerId, int MakerId, int ActivityId, int ScheduleId, int StudentId, int Rating, string Review, DateTime ReviewDate);
    Task<AppResult<ReviewsDTO>> UpdateReviewAsync(int Id, int? CustomerId, int? MakerId, int? ActivityId, int? ScheduleId, int? StudentId, int? Rating, string? Review, DateTime? ReviewDate);
    Task<AppResult<IEnumerable<ReviewsDTO>>> GetAllReviewsById(int? makerId, int? count, int? skip);
    Task<AppResult<IEnumerable<ReviewsDTO>>> GetAllReviewsByCustomerId(int? customerId, int? count, int? skip);
    Task<AppResult<IEnumerable<ReviewsDTO>>> GetAllReviewsByActivityId(int? activityId, int? count, int? skip);
}
