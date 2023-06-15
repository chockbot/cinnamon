using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Favorite;
using Cinnamon.Framework.Common;
using Entities = Cinnamon.Api.Data.Repository.Entities;

namespace Cinnamon.Api.Data.Services.Repository.Favorite
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly IDataStore dataStore;

        public FavoriteRepository(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public async Task<AppResult<FavoriteDTO>> Create(int customerId, int activityId)
        {
            try
            {
                var favoriteResult = await dataStore.Favorite.FindFirstAsync(f => f.CustomerId == customerId && f.ActivityId == activityId);

                if (favoriteResult.Result == null)
                {
                    var entity = new Entities.Favorite
                    {
                        CustomerId = customerId,
                        ActivityId = activityId
                    };

                    var result = await dataStore.Favorite.Add(entity);

                    if (!result.Succeeded || result.Result == null)
                    {
                        return AppResult<FavoriteDTO>.CreateFailed(new ApplicationException(result.Message), result.Message);
                    }

                    return AppResult<FavoriteDTO>.CreateSucceeded(new FavoriteDTO
                    {
                        CustomerId = customerId,
                        ActivityId = activityId
                    }, "Successully created favorite");
                }
                else 
                {
                    return AppResult<FavoriteDTO>.CreateSucceeded(new FavoriteDTO
                    {
                        CustomerId = favoriteResult.Result.CustomerId,
                        ActivityId = favoriteResult.Result.ActivityId
                    }, "Successully created favorite");
                }
            }
            catch (Exception ex)
            {
                return AppResult<FavoriteDTO>.CreateFailed(ex, "An error occured when creating favorite");
            }
        }

        public async Task<AppResult<bool>> Remove(int customerId, int activityId)
        {
            try
            {
                var favoriteResult = await dataStore.Favorite.FindFirstAsync(c => c.CustomerId == customerId && c.ActivityId == activityId);

                if (!favoriteResult.Succeeded || favoriteResult.Result == null)
                {
                    return AppResult<bool>.CreateFailed(favoriteResult.Error.Exception, favoriteResult.Message);
                }

                var result = await dataStore.Favorite.Remove(favoriteResult.Result);

                if (!result.Succeeded || result.Result == null)
                {
                    return AppResult<bool>.CreateFailed(new ApplicationException(result.Message), result.Message);
                }

                return AppResult<bool>.CreateSucceeded(result.Succeeded, "Successully removed favorite");
            }
            catch (Exception ex)
            {
                return AppResult<bool>.CreateFailed(ex, "An error occured when removing favorite");
            }
        }

        public async Task<AppResult<IEnumerable<FavoriteDTO>>> GetFavoritesByCustomer(int customerId)
        {
            var customerResult = await dataStore.Customer.FindFirstAsync(c => c.Id == customerId);

            if (!customerResult.Succeeded || customerResult.Result == null)
            {
                return AppResult<IEnumerable<FavoriteDTO>>.CreateFailed(customerResult.Error.Exception, customerResult.Message);
            }

            var result = await dataStore.Favorite.FindAsync(c => c.CustomerId == customerId);

            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<IEnumerable<FavoriteDTO>>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            var favorites = result.Result.Select(r => new FavoriteDTO
            {
                CustomerId = r.CustomerId,
                ActivityId = r.ActivityId
            });

            return AppResult<IEnumerable<FavoriteDTO>>.CreateSucceeded(favorites, "Successfully retrieved chat histories");
        }
    }
}
