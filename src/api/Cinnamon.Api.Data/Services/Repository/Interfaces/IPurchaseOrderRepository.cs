using Cinnamon.Framework.Common;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.PurchaseOrder;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Student;

namespace Cinnamon.Api.Data.Services.Repository.Interfaces;

public interface IPurchaseOrderRepository 
{
    Task<AppResult<PurchaseOrderDTO>> GetByIdAsync(int id);
    Task<AppResult<IEnumerable<PurchaseOrderDTO>>> GetAllAsync(int? count, int? skip, 
        bool? includeActivity, bool? includeSchedule, int? customerId, int? status);
    Task<AppResult<IEnumerable<PurchaseOrderDTO>>> GetAllAsync();
    Task<AppResult<PurchaseOrderDTO>> Create(int activityId, int scheduleId, int customerId, decimal total, decimal convinienceFee,
        string? coupon, decimal? couponAmount, decimal overallTotal, int status, string payload, decimal creditAmount,
            decimal unitPrice, int unitCount, bool isInclusivePayment, decimal perUnitDisburseAmount, decimal totalDisburseAmount, decimal addOnsAmount);
    Task<AppResult<PurchaseOrderDTO>> Update(int purchaseOrderId, int? scheduleId, decimal? total, decimal? convinienceFee,
        string? coupon, decimal? couponAmount, decimal? overallTotal, int? status, decimal? creditAmount,
        decimal? unitPrice, int? unitCount, string? pgPayload);
    Task<AppResult<IEnumerable<PurchaseOrderDTO>>> GetAllPurchaseOrderNeedToPayout();
    Task<AppResult<IEnumerable<PurchaseOrderDTO>>> UpdatePurchaseOrdersStatus(IEnumerable<int> ids, int status);
    Task<AppResult<IEnumerable<InclusivePurchaseOrderDTO>>> GetInclusiveTransactions(string? name, string? email, int? status, DateTime? dateFrom, DateTime? dateTo);
    Task<AppResult<IEnumerable<PurchaseOrderDTO>>> GetGrossSalesByProvider(int? id, DateTime? dateFrom);
    Task<AppResult<IEnumerable<DisburseStudentDTO>>> GetAllOteNeedToDisburse();
}