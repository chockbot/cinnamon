using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class GetPurchaseOrderHandler : IGetPurchaseOrderHandler
{
    private readonly IPurchaseOrderData purchaseOrderData;
    private readonly IHttpContextAccessor httpContext;
    private readonly IJsonSerializationProvider jsonSerialization;

    public GetPurchaseOrderHandler(IPurchaseOrderData purchaseOrderData, IHttpContextAccessor httpContext,
        IJsonSerializationProvider jsonSerialization)
    {
        this.purchaseOrderData = purchaseOrderData;
        this.httpContext = httpContext;
        this.jsonSerialization = jsonSerialization;
    }

    public AppResult<GetPurchaseOrderResult> Execute(GetPurchaseOrderArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetPurchaseOrderResult>.CreateFailed(ex, "An error occured in PurchaseOrderHandler");
        }
    }

    public async Task<AppResult<GetPurchaseOrderResult>> ExecuteAsync(GetPurchaseOrderArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<GetPurchaseOrderResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            var result = await purchaseOrderData.GetPurchaseOrderById(args.PurchaseOrderId);
            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<GetPurchaseOrderResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }
            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<GetPurchaseOrderResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "Invalid purchase order id");
            }

            if(result.Result.Result.CustomerId != id)
            {
                return AppResult<GetPurchaseOrderResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "Invalid purchase order id");
            }
            var purchaseOrder = result.Result.Result;

            var deserializedPayload = jsonSerialization.Deserialize<Payload>(purchaseOrder.Payload);
            int enroleeCount = 0;
            string paymentMethod = string.Empty;

            if(deserializedPayload != null)
            {
                enroleeCount = deserializedPayload.Students.Count();
                paymentMethod = deserializedPayload.PaymentChannel;
            }

            return AppResult<GetPurchaseOrderResult>.CreateSucceeded(new GetPurchaseOrderResult {
                ActivityId = purchaseOrder.ActivityId,
                ConvinienceFee = purchaseOrder.ConvinienceFee,
                Coupon = purchaseOrder.Coupon,
                CouponAmount = purchaseOrder.CouponAmount,
                CustomerId = purchaseOrder.CustomerId,
                Id = purchaseOrder.Id,
                OverallTotal = purchaseOrder.OverallTotal,
                ScheduleId = purchaseOrder.ScheduleId,
                Total = purchaseOrder.Total,
                EnrolleeCount = enroleeCount,
                PaymentMethod = paymentMethod
            }, "Successfully get purhase order details");
        }
        catch (Exception ex)
        {
            return AppResult<GetPurchaseOrderResult>.CreateFailed(ex, "An error occured in PurchaseOrderHandler");
        }
    }

    class Payload 
    {
        public IEnumerable<Student> Students {get; set;}
        public string PaymentChannel {get; set;}
    }

    class Student 
    {
        public int Id {get; set;}
        public string Name {get; set;}
    }
}