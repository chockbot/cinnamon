using System.Security.Claims;
using System.Text;
using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;
using Flurl;
using Microsoft.AspNetCore.WebUtilities;
using QRCoder;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class OtePurchaseOrderHandler : IOtePurchaseOrderHandler
{
    private readonly IPurchaseOrderData purchaseOrderData;
    private readonly ICustomerData customerData;
    private readonly IHttpContextAccessor httpContext;
    private readonly IRequestPaymentHandler requestPaymentHandler;
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly IValidateCouponCodeHandler validateCouponCodeHandler;
    private readonly ICustomerPricingData customerPricingData;
    private readonly IOteFindByHandler oteFindByHandler;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IOwnerPricingInclusiveHandler ownerPricingInclusiveHandler;
    private readonly ILogger<OtePurchaseOrderHandler> logger;
    private readonly ApplicationConfig applicationConfig;

    public OtePurchaseOrderHandler(IPurchaseOrderData purchaseOrderData, ICustomerData customerData,
        IHttpContextAccessor httpContext, IRequestPaymentHandler requestPaymentHandler, IJsonSerializationProvider jsonSerializationProvider,
        IValidateCouponCodeHandler validateCouponCodeHandler, ICustomerPricingData customerPricingData, ILogger<OtePurchaseOrderHandler> logger,
        IOteFindByHandler oteFindByHandler, IGetActivityHandler getActivityHandler, IOwnerPricingInclusiveHandler ownerPricingInclusiveHandler,
        ApplicationConfig applicationConfig)
    {
        this.purchaseOrderData = purchaseOrderData;
        this.customerData = customerData;
        this.httpContext = httpContext;
        this.requestPaymentHandler = requestPaymentHandler;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.validateCouponCodeHandler = validateCouponCodeHandler;
        this.customerPricingData = customerPricingData;
        this.oteFindByHandler = oteFindByHandler;
        this.getActivityHandler = getActivityHandler;
        this.ownerPricingInclusiveHandler = ownerPricingInclusiveHandler;
        this.logger = logger;
        this.applicationConfig = applicationConfig;
    }
    
    public AppResult<OtePurchaseOrderResult> Execute(OtePurchaseOrderArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<OtePurchaseOrderResult>.CreateFailed(ex, "An error occured when purchasing one time event");
        }
    }

    public async Task<AppResult<OtePurchaseOrderResult>> ExecuteAsync(OtePurchaseOrderArgs args)
    {
        try
        {
            // get customer id saved in claims
            var customerId = httpContext.HttpContext?.User.FindFirstValue("UserId");
            if(customerId == null)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            int id = Convert.ToInt32(customerId);

            bool isEmptyTicket = args.Tickets.Count() == 0;
            if(isEmptyTicket)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException("Select ticket to purchase. Invalid request."), "Select ticket to purchase. Invalid request.");
            }

            var checkActivityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                ActivityId = args.ActivityId
            });
            if(!checkActivityRes.Succeeded || checkActivityRes.Result is null)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException("Unable to identify selected one time event."), "Unable to identify selected one time event.");
            }

            var oteHandlerRes = await oteFindByHandler.ExecuteAsync(new ActivityService.Interactors.OteFindByHandlerArgs {
                Handler = checkActivityRes.Result.Handler,
                IncludeAddress = true,
                IncludeImages = true,
                IncludePricing = true,
                IncludeSchedule = true,
            });
            if(!oteHandlerRes.Succeeded || oteHandlerRes.Result is null)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException("Unable to identify selected one time event."), "Unable to identify selected one time event.");
            }
            var oteActivity = oteHandlerRes.Result;

            // filter tickets available only to ticket date
            var defaultDatePricing = oteActivity.Pricings.FirstOrDefault(p => p.Id == args.Tickets.First().Id);
            if(defaultDatePricing is null)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException("Unable to identify selected one time event."), "Unable to identify selected one time event.");
            }
            var fileteredAvailableTickets = oteActivity.Pricings.Where(p => p.OteDateId == defaultDatePricing.OteDateId);

            var checkInclusivePaymentRes = await ownerPricingInclusiveHandler.ExecuteAsync(new ActivityService.Interactors.OwnerPricingInclusiveArgs {
                CustomerId = oteActivity.ProviderId
            });
            if(!checkInclusivePaymentRes.Succeeded || checkInclusivePaymentRes.Result is null)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException("Unable to identify provider payment type."), "Unable to identify provider payment type.");
            }
            bool isInclusivePayment = checkInclusivePaymentRes.Result.IsInclusivePricing;

            var customerAccountRes = await customerData.GetCustomerById(id);
            if(!customerAccountRes.Succeeded || customerAccountRes.Result is null || !customerAccountRes.Result.IsSuccess)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException("Unable to identify customer account."), "Unable to identify customer account.");
            }
            var customer = customerAccountRes.Result.Result;

            var selectedTickets = new List<Ticket>();
            // validate selected tickets
            foreach(var ticket in args.Tickets)
            {
                var ticketPrice = fileteredAvailableTickets.FirstOrDefault(t => t.Id == ticket.Id);
                if(ticketPrice is null)
                {
                    return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException("Unable to identify selected ticket."), "Unable to identify selected ticket.");
                }

                if(ticketPrice.TicketSold >= ticketPrice.MaxSlots)
                {
                    return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException("Tickets already sold out."), "Tickets already sold out.");
                }

                if((ticketPrice.MaxSlots - ticketPrice.TicketSold) < ticket.Count)
                {
                    return AppResult<OtePurchaseOrderResult>.CreateFailed(
                        new ApplicationException("Some of the tickets already sold. Refresh the page and update your tickets."), "Some of the tickets already sold. Refresh the page and update your tickets.");
                }

                // create selected ticket instance
                for(int i = 0; i < ticket.Count; i++)
                {
                    var qrcode = CreateCode();
                    selectedTickets.Add(new Ticket {
                        Id = ticketPrice.Id,
                        Name = ticketPrice.Name,
                        Price = ticketPrice.Price,
                        Code = qrcode,
                        ImageData = GenerateQRCode(qrcode),
                        OteDateId = defaultDatePricing.OteDateId
                    });
                }
            }

            decimal subTotal = selectedTickets.Sum(t => t.Price);
            decimal providerFeePercent = args.PaymentMethod == "CARD" ? 5 : 3;
            decimal paymentProviderFee = isInclusivePayment ? 0 : subTotal * (providerFeePercent / 100); //.05m;
            decimal discount = 0;
            decimal serviceFee = isInclusivePayment ? 0 : 15; //50;
            decimal overallTotal = subTotal + paymentProviderFee + serviceFee;
            decimal creditAmount = 0;

            decimal perUnitDisburseAmount = subTotal;
            decimal totalDisburseAmount = subTotal;

            // validate coupon
            if(!string.IsNullOrEmpty(args.CouponCode))
            {
                var validateCouponRes = await validateCouponCodeHandler.ExecuteAsync(new ActivityService.Interactors.ValidateCouponCodeArgs {
                    ActivityId = oteActivity.Id,
                    Amount = overallTotal,
                    CouponCode = args.CouponCode
                });
                if(!validateCouponRes.Succeeded || validateCouponRes.Result == null)
                {
                    return AppResult<OtePurchaseOrderResult>.CreateFailed(
                        new ApplicationException("Invalid coupon code. Invalid request."), "Invalid coupon code. Invalid request.");
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
                    if(isInclusivePayment)
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
                perUnitDisburseAmount -= disbursementDisccount;
                totalDisburseAmount -= disbursementDisccount;

                // deduct overall total to discount
                overallTotal -= discount;
            }

            // disbursement for inclusive pricing
            if(isInclusivePayment)
            {
                var customerPricingRes = await customerPricingData.GetCustomerPricingByCustomerId(oteActivity.ProviderId);
                if(!customerPricingRes.Succeeded || customerPricingRes.Result == null || !customerPricingRes.Result.IsSuccess)
                {
                    return AppResult<OtePurchaseOrderResult>.CreateFailed(
                        new ApplicationException("Unable to identity provider pricing details."), "Unable to identity provider pricing details.");
                }
                var customerPricing = customerPricingRes.Result.Result;

                decimal amountToDeduct = 0;
                var percentage = customerPricing.Rate / 100;
                amountToDeduct = percentage * subTotal;

                perUnitDisburseAmount -= (amountToDeduct);
                totalDisburseAmount -= (amountToDeduct);
            }

            // for credits
            if(args.IsCreditsApplied && customer.TotalCredits > 0)
            {
                var creditsBalance = customer.TotalCredits;
                creditAmount = overallTotal >= creditsBalance ? creditsBalance : overallTotal;
                overallTotal = overallTotal >= creditsBalance ? overallTotal - creditsBalance : 0;
            }

            // zero out over all total if less than zero
            overallTotal = overallTotal < 0 ? 0 : overallTotal;

            // generate token and guid
            var guid = Guid.NewGuid();
            var timestamp = DateTime.UtcNow;
            byte[] time = BitConverter.GetBytes(timestamp.ToBinary());
            byte[] key = guid.ToByteArray();
            var token = Convert.ToBase64String(time.Concat(key).ToArray());
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            
            // serialize students data to use later
            var payloadData = new {
                Tickets = selectedTickets,
                PaymentMethod = args.PaymentMethod,
                PaymentChannel = args.PaymentChannel ?? string.Empty,
                Fees = new {
                    PaymentProviderFee = paymentProviderFee,
                    ServiceFee = serviceFee
                },
                isInclusivePayment,
                OteScheduleId = oteActivity.Pricings.First().OteScheduleId,
                Guid = guid.ToString(),
                Token = encodedToken
            };
            var serializedPayload = jsonSerializationProvider.Serialize(payloadData);

            var result = await purchaseOrderData.CreatePurchaseOrder(new Framework.ApiCommand.ApiData.PurchaseOrder.Request.CreatePurchaseOrderArgs {
                ActivityId = args.ActivityId,
                ConvinienceFee = paymentProviderFee + serviceFee,
                Coupon = args.CouponCode ?? string.Empty,
                CouponAmount = discount,
                CustomerId = id,
                OverallTotal = overallTotal,
                ScheduleId = 0,
                Total = subTotal,
                // if overall total is 0 due to applied credits,
                // then status should be 1 no need to send transation to payment gateway
                Status = overallTotal == 0 ? (int)TransactionStatus.Success : (int)TransactionStatus.Pending,
                Payload = serializedPayload,
                CreditAmount = creditAmount,
                UnitCount = 1,
                UnitPrice = subTotal,
                IsInclusivePayment = isInclusivePayment,
                PerUnitDisburseAmount = perUnitDisburseAmount,
                TotalDisburseAmount = totalDisburseAmount
            });
            if(!result.Succeeded || result.Result == null || !result.Result.IsSuccess)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(
                    new ApplicationException("Unable to create purchase order transaction."), "Unable to create purchase order transaction.");
            }

            var successUrl = applicationConfig.FrontendUrl
                .AppendPathSegment("purchase/order/ote")
                .AppendPathSegment(result.Result.Result.Id);
                
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
                } : null,
                SuccessUrl = successUrl
            });
            if(!requestPayment.Succeeded || requestPayment.Result == null)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException(requestPayment.Message), requestPayment.Message);
            }

            return AppResult<OtePurchaseOrderResult>.CreateSucceeded(new OtePurchaseOrderResult {
                Action = requestPayment.Result.Action,
                Id = result.Result.Result.Id,
                Url = requestPayment.Result.Url
            }, "Successfully request purchase order details");
        }
        catch (Exception ex)
        {
            return AppResult<OtePurchaseOrderResult>.CreateFailed(ex, "An error occured when purchasing one time event");
        }
    }

    private string GenerateQRCode(string code)
    {
        string result = string.Empty;

        using (QRCodeGenerator generator = new QRCodeGenerator())
        using (QRCodeData data = generator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q))
        {
            var encoded = new PngByteQRCode(data);
            var pngData = encoded.GetGraphic(20);
            result = "data:image/png;base64," + Convert.ToBase64String(pngData);
        }
        
        return result;
    }

    private string CreateCode()
    {
        var date = DateTime.Now.ToString("MMddyyyyhhmmss");
        var guid = Guid.NewGuid().ToString();
        return date + guid;
    }

    private class Ticket 
    {
        public int Id {get; set;}
        public int OteDateId {get; set;}
        public decimal Price {get; set;}
        public string Name {get; set;}
        public string Code {get; set;}
        public string ImageData {get; set;}
    }
}