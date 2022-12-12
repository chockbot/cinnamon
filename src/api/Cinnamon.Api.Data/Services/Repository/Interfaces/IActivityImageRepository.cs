using Cinnamon.Framework.Common;
using Cinnamon.Api.Data.Services.Repository.ActivityImage.DTO;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IActivityImageRepository 
{
    Task<AppResult<ActivityImageDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<ActivityImageDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<ActivityImageDTO>>> GetAllAsync();
    Task<AppResult<ActivityImageDTO>> Create(int activityId, string imageName, string imagePath);
    Task<AppResult<ActivityImageDTO>> Update(int activityImageId, string? imageName, string? imagePath);
}