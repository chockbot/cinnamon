using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;
using System.Security.Claims;

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

            if (deserializedPayload != null)
            {
                enroleeCount = deserializedPayload.Students.Count();
                paymentMethod = deserializedPayload.PaymentChannel;
            }

            return AppResult<GetPurchaseOrderResult>.CreateSucceeded(new GetPurchaseOrderResult {
                ActivityId         = purchaseOrder.ActivityId,
                ConvinienceFee     = purchaseOrder.ConvinienceFee,
                Coupon             = purchaseOrder.Coupon,
                CouponAmount       = purchaseOrder.CouponAmount,
                CustomerId         = purchaseOrder.CustomerId,
                Id                 = purchaseOrder.Id,
                OverallTotal       = purchaseOrder.OverallTotal,
                ScheduleId         = purchaseOrder.ScheduleId,
                Total              = purchaseOrder.Total,
                EnrolleeCount      = enroleeCount,
                PaymentMethod      = paymentMethod,
                ServiceFee         = deserializedPayload != null ? deserializedPayload.Fees.ServiceFee : 0,
                PaymentProviderFee = deserializedPayload != null ? deserializedPayload.Fees.PaymentProviderFee : 0,
                AppliedCredits     = purchaseOrder.CreditAmount,
                IsInclusivePayment = deserializedPayload?.IsInclusivePayment ?? false,
                AddOnsAmount       = purchaseOrder.AddOnsAmount,
                AddOnsDetails      = deserializedPayload != null ? deserializedPayload.AddOnsDetails.Select(s =>
                {
                    return new GetPurchaseOrderResult.AddOnDetail
                    {
                        AddOnId      = s.AddOnId,
                        AddOnName    = s.AddOnName
                    };
                }) : Enumerable.Empty<GetPurchaseOrderResult.AddOnDetail>(),
            }, "Successfully get purchase order details");
        }
        catch (Exception ex)
        {
            return AppResult<GetPurchaseOrderResult>.CreateFailed(ex, "An error occurred in PurchaseOrderHandler");
        }
    }

    class Payload 
    {
        public IEnumerable<Student> Students {get; set;}
        public IEnumerable<AddOn> AddOnsDetails { get; set; }
        public Fees Fees {get; set;}
        public string PaymentChannel {get; set;}
        public bool IsInclusivePayment {get; set;}

    }

    class Student 
    {
        public int Id {get; set;}
        public string Name {get; set;}
    }

    class AddOn
    {
        public int AddOnId { get; set; }
        public string AddOnName { get; set; }
    }

    class Fees {
        public decimal PaymentProviderFee {get; set;}
        public decimal ServiceFee {get; set;}
    }
}