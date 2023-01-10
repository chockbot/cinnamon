using Cinnamon.Core.Common;
using Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Response;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers;

public interface ITransactionApiHandler 
{
    Task<AppResult<SubmitPurchaseOrderResult>> SubmitPurchaseOrder(SubmitPurchaseOrderArgs args, string token);
    Task<AppResult<GetPurchaseOrderResult>> GetPurchaseOrder(int id, string token);
} 
