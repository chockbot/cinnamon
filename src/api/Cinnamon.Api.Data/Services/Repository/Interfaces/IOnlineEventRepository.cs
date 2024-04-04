using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IOnlineEventRepository
{
    Task<AppResult<bool>> DeleteOnlineEvent(int Id);
}
