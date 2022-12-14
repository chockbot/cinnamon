using Cinnamon.Framework.ApiCommand.ApiData.Waitlist.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Waitlist.Request;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IWaitListData 
{
    Task<AppResult<GetWaitlistResult>> GetWaitListByEmail(string email);
}