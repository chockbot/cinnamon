using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.Favorite.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Favorite.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.Favorite
{
    public class FavoriteData : IFavoriteData
    {
        private readonly IFlurlClient flurlClient;

        public FavoriteData(ApplicationConfig config, IFlurlClientFactory flurlFac)
        {
            this.flurlClient = flurlFac.Get(config.ApiDataUrl);
        }
        public async Task<AppResult<CreateFavoriteResult>> CreateFavorite(CreateFavoriteArgs args)
        {
            try
            {
                var result = await flurlClient
                    .Request("Favorite/Create")
                    .PostJsonAsync(args)
                    .ReceiveJson<CreateFavoriteResult>();

                return AppResult<CreateFavoriteResult>.CreateSucceeded(result, "Successfully posted create favorite api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<CreateFavoriteResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<CreateFavoriteResult>.CreateFailed(ex, "An error occured when posting create favorite api");
            }
        }

        public async Task<AppResult<GetFavoritesByCustomerResult>> GetFavoritesByCustomer(GetFavoritesByCustomerArgs args)
        {
            try
            {
                var result = await flurlClient
                            .Request("Favorite/ByCustomer")
                            .SetQueryParams(args)
                            .GetJsonAsync<GetFavoritesByCustomerResult>();

                return AppResult<GetFavoritesByCustomerResult>.CreateSucceeded(result, "Successfully posted get favorites api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetFavoritesByCustomerResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetFavoritesByCustomerResult>.CreateFailed(ex, "An error occured when posting get favorites api");
            }
        }

        public async Task<AppResult<RemoveFavoriteResult>> RemoveFavorite(RemoveFavoriteArgs args)
        {
            try
            {
                var result = await flurlClient
                    .Request("Favorite/Remove")
                    .PostJsonAsync(args)
                    .ReceiveJson<RemoveFavoriteResult>();

                return AppResult<RemoveFavoriteResult>.CreateSucceeded(result, "Successfully posted remove favorite api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<RemoveFavoriteResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<RemoveFavoriteResult>.CreateFailed(ex, "An error occured when posting remove favorite api");
            }
        }
    }
}
