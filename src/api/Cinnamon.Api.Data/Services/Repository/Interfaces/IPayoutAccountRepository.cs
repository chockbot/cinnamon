using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.PayoutAccount;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IPayoutAccountRepository 
{
    Task<AppResult<PayoutAccountDTO>> GetByIdAsync(int id);
    Task<AppResult<PayoutAccountDTO>> GetByCustomerIdAsync(int id);
    Task<AppResult<IEnumerable<PayoutAccountDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<PayoutAccountDTO>>> GetAllAsync();
    Task<AppResult<PayoutAccountDTO>> Create(int customerId, string accountNo, string accountHolder, string payload, string bankChannel);
    Task<AppResult<PayoutAccountDTO>> Update(int id, string? accountNo, string? accountHolder, string? payload, string? bankChannel);
}
