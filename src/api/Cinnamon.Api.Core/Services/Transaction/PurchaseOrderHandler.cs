using System.Security.Claims;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.OngoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class PurchaseOrderHandler : IPurchaseOrderHandler
{
    private readonly IPurchaseOrderData purchaseOrderData;
    private readonly ICustomerData customerData;
    private readonly IHttpContextAccessor httpContext;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IRequestPaymentHandler requestPaymentHandler;
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly IFinishTransactionHandler finishTransactionHandler;
    private readonly IValidateCouponCodeHandler validateCouponCodeHandler;

    public PurchaseOrderHandler(IPurchaseOrderData purchaseOrderData, IHttpContextAccessor httpContext,
        IGetActivityHandler getActivityHandler, ICustomerData customerData,
        IRequestPaymentHandler requestPaymentHandler, IJsonSerializationProvider jsonSerializationProvider,
        IFinishTransactionHandler finishTransactionHandler, IValidateCouponCodeHandler validateCouponCodeHandler)
    {
        this.purchaseOrderData = purchaseOrderData;
        this.httpContext = httpContext;
        this.getActivityHandler = getActivityHandler;
        this.customerData = customerData;
        this.requestPaymentHandler = requestPaymentHandler;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.finishTransactionHandler = finishTransactionHandler;
        this.validateCouponCodeHandler = validateCouponCodeHandler;
    }

    public AppResult<PurchaseOrderResult> Execute(PurchaseOrderArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<PurchaseOrderResult>.CreateFailed(ex, "An error occured in PurchaseOrderHandler");
        }
    }

    public async Task<AppResult<PurchaseOrderResult>> ExecuteAsync(PurchaseOrderArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<PurchaseOrderResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            // check activity id
            var activityRes = await getActivityHandler.ExecuteAsync(
                    new ActivityService.Interactors.GetActivityArgs {
                        ActivityId = args.ActivityId, 
                        IncludeAtivitySchedules = true,
                        IncludeCustomer = true});

            if(!activityRes.Succeeded || activityRes.Result == null)
            {
                return AppResult<PurchaseOrderResult>.CreateFailed(new ApplicationException("Invalid activity id provided"), "Invalid activity id provided");
            }

            // check activity schedule
            var activitySchedule = activityRes.Result.ActivitySchedules.FirstOrDefault(s => s.Id == args.ScheduleId);
            if(activitySchedule == null)
            {
                return AppResult<PurchaseOrderResult>.CreateFailed(new ApplicationException("Invalid schedule id provided"), "Invalid schedule id provided");
            }

            // check customer id
            var customerRes = await customerData.GetCustomerById(id);
            if(!customerRes.Succeeded || customerRes.Result == null)
            {
                return AppResult<PurchaseOrderResult>.CreateFailed(new ApplicationException(customerRes.Message), customerRes.Message);
            }
            if(customerRes.Succeeded && !customerRes.Result.IsSuccess)
            {
                return AppResult<PurchaseOrderResult>.CreateFailed(
                    new ApplicationException(customerRes.Result.ErrorInfo?.Message), "Invalid customer id provided");
            }

            decimal subTotal = activitySchedule.Price * args.NumberOfHeads;
            decimal paymentProviderFee = subTotal * .05m;
            decimal serviceFee = 50;
            decimal discount = 0;

            decimal overallTotal = subTotal + paymentProviderFee + serviceFee;
            decimal creditAmount = 0;

            // validate coupon
            if(!string.IsNullOrEmpty(args.CouponCode))
            {
                var validateCouponRes = await validateCouponCodeHandler.ExecuteAsync(new ActivityService.Interactors.ValidateCouponCodeArgs {
                    ActivityId = args.ActivityId,
                    Amount = overallTotal,
                    CouponCode = args.CouponCode
                });
                if(!validateCouponRes.Succeeded || validateCouponRes.Result == null)
                {
                    return AppResult<PurchaseOrderResult>.CreateFailed(
                        new ApplicationException(customerRes.Result.ErrorInfo?.Message), "Invalid request.");
                }
                var couopon = validateCouponRes.Result;

                // fixed amount
                if(couopon.DiscountType == 1)
                {
                    discount = couopon.Amount;
                }
                // percentage 
                else if(couopon.DiscountType == 0)
                {
                    decimal percentage = couopon.Amount / 100;
                    discount = overallTotal * percentage;
                }

                // deduct overall total to discount
                overallTotal -= discount;
            }
            
            if(args.IsCreditsApplied && customerRes.Result.Result.TotalCredits > 0)
            {
                var creditsBalance = customerRes.Result.Result.TotalCredits;
                creditAmount = overallTotal >= creditsBalance ? creditsBalance : overallTotal;
                overallTotal = overallTotal >= creditsBalance ? overallTotal - creditsBalance : 0;
            }

            // serialize students data to use later
            var payloadData = new {
                Students = args.Students.Select(s => {
                    return new {
                        Id = s.FamilyMemberId,
                        Name = s.Name
                    };
                }),
                PaymentMethod = args.PaymentMethod,
                PaymentChannel = args.PaymentChannel ?? string.Empty,
                Fees = new {
                    PaymentProviderFee = paymentProviderFee,
                    ServiceFee = serviceFee
                }
            };
            var serializedPayload = jsonSerializationProvider.Serialize(payloadData);

            var result = await purchaseOrderData.CreatePurchaseOrder(new Framework.ApiCommand.ApiData.PurchaseOrder.Request.CreatePurchaseOrderArgs {
                ActivityId = args.ActivityId,
                ConvinienceFee = paymentProviderFee + serviceFee,
                Coupon = args.CouponCode ?? string.Empty,
                CouponAmount = discount,
                CustomerId = id,
                OverallTotal = overallTotal,
                ScheduleId = args.ScheduleId,
                Total = subTotal,
                // if overall total is 0 due to applied credits,
                // then status should be 1 no need to send transation to payment gateway
                Status = overallTotal == 0 ? (int)TransactionStatus.Success : (int)TransactionStatus.Pending,
                Payload = serializedPayload,
                CreditAmount = creditAmount,
                UnitCount = args.Students.Count(),
                UnitPrice = activitySchedule.Price
            });

            if(!result.Succeeded || result.Result == null)
            {
                return AppResult<PurchaseOrderResult>.CreateFailed(new ApplicationException(result.Message), result.Message);
            }

            if(result.Succeeded && !result.Result.IsSuccess)
            {
                return AppResult<PurchaseOrderResult>.CreateFailed(
                    new ApplicationException(result.Result.ErrorInfo?.Message), "An error occured in PurchaseOrderHandler");
            }

            if(overallTotal == 0)
            {
                var finishTransaction = await finishTransactionHandler.ExecuteAsync(new FinishTransactionArgs {
                    TransactionId = result.Result.Result.Id
                });
                if(!finishTransaction.Succeeded || finishTransaction.Result == null)
                {
                    return AppResult<PurchaseOrderResult>.CreateFailed(
                    new ApplicationException(finishTransaction.Message), finishTransaction.Message);
                }

                return AppResult<PurchaseOrderResult>.CreateSucceeded(new PurchaseOrderResult {
                    Action = 0,
                    Id = result.Result.Result.Id,
                    Url = string.Empty
                }, "Successfully request purchase order details"); 
            }

            var requestPayment = await requestPaymentHandler.ExecuteAsync(new RequestPaymentArgs {
                Amount = (subTotal + paymentProviderFee + serviceFee) - creditAmount,
                AmountCurrency = "PHP",
                CustomerId = id,
                PaymentChannel = args.PaymentChannel ?? string.Empty,
                PaymentMethod = args.PaymentMethod,
                TransactionId = result.Result.Result.Id,
                CardInformation = args.CardInformation != null ? new RequestPaymentArgs.CardDetails {
                    AccountHolder = args.CardInformation.AccountHolder,
                    CardNumber = args.CardInformation.CardNumber,
                    CVV = args.CardInformation.CVV,
                    ExpireMonthYear = args.CardInformation.ExpireMonthYear
                } : null
            });

            if(!requestPayment.Succeeded || requestPayment.Result == null)
            {
                return AppResult<PurchaseOrderResult>.CreateFailed(new ApplicationException(requestPayment.Message), requestPayment.Message);
            }

            return AppResult<PurchaseOrderResult>.CreateSucceeded(new PurchaseOrderResult {
                Action = requestPayment.Result.Action,
                Id = result.Result.Result.Id,
                Url = requestPayment.Result.Url
            }, "Successfully request purchase order details");
        }
        catch (Exception ex)
        {
            return AppResult<PurchaseOrderResult>.CreateFailed(ex, "An error occured in PurchaseOrderHandler");
        }
    }
}