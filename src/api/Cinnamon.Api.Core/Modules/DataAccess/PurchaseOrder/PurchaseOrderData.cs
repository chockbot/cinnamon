using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Framework.ApiCommand.ApiData.PurchaseOrder.Request;
using Cinnamon.Framework.ApiCommand.ApiData.PurchaseOrder.Response;
using Cinnamon.Framework.Common;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Cinnamon.Api.Core.Modules.DataAccess.PurchaseOrder
{
    public class PurchaseOrderData : IPurchaseOrderData
    {
        private readonly IFlurlClient _flurlClient;
        
        public PurchaseOrderData(ApplicationConfig config, IFlurlClientFactory flurlFac)
        {
            _flurlClient = flurlFac.Get(config.ApiDataUrl);
        }

        public async Task<AppResult<CreatePurchaseOrderResult>> CreatePurchaseOrder(CreatePurchaseOrderArgs args)
        {
            try
            {
                var result = await _flurlClient
                    .Request("PurchaseOrder/CreatePurchaseOrder")
                    .PostJsonAsync(args)
                    .ReceiveJson<CreatePurchaseOrderResult>();

                return AppResult<CreatePurchaseOrderResult>.CreateSucceeded(result, "Successfully posting create Purchase Order api");
            }
            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseJsonAsync();
                return AppResult<CreatePurchaseOrderResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<CreatePurchaseOrderResult>.CreateFailed(ex, "An error occured when posting create Purchase Order api");
            }
        }

        public async Task<AppResult<GetAllPurchaseOrderResult>> GetAllPurchaseOrder(GetAllPurchaseOrderArgs args)
        {
            try
            {
                var result = await _flurlClient
                                .Request("PurchaseOrder/GetAllPurchaseOrder")
                                .SetQueryParams(args)
                                .GetJsonAsync<GetAllPurchaseOrderResult>();

                return AppResult<GetAllPurchaseOrderResult>.CreateSucceeded(result, "Successfully getting get all PurchaseOrder api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetAllPurchaseOrderResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetAllPurchaseOrderResult>.CreateFailed(ex, "An error occured when getting all PurchaseOrder api");
            }
        }

        public async Task<AppResult<GetPurchaseOrderResult>> GetPurchaseOrderById(int id)
        {
            try
            {
                var result = await _flurlClient
                                .Request($"PurchaseOrder/GetPurchaseOrderById/{id}")
                                .GetJsonAsync<GetPurchaseOrderResult>();

                return AppResult<GetPurchaseOrderResult>.CreateSucceeded(result, "Successfully getting purchase order by id api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetPurchaseOrderResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetPurchaseOrderResult>.CreateFailed(ex, "An error occured when getting purchase order by id api");
            }
        }

        public async Task<AppResult<UpdatePurchaseOrderResult>> UpdatePurchaseOrder(UpdatePurchaseOrderArgs args)
        {
            try
            {
                var result = await _flurlClient
                                .Request("PurchaseOrder/UpdatePurchaseOrder")
                                .PostJsonAsync(args)
                                .ReceiveJson<UpdatePurchaseOrderResult>();

                return AppResult<UpdatePurchaseOrderResult>.CreateSucceeded(result, "Successfully posting update purchase order api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<UpdatePurchaseOrderResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<UpdatePurchaseOrderResult>.CreateFailed(ex, "An error occured when posting update purchase order api");
            }
        }

        public async Task<AppResult<GetAllPurchaseOrderResult>> GetAllPurchaseOrderNeedToPayout()
        {
            try
            {
                var result = await _flurlClient
                                .Request("PurchaseOrder/GetAllPurchaseOrderNeedToPayout")
                                .GetJsonAsync<GetAllPurchaseOrderResult>();

                return AppResult<GetAllPurchaseOrderResult>.CreateSucceeded(result, "Successfully getting get all PurchaseOrder api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetAllPurchaseOrderResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetAllPurchaseOrderResult>.CreateFailed(ex, "An error occured when getting all PurchaseOrder api");
            }
        }

        public async Task<AppResult<UpdatePurchaseOrdersStatusResult>> UpdatePurchaseOrdersStatus(UpdatePurchaseOrdersStatusArgs args)
        {
            try
            {
                var result = await _flurlClient
                                .Request("PurchaseOrder/UpdatePurchaseOrdersStatus")
                                .PostJsonAsync(args)
                                .ReceiveJson<UpdatePurchaseOrdersStatusResult>();

                return AppResult<UpdatePurchaseOrdersStatusResult>.CreateSucceeded(result, "Successfully posting update purchase order api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<UpdatePurchaseOrdersStatusResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<UpdatePurchaseOrdersStatusResult>.CreateFailed(ex, "An error occured when posting update purchase order api");
            }
        }

        public async Task<AppResult<GetAllInclusiveTransactionResult>> GetAllInclusiveTransactions(GetAllInclusiveTransactionArgs args)
        {
            try
            {
                var result = await _flurlClient
                                .Request("PurchaseOrder/GetAllInclusiveTransactions")
                                .SetQueryParams(args)
                                .GetJsonAsync<GetAllInclusiveTransactionResult>();

                return AppResult<GetAllInclusiveTransactionResult>.CreateSucceeded(result, "Successfully getting get all PurchaseOrder api");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetAllInclusiveTransactionResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetAllInclusiveTransactionResult>.CreateFailed(ex, "An error occured when getting all PurchaseOrder api");
            }
        }

        public async Task<AppResult<GetGrossSalesByProviderResult>> GetGrossSalesByProvider(GetGrossSalesByProviderArgs args)
        {
            try
            {
                var result = await _flurlClient
                                .Request("PurchaseOrder/GetGrossSalesByProvider")
                                .GetJsonAsync<GetGrossSalesByProviderResult>();

                return AppResult<GetGrossSalesByProviderResult>.CreateSucceeded(result, "Successfully getting get gross sales");
            }
            catch (FlurlHttpException ex)
            {
                return AppResult<GetGrossSalesByProviderResult>.CreateFailed(ex, ex.Message);
            }
            catch (Exception ex)
            {
                return AppResult<GetGrossSalesByProviderResult>.CreateFailed(ex, "An error occured when getting gross sales");
            }
        }
    }
}
