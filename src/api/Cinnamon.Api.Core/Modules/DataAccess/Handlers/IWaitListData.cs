using Cinnamon.Framework.ApiCommand.ApiData.Waitlist.Response;
using Cinnamon.Framework.ApiCommand.ApiData.Waitlist.Request;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IWaitListData 
{
    Task<AppResult<GetWaitlistResult>> GetWaitListByEmail(string email);
    Task<AppResult<GetWaitlistResult>> GetWaitListById(int id);
    Task<AppResult<GetWaitlistResult>> GetWaitListByGuid(string guid);
    Task<AppResult<GetAllWaitlistResult>> GetAllWaitlist(GetAllWaitlistArgs args);
    Task<AppResult<CreatedWaitlistResult>> CreateWaitlist(CreateWaitlistArgs args);
    Task<AppResult<UpdateWaitlistResult>> UpdateWaitlist(UpdateWaitlistArgs args);
    Task<AppResult<DeleteWaitlistResult>> DeleteWaitlist(DeleteWaitlistArgs args);
}