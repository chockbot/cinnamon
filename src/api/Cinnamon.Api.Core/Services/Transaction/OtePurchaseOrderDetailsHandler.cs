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

namespace Cinnamon.Api.Core.Services.TransactionService;

public class OtePurchaseOrderDetailsHandler : IOtePurchaseOrderDetailsHandler
{
    private readonly IPurchaseOrderData purchaseOrderData;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IOteFindByHandler oteFindByHandler;
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly ApplicationConfig applicationConfig;
    private readonly IOteTicketData oteTicketData;
    private readonly IOteDateData oteDateData;

    public OtePurchaseOrderDetailsHandler(IPurchaseOrderData purchaseOrderData, IGetActivityHandler getActivityHandler,
        IOteFindByHandler oteFindByHandler, IJsonSerializationProvider jsonSerializationProvider, IGetProfileHandler getProfileHandler,
        ApplicationConfig applicationConfig, IOteTicketData oteTicketData, IOteDateData oteDateData)
    {
        this.purchaseOrderData = purchaseOrderData;
        this.getActivityHandler = getActivityHandler;
        this.oteFindByHandler = oteFindByHandler;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.getProfileHandler = getProfileHandler;
        this.applicationConfig = applicationConfig;
        this.oteTicketData = oteTicketData;
        this.oteDateData = oteDateData;
    }

