using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.BadgeList;
namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IBadgeListRepository
{
    Task<AppResult<BadgeListDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<BadgeListDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<BadgeListDTO>>> GetAllAsync();
    Task<AppResult<BadgeListDTO>> CreateBadgeAsync(string Name, string Description, int NumberOfStudent,int NumberOfCompleted, int NumberOfReviews, string ImgScr);
    Task<AppResult<BadgeListDTO>> UpdateBadgeAsync(int Id, string? Name, string? Description, int? NumberOfStudent, int? NumberOfCompleted, int? NumberOfReviews, string? ImgScr);
}
