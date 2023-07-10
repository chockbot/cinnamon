using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService
{
    public class CreateFavoriteHandler : ICreateFavoriteHandler
    {
        private readonly IFavoriteData favoriteData;

        public CreateFavoriteHandler(IFavoriteData favoriteData)
        {
            this.favoriteData = favoriteData;
        }
        public AppResult<CreateFavoriteResult> Execute(CreateFavoriteArgs args)
        {
            try
            {
                return ExecuteAsync(args).Result;
            }
            catch (Exception ex)
            {
                return AppResult<CreateFavoriteResult>.CreateFailed(ex, "An error occured in CreateFavoriteHandler");
            }
        }

        public async Task<AppResult<CreateFavoriteResult>> ExecuteAsync(CreateFavoriteArgs args)
        {
            var result = await favoriteData.CreateFavorite(new Framework.ApiCommand.ApiData.Favorite.Request.CreateFavoriteArgs
            {
                ActivityId = args.ActivityId,
                CustomerId = args.CustomerId
            });

            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<CreateFavoriteResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if (result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<CreateFavoriteResult>.CreateFailed(new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in CreateFavoriteHandler");
            }

            var favoriteResult = result.Result.Result;

            return AppResult<CreateFavoriteResult>.CreateSucceeded(new CreateFavoriteResult
            {
                ActivityId = favoriteResult.ActivityId,
                CustomerId = favoriteResult.CustomerId
            }, "successfully called CreateFavoriteHandler");
        }
    }
}
