using Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Response;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers;

public interface ITransactionApiHandler 
{
    Task<AppResult<SubmitPurchaseOrderResult>> SubmitPurchaseOrder(SubmitPurchaseOrderArgs args, string token);
    Task<AppResult<GetPurchaseOrderResult>> GetPurchaseOrder(int id, string token);
} 
