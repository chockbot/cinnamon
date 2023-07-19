using Cinnamon.Framework.ApiCommand.ApiData.DTO.Favorite;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<AppResult<FavoriteDTO>> Create(int customerId, int activityId);
        Task<AppResult<bool>> Remove(int customerId, int activityId);
        Task<AppResult<IEnumerable<FavoriteDTO>>> GetFavoritesByCustomer(int customerId);
    }
}