    public AppResult<OtePurchaseOrderDetailsResult> Execute(OtePurchaseOrderDetailsArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<OtePurchaseOrderDetailsResult>.CreateFailed(ex, "An error occured in OtePurchaseOrderDetailsHandler");
        }
    }

    public async Task<AppResult<OtePurchaseOrderDetailsResult>> ExecuteAsync(OtePurchaseOrderDetailsArgs args)
    {
        try
        {
            var purchaseOrdeRes = await purchaseOrderData.GetPurchaseOrderById(args.PurchaseOrderId);
            if (!purchaseOrdeRes.Succeeded || purchaseOrdeRes.Result is null || !purchaseOrdeRes.Result.IsSuccess)
            {
                return AppResult<OtePurchaseOrderDetailsResult>.CreateFailed(new ApplicationException(purchaseOrdeRes.Message), purchaseOrdeRes.Message);
            }
            var purchaseOrder = purchaseOrdeRes.Result.Result;

            var deserializedPayload = jsonSerializationProvider.Deserialize<PayloadData>(purchaseOrder.Payload);
            if (deserializedPayload is null)
            {
                throw new Exception("An error occured. Please contact support.");
            }

            var customerRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs { });
            if (!customerRes.Succeeded || customerRes.Result is null)
            {
                return AppResult<OtePurchaseOrderDetailsResult>.CreateFailed(new ApplicationException(customerRes.Message), customerRes.Message);
            }
            var customerProfile = customerRes.Result;

            if (purchaseOrder.CustomerId != customerProfile.Id)
            {
                return AppResult<OtePurchaseOrderDetailsResult>.CreateFailed(new ApplicationException("Invalid Request."), "Invalid Request.");
            }

            var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs
            {
                ActivityId = purchaseOrder.ActivityId
            });
            if (!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<OtePurchaseOrderDetailsResult>.CreateFailed(new ApplicationException(activityRes.Message), activityRes.Message);
            }
            var activity = activityRes.Result;

            var oteActivityRes = await oteFindByHandler.ExecuteAsync(new ActivityService.Interactors.OteFindByHandlerArgs
            {
                Handler = activity.Handler,
                IncludeAddress = true,
                IncludeDescription = true,
                IncludeImages = false,
                IncludePricing = true,
                IncludeSchedule = true
            });
            if (!oteActivityRes.Succeeded || oteActivityRes.Result is null)
            {
                return AppResult<OtePurchaseOrderDetailsResult>.CreateFailed(new ApplicationException(oteActivityRes.Message), oteActivityRes.Message);
            }
            var oteActivity = oteActivityRes.Result;

            /* 
                get first ticket and ticket price to get ote date reference
            */
            var firstTicket = deserializedPayload.Tickets.FirstOrDefault();
            if (firstTicket is null)
            {
                throw new Exception("An error occured. Please contact support.");
            }
            var ticketPrice = oteActivity.Pricings.Where(p => p.Id == firstTicket.Id).FirstOrDefault();
            if (ticketPrice is null)
            {
                throw new Exception("An error occured. Please contact support.");
            }
            var oteDateRes = await oteDateData.GetOteDate(ticketPrice.OteDateId);
            if (!oteDateRes.Succeeded || oteDateRes.Result is null || !oteDateRes.Result.IsSuccess)
            {
                return AppResult<OtePurchaseOrderDetailsResult>.CreateFailed(new ApplicationException(oteDateRes.Message), oteDateRes.Message);
            }
            var oteDate = oteDateRes.Result.Result;

            var url = applicationConfig.FrontendUrl
                .AppendPathSegment("transactions")
                .AppendPathSegment("ote-tickets")
                .AppendPathSegment(deserializedPayload.Guid)
                .AppendPathSegment(deserializedPayload.Token);

            var location = oteActivity.ExperienceTypeId == 2 ? "Online" : string.IsNullOrEmpty(oteActivity.PinnedLocation) ? $"{oteActivity.HouseNo}, {oteActivity.BarangayName}, {oteActivity.CityName}, {oteActivity.RegionName}" : oteActivity.PinnedLocation;

            var ticketsRes = await oteTicketData.GetByPurchaseOrderId(purchaseOrder.Id, new());
            if (!ticketsRes.Succeeded || ticketsRes.Result is null || !ticketsRes.Result.IsSuccess)
            {
                return AppResult<OtePurchaseOrderDetailsResult>.CreateFailed(new ApplicationException(ticketsRes.Error?.Description), ticketsRes.Message);
            }
            var tickets = ticketsRes.Result.Result;

            //Get seatnumber from PO payload
            var Ticketspayload = deserializedPayload.Tickets.ToList();


            var result = new OtePurchaseOrderDetailsResult
            {
                EventDate     = oteDate.DateStart,
                EventLocation = location,
                EventName     = oteActivity.EventName,
                HandlingFee   = deserializedPayload.Fees.PaymentProviderFee,
                PaymentMethod = deserializedPayload.PaymentChannel,
                ServiceFee    = deserializedPayload.Fees.ServiceFee,
                TotalPurchase = purchaseOrder.OverallTotal,
                SubTotal      = purchaseOrder.Total,
                Discount      = purchaseOrder.CouponAmount,
                Tickets       = tickets.Select(t => {
                    var matchingTicketPayload = Ticketspayload.FirstOrDefault(tp => tp.Code == t.QRCode);
                    return new OtePurchaseOrderDetailsResult.Ticket
                    {
                        Code       = t.QRCode,
                        Id         = t.Id,
                        ImageData  = t.QRImageData,
                        Name       = t.Title,
                        Price      = t.Amount,
                        SeatNumber = matchingTicketPayload != null ? matchingTicketPayload.SeatNumber : string.Empty
                    };
                }),
                PurchasedDate = purchaseOrder.PurchaseDate,
                TicketUrl     = url
            };

            return AppResult<OtePurchaseOrderDetailsResult>.CreateSucceeded(result, "Ote purchase order details successfully get.");
        }
        catch (Exception ex)
        {
            return AppResult<OtePurchaseOrderDetailsResult>.CreateFailed(ex, "An error occured in OtePurchaseOrderDetailsHandler");
        }
    }

    class PayloadData 
    {
        public IEnumerable<Ticket> Tickets {get; set;}
        public Fees Fees {get; set;}
        public string PaymentMethod {get; set;}
        public string PaymentChannel {get; set;}
        public bool IsInclusivePayment {get; set;}
        public int OteScheduleId { get; set; }
        public string Guid {get; set;}
        public string Token {get; set;}
    }

    private class Ticket 
    {
        public int Id {get; set;}
        public decimal Price {get; set;}
        public string Name {get; set;}
        public string Code {get; set;}
        public string ImageData {get; set;}
        public int OteDateId {get; set;}
        public string SeatNumber { get; set; }
    }

    class Fees {
        public decimal PaymentProviderFee {get; set;}
        public decimal ServiceFee {get; set;}
    }
}