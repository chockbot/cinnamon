using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;
using Flurl;
using System.Security.Claims;

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
    private readonly IOwnerPricingInclusiveHandler ownerPricingInclusiveHandler;
    private readonly IValidateCouponCodeHandler validateCouponCodeHandler;
    private readonly ICustomerPricingData customerPricingData;
    private readonly ILogger logger;
    private readonly ApplicationConfig applicationConfig;
    private readonly ITokenGeneratedData tokenGeneratedData;
    private readonly ITokenGeneratorProvider tokenGeneratorProvider;

    public PurchaseOrderHandler(IPurchaseOrderData purchaseOrderData, IHttpContextAccessor httpContext,
        IGetActivityHandler getActivityHandler, ICustomerData customerData,
        IRequestPaymentHandler requestPaymentHandler, IJsonSerializationProvider jsonSerializationProvider,
        IFinishTransactionHandler finishTransactionHandler, IOwnerPricingInclusiveHandler ownerPricingInclusiveHandler,
        IValidateCouponCodeHandler validateCouponCodeHandler, ICustomerPricingData customerPricingData,
        ILogger<PurchaseOrderHandler> logger, ApplicationConfig applicationConfig, ITokenGeneratedData tokenGeneratedData,
        ITokenGeneratorProvider tokenGeneratorProvider)
    {
        this.purchaseOrderData = purchaseOrderData;
        this.httpContext = httpContext;
        this.getActivityHandler = getActivityHandler;
        this.customerData = customerData;
        this.requestPaymentHandler = requestPaymentHandler;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.finishTransactionHandler = finishTransactionHandler;
        this.ownerPricingInclusiveHandler = ownerPricingInclusiveHandler;
        this.validateCouponCodeHandler = validateCouponCodeHandler;
        this.customerPricingData = customerPricingData;
        this.logger = logger;
        this.applicationConfig = applicationConfig;
        this.tokenGeneratedData = tokenGeneratedData;
        this.tokenGeneratorProvider = tokenGeneratorProvider;
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
            

            // check if inclusive payment
            var checkInclusiveRes = await ownerPricingInclusiveHandler.ExecuteAsync(new ActivityService.Interactors.OwnerPricingInclusiveArgs {
                CustomerId = activityRes.Result.Owner?.Id ?? 0
            });
            if(!checkInclusiveRes.Succeeded || checkInclusiveRes.Result == null)
            {
                logger.LogError("Error in purchase order handler");
                logger.LogError(checkInclusiveRes.Error.Description);
                logger.LogError(checkInclusiveRes.Message);
                return AppResult<PurchaseOrderResult>.CreateFailed(new ApplicationException("An error occured. Please try again"), "An error occured. Please try again");
            }

            bool IsInclusivePayment = checkInclusiveRes.Result.IsInclusivePricing;

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
            decimal subTotal           = (activitySchedule != null ? activitySchedule.Price : 0) * args.NumberOfHeads;
            decimal addOnsTotal        = args.AddOnsAmount;
            decimal paymentProviderFee = IsInclusivePayment ? 0 : subTotal * 0; //.05m;
            decimal discount           = 0;
            decimal serviceFee         = IsInclusivePayment ? 0 : 39; //50;
            decimal overallTotal       = subTotal + addOnsTotal + paymentProviderFee + serviceFee;
            decimal creditAmount       = 0;
          

            decimal perUnitDisburseAmount = activitySchedule != null ? activitySchedule.Price : 0;
            decimal totalDisburseAmount = subTotal;

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

                decimal disbursementDisccount = 0;

                // fixed amount
                if(couopon.DiscountType == 1)
                {
                    discount = couopon.Amount;
                    disbursementDisccount = couopon.Amount;
                }
                // percentage 
                else if(couopon.DiscountType == 0)
                {
                    decimal percentage = couopon.Amount / 100;
                    decimal couponSubTotal = 0;
                    if(IsInclusivePayment)
                    {
                        couponSubTotal = subTotal;
                    }
                    else 
                    {
                        couponSubTotal = subTotal + paymentProviderFee;
                    }

                    discount = couponSubTotal * percentage;
                    disbursementDisccount = subTotal * percentage;
                }

                // deduct from disbursement amount
                perUnitDisburseAmount -= (disbursementDisccount / args.NumberOfHeads);
                totalDisburseAmount -= disbursementDisccount;

                // deduct overall total to discount
                overallTotal -= discount;
            }
            
            // disbursement for inclusive pricing
            if(IsInclusivePayment)
            {
                var customerPricingRes = await customerPricingData.GetCustomerPricingByCustomerId(activityRes.Result.Owner?.Id ?? 0);
                if(!customerPricingRes.Succeeded || customerPricingRes.Result == null || !customerPricingRes.Result.IsSuccess)
                {
                    return AppResult<PurchaseOrderResult>.CreateFailed(
                        new ApplicationException(customerRes.Result.ErrorInfo?.Message), "Invalid request.");
                }
                var customerPricing = customerPricingRes.Result.Result;

                decimal amountToDeduct = 0;
                var percentage = customerPricing.Rate / 100;
                amountToDeduct = percentage * perUnitDisburseAmount;

                perUnitDisburseAmount -= amountToDeduct;
                totalDisburseAmount -= (amountToDeduct * args.NumberOfHeads);
            }
            
            if(args.IsCreditsApplied && customerRes.Result.Result.TotalCredits > 0)
            {
                var creditsBalance = customerRes.Result.Result.TotalCredits;
                creditAmount = overallTotal >= creditsBalance ? creditsBalance : overallTotal;
                overallTotal = overallTotal >= creditsBalance ? overallTotal - creditsBalance : 0;
            }

            // zero out over all total if less than zero
            overallTotal = overallTotal < 0 ? 0 : overallTotal;

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
                },
                IsInclusivePayment,
                SelectedPeriod = args.SelectedPeriod,
                AddOnsDetails = args.AddOnsDetails.Select(a =>
                {
                    return new
                    {
                        AddOnId = a.AddOnId,
                        AddOnName = a.AddOnName,
                        AddOnCount = a.AddOnCount,
                    };
                })
            };
            var serializedPayload = jsonSerializationProvider.Serialize(payloadData);

            var result = await purchaseOrderData.CreatePurchaseOrder(new Framework.ApiCommand.ApiData.PurchaseOrder.Request.CreatePurchaseOrderArgs {
                ActivityId     = args.ActivityId,
                ConvinienceFee = paymentProviderFee + serviceFee,
                Coupon         = args.CouponCode ?? string.Empty,
                CouponAmount   = discount,
                CustomerId     = id,
                OverallTotal   = overallTotal,
                ScheduleId     = args.ScheduleId,
                Total          = subTotal,
                // if overall total is 0 due to applied credits,
                // then status should be 1 no need to send transaction to payment gateway
                Status                = overallTotal == 0 ? (int)TransactionStatus.Success : (int)TransactionStatus.Pending,
                Payload               = serializedPayload,
                CreditAmount          = creditAmount,
                UnitCount             = args.Students.Count(),
                UnitPrice             = activitySchedule != null ? activitySchedule.Price : 0,
                IsInclusivePayment    = IsInclusivePayment,
                PerUnitDisburseAmount = perUnitDisburseAmount,
                TotalDisburseAmount   = totalDisburseAmount,
                AddOnsAmount          = args.AddOnsAmount,
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
                    TransactionId = result.Result.Result.Id,
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

            var successUrl = applicationConfig.FrontendUrl
                .AppendPathSegment("purchase/order")
                .SetQueryParam("purchaseid", result.Result.Result.Id);

            var failedUrl = applicationConfig.FrontendUrl
                .AppendPathSegment($"payment/{args.ActivityId}/{args.ScheduleId}")
                .SetQueryParam("Status", "failed");
            
            // generate token and guid for transaction redirection details
            var tokenGenerated = tokenGeneratorProvider.Generator();
            var payload = new 
            {
                ActivityId = result.Result.Result.ActivityId,
                TransactionId = result.Result.Result.Id,
                ScheduleId = args.ScheduleId,
                Url = successUrl,
            };

            var tokenSerializedPayload = jsonSerializationProvider.Serialize(payload);

            var createTokenRes = await tokenGeneratedData.CreateTokenGenerated(new Framework.ApiCommand.ApiData.TokenGenerated.Request.CreateTokenArgs {
                Guid = tokenGenerated.Guid,
                Payload = tokenSerializedPayload,
                Token = tokenGenerated.Token,
                TokenType = "TRANSACTION-REQUEST",
            });
            if(!createTokenRes.Succeeded || createTokenRes.Result is null || !createTokenRes.Result.IsSuccess)
            {
                return AppResult<PurchaseOrderResult>.CreateFailed(
                    new ApplicationException(createTokenRes.Result?.ErrorInfo?.Message), "An error occured in PurchaseOrderHandler");
            }

            var paymentRedirectUrl = applicationConfig.FrontendUrl
                                        .AppendPathSegment($"/transaction/finalize")
                                        .SetQueryParam("Guid", tokenGenerated.Guid)
                                        .SetQueryParam("Token", tokenGenerated.Token);

            var requestPayment = await requestPaymentHandler.ExecuteAsync(new RequestPaymentArgs {
                Amount = overallTotal - creditAmount,
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
                } : null,
                SuccessUrl = paymentRedirectUrl,
                FailedUrl = paymentRedirectUrl
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