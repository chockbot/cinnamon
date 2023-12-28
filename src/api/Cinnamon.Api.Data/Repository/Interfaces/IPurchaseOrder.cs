using Cinnamon.Api.Data.Repository.Entities;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.PurchaseOrder;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.Student;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Data.Repository.Interfaces;

public interface IPurchaseOrder : IGenericEntity<PurchaseOrder>
{
    Task<AppResult<IEnumerable<PurchaseOrder>>> GetAllPurchaseOrdersNeedPayout();
    Task<AppResult<IEnumerable<PurchaseOrder>>> UpdatePurchaseOrdersByStatus(IEnumerable<PurchaseOrder> purchaseOrders);
    Task<AppResult<IEnumerable<InclusivePurchaseOrderDTO>>> GetInclusiveTransaction(string? name, string? email, int? status, DateTime? dateFrom, DateTime? dateTo);
    Task<AppResult<IEnumerable<PurchaseOrder>>> GetGrossSalesByProvider(int? Id, DateTime? dateFrom);
    Task<AppResult<IEnumerable<DisburseStudentDTO>>> AddOnsNeedToDisburse();
}