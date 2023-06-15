using Cinnamon.Framework.ApiCommand.ApiData.Favorite.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Favorite.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers
{
    public interface IFavoriteData
    {
        Task<AppResult<CreateFavoriteResult>> CreateFavorite(CreateFavoriteArgs args);
        Task<AppResult<RemoveFavoriteResult>> RemoveFavorite(RemoveFavoriteArgs args);
        Task<AppResult<GetFavoritesByCustomerResult>> GetFavoritesByCustomer(GetFavoritesByCustomerArgs args);
    }
}
