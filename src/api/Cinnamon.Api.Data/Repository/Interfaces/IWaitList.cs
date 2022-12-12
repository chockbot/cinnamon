using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IWaitList : IGenericEntity<WaitList>
{
    Task<AppResult<WaitList>> GetWaitListByEmailAsync(string email);
    Task<AppResult<WaitList>> GetWaitListByGuidAsync(string guid);
}