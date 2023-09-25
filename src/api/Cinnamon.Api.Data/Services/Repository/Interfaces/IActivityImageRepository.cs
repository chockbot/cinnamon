using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ActivityImage;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IActivityImageRepository 
{
    Task<AppResult<ActivityImageDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<ActivityImageDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<ActivityImageDTO>>> GetAllAsync();
    Task<AppResult<ActivityImageDTO>> Create(int activityId, string imageName, string imagePath, int order);
    Task<AppResult<ActivityImageDTO>> Update(int activityImageId, string? imageName, string? imagePath, int? order);
    Task<AppResult<IEnumerable<ActivityImageDTO>>> Create(IEnumerable<ActivityImageDTO> images);
    Task<AppResult<IEnumerable<ActivityImageDTO>>> Update(IEnumerable<ActivityImageDTO> images);
    Task<AppResult<bool>> DeleteActivityImages(int activityId);
    Task<AppResult<bool>> DeleteActivityImages(int[] ids);
}