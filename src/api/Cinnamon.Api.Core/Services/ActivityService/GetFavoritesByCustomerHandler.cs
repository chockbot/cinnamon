using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService
{
    public class GetFavoritesByCustomerHandler : IGetFavoritesByCustomerHandler
    {
        private readonly IFavoriteData favoriteData;

        public GetFavoritesByCustomerHandler(IFavoriteData favoriteData)
        {
            this.favoriteData = favoriteData;
        }

        public AppResult<GetFavoritesByCustomerResult> Execute(GetFavoritesByCustomerArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<GetFavoritesByCustomerResult>.CreateFailed(ex, "An error occured in GetFavoritesByCustomerHandler");
            }
        }

        public async Task<AppResult<GetFavoritesByCustomerResult>> ExecuteAsync(GetFavoritesByCustomerArgs args)
        {
            var result = await favoriteData.GetFavoritesByCustomer(new Framework.ApiCommand.ApiData.Favorite.Request.GetFavoritesByCustomerArgs
            {
                CustomerId = args.CustomerId
            });

            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<GetFavoritesByCustomerResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetFavoritesByCustomerResult>.CreateFailed(new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in GetFavoritesByCustomerHandler");
            }

            var favoriteResult = result.Result.Result;

            return AppResult<GetFavoritesByCustomerResult>.CreateSucceeded(new GetFavoritesByCustomerResult
            {
                Favorites = favoriteResult.Select(f => new GetFavoritesByCustomerResult.Favorite
                {
                    CustomerId = f.CustomerId,
                    ActivityId = f.ActivityId
                })
            }, "successfully called GetFavoritesByCustomerHandler");
        }
    }
}
