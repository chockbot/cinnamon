using Cinnamon.Framework.ApiCommand.ApiData.PayoutAccount.Request;
using Cinnamon.Framework.ApiCommand.ApiData.PayoutAccount.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers;

public interface IPayoutAccountData 
{
    Task<AppResult<GetPayoutAccountResult>> GetPayoutAccountById(int id);
    Task<AppResult<GetPayoutAccountResult>> GetPayoutAccountByCustomerId(int id);
    Task<AppResult<GetAllPayoutAccountsResult>> GetAllPayoutAccounts(GetAllPayoutAccountsArgs args);
    Task<AppResult<CreatePayoutAccountResult>> CreatePayoutAccount(CreatePayoutAccountArgs args);
    Task<AppResult<UpdatePayoutAccountResult>> UpdatePayoutAccount(UpdatePayoutAccountArgs args);
}