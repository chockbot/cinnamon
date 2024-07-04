using Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Request;
using Cinnamon.Framework.ApiCommand.ApiCore.Transaction.Response;
using Cinnamon.Framework.ApiCommand.ApiData.DirectStudent.Request;
using Cinnamon.Framework.Common;

namespace Cinnamon.Web.Modules.ApiAccess.Handlers;

public interface ITransactionApiHandler 
{
    Task<AppResult<SubmitPurchaseOrderResult>> SubmitPurchaseOrder(SubmitPurchaseOrderArgs args, string token);
    Task<AppResult<GetPurchaseOrderResult>> GetPurchaseOrder(int id, string token);
    Task<AppResult<GetGrossSalesByProviderResult>> GetGrossSalesByProvider(GetGrossSalesByProviderArgs args, string token);
    Task<AppResult<GetPayoutsByProviderResult>> GetPayoutsByProvider(GetPayoutsByProviderArgs args, string token);
    Task<AppResult<SubmitOtePurchaseOrderResult>> SubmitOtePurchaseOrder(SubmitOtePurchaseOrderArgs args, string token);
    Task<AppResult<OteGetPurchaseOrderResult>> GetOtePurchaseOrder(int id, string token);
    Task<AppResult<TransactionRedirectionResult>> TransactionRedirection(TransactionRedirectionArgs args);
    Task<AppResult<GetDirectStudentSalesResult>> GetDirectStudentSales(GetDirectStudentSalesArgs args, string token);
} 
