using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;
using Flurl;
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
    private readonly IOteFinishTransactionHandler oteFinishTransactionHandler;
    private readonly ILogger<OtePurchaseOrderHandler> logger;
    private readonly ApplicationConfig applicationConfig;
    private readonly ITokenGeneratedData tokenGeneratedData;
    private readonly IActivityQuestionsHandler activityQuestionsHandler;
    private readonly ICreateOteWaitlistHandler createOteWaitlist;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly ITokenGeneratorProvider tokenGeneratorProvider;
    private readonly IGetOteRequestPaymentHandler getOteRequestPaymentHandler;

    public OtePurchaseOrderHandler(IPurchaseOrderData purchaseOrderData, ICustomerData customerData,
        IHttpContextAccessor httpContext, IRequestPaymentHandler requestPaymentHandler, IJsonSerializationProvider jsonSerializationProvider,
        IValidateCouponCodeHandler validateCouponCodeHandler, ICustomerPricingData customerPricingData, ILogger<OtePurchaseOrderHandler> logger,
        IOteFindByHandler oteFindByHandler, IGetActivityHandler getActivityHandler, IOwnerPricingInclusiveHandler ownerPricingInclusiveHandler,
        ApplicationConfig applicationConfig, IOteFinishTransactionHandler oteFinishTransactionHandler,
        ITokenGeneratedData tokenGeneratedData, IActivityQuestionsHandler activityQuestionsHandler,
        ICreateOteWaitlistHandler createOteWaitlist, IGetProfileHandler getProfileHandler,
        ITokenGeneratorProvider tokenGeneratorProvider, IGetOteRequestPaymentHandler getOteRequestPaymentHandler)
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
        this.oteFinishTransactionHandler = oteFinishTransactionHandler;
        this.tokenGeneratedData = tokenGeneratedData;
        this.activityQuestionsHandler = activityQuestionsHandler;
        this.createOteWaitlist = createOteWaitlist;
        this.getProfileHandler = getProfileHandler;
        this.tokenGeneratorProvider = tokenGeneratorProvider;
        this.getOteRequestPaymentHandler = getOteRequestPaymentHandler;
    }
    
    public AppResult<OtePurchaseOrderResult> Execute(OtePurchaseOrderArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OtePurchaseOrderResult>> ExecuteAsync(OtePurchaseOrderArgs args)
    {
        try
        {
            // get customer id saved in claims
            var currentUserRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!currentUserRes.Succeeded || currentUserRes.Result is null)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(
                    new ApplicationException("Unable to determine current account login"), "Unable to determine current account login");
            }
            var currentUser = currentUserRes.Result;
            int id = Convert.ToInt32(currentUser.Id);

            bool isEmptyTicket = args.Tickets.Count() == 0;
            if(isEmptyTicket)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException("Select ticket to purchase. Invalid request."), "Select ticket to purchase. Invalid request.");
            }

            var checkActivityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                ActivityId = args.ActivityId,
                IncludeCustomer = true
            });
            if(!checkActivityRes.Succeeded || checkActivityRes.Result is null)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException("Unable to identify selected one time event."), "Unable to identify selected one time event.");
            }
            var provider = checkActivityRes.Result.Owner;

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

            var activityQuestions = await activityQuestionsHandler.ExecuteAsync(new ActivityService.Interactors.ActivityQuestionsArgs {
                ActivityId = oteActivity.Id
            });
            if(!activityQuestions.Succeeded || activityQuestions.Result is null)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(
                    new ApplicationException("Unable to get activity questions."), "Unable to get activity questions.");
            }
            var questions = activityQuestions.Result.Questions;

            // filter tickets available only to ticket date
            var defaultDatePricing = oteActivity.Pricings.FirstOrDefault(p => p.Id == args.Tickets.First().Id);
            if(defaultDatePricing is null)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException("Unable to identify selected one time event."), "Unable to identify selected one time event.");
            }
            var fileteredAvailableTickets = oteActivity.Pricings.Where(p => p.OteDateId == defaultDatePricing.OteDateId);

            var selectedTicketIds = args.Tickets.Select(t => t.Id);
            var validSelectedTickets = fileteredAvailableTickets.Where(t => selectedTicketIds.Contains(t.Id));

            // should not able to purchase with the mix of free and paid tickets
            var invalidSelectedTickets = validSelectedTickets.Count() == 0 && validSelectedTickets.Any(t => t.Price == 0) && validSelectedTickets.Any(t => t.Price != 0);
            if(invalidSelectedTickets)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException("Combining free and paid tickets is not permitted."), "Combining free and paid tickets is not permitted.");
            }

            var oteDate = oteActivity.OteDates.FirstOrDefault(d => d.Id == defaultDatePricing.OteDateId);
            if(oteDate is null)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(
                    new ApplicationException("Unable to get ote date. Please contact support."), "Unable to get ote date. Please contact support.");
            }

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

            var requestedPaymentRes = await getOteRequestPaymentHandler.ExecuteAsync(new OteGetRequestPaymentArgs {
                Guid = args.Guid,
                Token = args.Token
            });
            if(!requestedPaymentRes.Succeeded || requestedPaymentRes.Result is null)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException(requestedPaymentRes.Message), requestedPaymentRes.Message);
            }
            var requestedPayment = requestedPaymentRes.Result;

            if(requestedPayment.Used)
            {
                return AppResult<OtePurchaseOrderResult>.CreateFailed(
                    new ApplicationException("Request payment already used."), "Request payment already used.");
            }

            var selectedTickets = new List<Ticket>();
            // validate selected tickets
            foreach(var ticket in args.Tickets)
            {
                var ticketPrice = fileteredAvailableTickets.FirstOrDefault(t => t.Id == ticket.Id);
                if(ticketPrice is null)
                {
                    return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException("Unable to identify selected ticket."), "Unable to identify selected ticket.");
                }

                // check ticket availablity if not force to create
                if(!requestedPayment.ForceCreateTicket)
                {
                    if(ticketPrice.TicketSold >= ticketPrice.MaxSlots && !ticketPrice.IsUnlimited)
                    {
                        return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException("Tickets already sold out."), "Tickets already sold out.");
                    }

                    if((ticketPrice.MaxSlots - ticketPrice.TicketSold) < ticket.Count && !ticketPrice.IsUnlimited)
                    {
                        return AppResult<OtePurchaseOrderResult>.CreateFailed(
                            new ApplicationException("Some of the tickets already sold. Refresh the page and update your tickets."), "Some of the tickets already sold. Refresh the page and update your tickets.");
                    }
                }

                // create selected ticket instance
                var seatNumberList = ticket.SeatNumber.Split(",").ToList();
                for(int i = 0; i < ticket.Count; i++)
                {
                    var qrcode = CreateCode();
                    selectedTickets.Add(new Ticket {
                        Id               = ticketPrice.Id,
                        Name             = ticketPrice.Name,
                        Price            = ticketPrice.Price,
                        Code             = qrcode,
                        ImageData        = GenerateQRCode(qrcode),
                        OteDateId        = ticketPrice.OteDateId,
                        RequiredApproval = ticketPrice.RequiredApproval,
                        SeatNumber       = seatNumberList[i]
                    });
                }
            }

            decimal subTotal = selectedTickets.Sum(t => t.Price);
            decimal providerFeePercent = args.PaymentMethod == "CARD" ? 5 : 3;
            decimal paymentProviderFee = isInclusivePayment ? 0 : subTotal * (providerFeePercent / 100); //.05m;
            decimal discount = 0;
            decimal serviceFee = subTotal <= 0 ? 0 : (isInclusivePayment ? 0 : 15); //50;
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
            var purchaseToken = tokenGeneratorProvider.Generator();
            
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
                Guid = purchaseToken.Guid,
                Token = purchaseToken.Token,
                Waitlisted = requestedPayment.Waitlisted,
                WaitListId = requestedPayment.WaitListId,
                PaymentRequestToken = requestedPayment.Token, // to be used for ote finish transaction
                PaymentRequestGuid = requestedPayment.Guid,   // to invalidate requested payment token
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

            string ticketQueryString = string.Empty;
            foreach(var ticket in args.Tickets)
            {

                ticketQueryString += $"{ticket.Id}-{ticket.Count},";
            }

            var successUrl = applicationConfig.FrontendUrl
                .AppendPathSegment(oteActivity.Schedule.ReserveSeat
                                   ? $"purchase/order/ote/reserved/{oteActivity.Handler}"
                                   : "purchase/order/ote")
                .AppendPathSegment(result.Result.Result.Id);

            var failedUrl = applicationConfig.FrontendUrl
                .AppendPathSegment($"payment/ote/{oteActivity.Handler}")
                .SetQueryParam("Ticket", ticketQueryString)
                .SetQueryParam("Status","failed");

            var noNeedPaymentGateway = overallTotal == 0;
            if(noNeedPaymentGateway)
            {
                var freeTicketsWithoutApproval = validSelectedTickets.Where(t => t.Price == 0 && !t.RequiredApproval);
                if(freeTicketsWithoutApproval.Any())
                {
                    var finishTransaction = await oteFinishTransactionHandler.ExecuteAsync(new OteFinishTransactionArgs {
                        TransactionId = result.Result.Result.Id
                    });
                    if(!finishTransaction.Succeeded || finishTransaction.Result is null)
                    {
                        return AppResult<OtePurchaseOrderResult>.CreateFailed(new ApplicationException(finishTransaction.Message), finishTransaction.Message);
                    }
                }

                var freeTicketsWithApproval = selectedTickets
                                                .Where(t => t.Price == 0 && t.RequiredApproval)
                                                .GroupBy(t => t.Id);
                if(freeTicketsWithApproval.Any())
                {
                    var waitlistPayload = new {
                        Tickets = freeTicketsWithApproval.Select(t => {
                            var first = t.First();
                            var ticket = new {
                                Id = first.Id,
                                Name = first.Name,
                                Price = first.Price,
                                OteDateId = first.OteDateId,
                                Date = oteDate.DateStart,
                                Count = t.Count(),
                                SeatName = first.SeatNumber
                            };
                            return ticket;
                        }),
                        Questions = args.Questions?.Select(q => new {
                            Question = q.Question,
                            Answer = q.Answer
                        }),
                        TransactionId = result.Result.Result.Id
                    };
                    var waitlistSerializedPayload = jsonSerializationProvider.Serialize(waitlistPayload);

                    var createWaitlistRes = await createOteWaitlist.ExecuteAsync(new ActivityService.Interactors.CreateOteWaitlistArgs {
                        ActivityId   = oteActivity.Id,
                        CustomerId   = id,
                        CustomerName = $"{currentUser.FirstName} {currentUser.LastName}",
                        Payload      = waitlistSerializedPayload,
                        ProviderId   = provider?.Id ?? 0,
                        Status       = 1,
                        Type         = "Free",
                        OteDateId    = oteDate.Id,
                    });
                    if(!createWaitlistRes.Succeeded || createWaitlistRes.Result is null)
                    {
                        return AppResult<OtePurchaseOrderResult>.CreateFailed(
                            new ApplicationException("Unable to create waitlist"), "Unable to create waitlist");
                    }
                }

                return AppResult<OtePurchaseOrderResult>.CreateSucceeded(new OtePurchaseOrderResult {
                    Action = 1,
                    Id = result.Result.Result.Id,
                    Url = successUrl.ToString()
                }, "Successfully request purchase order details.");
            }

            // generate token and guid for transaction redirection details
            var tokenGenerated = tokenGeneratorProvider.Generator();
            var payload = new 
            {
                ActivityId = result.Result.Result.ActivityId,
                TransactionId = result.Result.Result.Id,
                OteQuery = ticketQueryString,
                Url = successUrl.ToString()
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
                return AppResult<OtePurchaseOrderResult>.CreateFailed(
                    new ApplicationException(createTokenRes.Result?.ErrorInfo?.Message), "An error occured in when purchasing an event.");
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
        public bool RequiredApproval {get; set;}
        public string SeatNumber { get; set; }
    }
}