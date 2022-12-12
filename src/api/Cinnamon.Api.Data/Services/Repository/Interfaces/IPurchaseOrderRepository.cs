using Cinnamon.Framework.Common;
using Cinnamon.Api.Data.Services.Repository.PurchaseOrder.DTO;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IPurchaseOrderRepository 
{
    Task<AppResult<PurchaseOrderDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<PurchaseOrderDTO>>> GetAllAsync(int? count, int? skip);
    Task<AppResult<IEnumerable<PurchaseOrderDTO>>> GetAllAsync();
    Task<AppResult<PurchaseOrderDTO>> Create(int activityId, int scheduleId, int customerId, decimal total, decimal convinienceFee,
        string? coupon, decimal? couponAmount, decimal overallTotal);
    Task<AppResult<PurchaseOrderDTO>> Update(int purchaseOrderId, int? scheduleId, decimal? total, decimal? convinienceFee,
        string? coupon, decimal? couponAmount, decimal? overallTotal);
}