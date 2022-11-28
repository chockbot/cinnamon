using Cinnamon.Core.Models;

namespace Cinnamon.Core;

public interface IPurchaseOrder : IBaseTable<PurchaseOrderModel> 
{
    Task<PurchaseOrderModel> GetPurchaseByIdAsync(int id);
}