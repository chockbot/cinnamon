using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService
{
    public class RemoveFavoriteHandler : IRemoveFavoriteHandler
    {
        private readonly IFavoriteData favoriteData;

        public RemoveFavoriteHandler(IFavoriteData favoriteData)
        {
            this.favoriteData = favoriteData;
        }

        public AppResult<RemoveFavoriteResult> Execute(RemoveFavoriteArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<RemoveFavoriteResult>.CreateFailed(ex, "An error occured in RemoveFavoriteHandler");
            }
        }

        public async Task<AppResult<RemoveFavoriteResult>> ExecuteAsync(RemoveFavoriteArgs args)
        {
            var result = await favoriteData.RemoveFavorite(new Framework.ApiCommand.ApiData.Favorite.Request.RemoveFavoriteArgs
            {
                ActivityId = args.ActivityId,
                CustomerId = args.CustomerId
            });

            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<RemoveFavoriteResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<RemoveFavoriteResult>.CreateFailed(new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in RemoveFavoriteHandler");
            }

            var favoriteResult = result.Result.Result;

            return AppResult<RemoveFavoriteResult>.CreateSucceeded(new RemoveFavoriteResult
            {
                ActivityId = favoriteResult.ActivityId,
                CustomerId = favoriteResult.CustomerId
            }, "successfully called RemoveFavoriteHandler");
        }
    }
}
