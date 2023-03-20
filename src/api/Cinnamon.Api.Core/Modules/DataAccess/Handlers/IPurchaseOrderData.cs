using Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;
using Cinnamon.Framework.ApiCommand.ApiData.Activity.Response;
using Cinnamon.Framework.ApiCommand.ApiData.PurchaseOrder.Request;
using Cinnamon.Framework.ApiCommand.ApiData.PurchaseOrder.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Modules.DataAccess.Handlers
{
    public interface IPurchaseOrderData
    {
        Task<AppResult<GetPurchaseOrderResult>> GetPurchaseOrderById(int id);
        Task<AppResult<GetAllPurchaseOrderResult>> GetAllPurchaseOrder(GetAllPurchaseOrderArgs args);
        Task<AppResult<CreatePurchaseOrderResult>> CreatePurchaseOrder(CreatePurchaseOrderArgs args);
        Task<AppResult<UpdatePurchaseOrderResult>> UpdatePurchaseOrder(UpdatePurchaseOrderArgs args);
        Task<AppResult<GetAllPurchaseOrderResult>> GetAllPurchaseOrderNeedToPayout();
    }
}
