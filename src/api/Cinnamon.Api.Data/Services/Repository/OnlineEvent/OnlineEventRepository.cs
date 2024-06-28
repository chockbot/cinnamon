using Cinnamon.Api.Data.Repository.Interfaces;
using Cinnamon.Api.Data.Services.Repository.Interfaces;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.OnlineEvent;
public class OnlineEventRepository : IOnlineEventRepository
{
    private readonly IDataStore _dataStore;
    public OnlineEventRepository(IDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public async Task<AppResult<bool>> DeleteOnlineEvent(int Id)
    {
        try
        {
            var onlineEvent = await _dataStore.OteOnlineEvent.GetByIdAsync(Id);
            if (onlineEvent.Result == null)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException("No online event to delete"), "No online event to delete");
            }

            var result = await _dataStore.OteOnlineEvent.Remove(onlineEvent.Result);

            if (!result.Succeeded || result.Result == null)
            {
                return AppResult<bool>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            return AppResult<bool>.CreateSucceeded(true, "Successfully deleted online event");
        }
        catch (Exception ex)
        {
            return AppResult<bool>.CreateFailed(ex, "An error occured in deleting online event");
        }
    }
}
