using Cinnamon.Framework.ApiCommand.ApiData.DTO.Reviews;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IReviewsRepository
{
    Task<AppResult<ReviewsDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<ReviewsDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<ReviewsDTO>>> GetAllAsync();
    Task<AppResult<ReviewsDTO>> CreateExperienceCategoryAsync(int CustomerId, int MakerId, int ActivityId, int ScheduleId, int StudentId, int Rating, string Review, DateTime ReviewDate);
    Task<AppResult<ReviewsDTO>> UpdateExperienceCategoryAsync(int Id, int? CustomerId, int? MakerId, int? ActivityId, int? ScheduleId, int? StudentId, int? Rating, string? Review, DateTime? ReviewDate);
}
