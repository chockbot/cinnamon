using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.PayoutLog;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IPayoutLogRepository 
{
    Task<AppResult<PayoutLogDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<PayoutLogDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<PayoutLogDTO>>> GetAllAsync();
    Task<AppResult<PayoutLogDTO>> Create(int purchaseOrderId, int customerId, decimal amount, int status, string remarks, string payload);
    Task<AppResult<PayoutLogDTO>> Update(int id, int? status, string? remarks);
    Task<AppResult<IEnumerable<PayoutLogDTO>>> GetPayoutByProvider(int? id, DateTime? dateFrom, int status);
}