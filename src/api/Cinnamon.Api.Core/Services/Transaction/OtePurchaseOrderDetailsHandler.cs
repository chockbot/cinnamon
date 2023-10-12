using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class OtePurchaseOrderDetailsHandler : IOtePurchaseOrderDetailsHandler
{
    private readonly IPurchaseOrderData purchaseOrderData;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IOteFindByHandler oteFindByHandler;
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly IGetProfileHandler getProfileHandler;

    public OtePurchaseOrderDetailsHandler(IPurchaseOrderData purchaseOrderData, IGetActivityHandler getActivityHandler,
        IOteFindByHandler oteFindByHandler, IJsonSerializationProvider jsonSerializationProvider, IGetProfileHandler getProfileHandler)
    {
        this.purchaseOrderData = purchaseOrderData;
        this.getActivityHandler = getActivityHandler;
        this.oteFindByHandler = oteFindByHandler;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.getProfileHandler = getProfileHandler;
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
            if(!purchaseOrdeRes.Succeeded || purchaseOrdeRes.Result is null || !purchaseOrdeRes.Result.IsSuccess)
            {
                return AppResult<OtePurchaseOrderDetailsResult>.CreateFailed(new ApplicationException(purchaseOrdeRes.Message), purchaseOrdeRes.Message);
            }
            var purchaseOrder = purchaseOrdeRes.Result.Result;

            var deserializedPayload = jsonSerializationProvider.Deserialize<PayloadData>(purchaseOrder.Payload);
            if(deserializedPayload is null)
            {
                throw new Exception("An error occured. Please contact support.");
            }

            var customerRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!customerRes.Succeeded || customerRes.Result is null)
            {
                return AppResult<OtePurchaseOrderDetailsResult>.CreateFailed(new ApplicationException(customerRes.Message), customerRes.Message);
            }
            var customerProfile = customerRes.Result;

            if(purchaseOrder.CustomerId != customerProfile.Id)
            {
                return AppResult<OtePurchaseOrderDetailsResult>.CreateFailed(new ApplicationException("Invalid Request."), "Invalid Request.");
            }

            var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                ActivityId = purchaseOrder.ActivityId
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<OtePurchaseOrderDetailsResult>.CreateFailed(new ApplicationException(activityRes.Message), activityRes.Message);
            }
            var activity = activityRes.Result;

            var oteActivityRes = await oteFindByHandler.ExecuteAsync(new ActivityService.Interactors.OteFindByHandlerArgs {
                Handler = activity.Handler,
                IncludeAddress = true,
                IncludeDescription = true,
                IncludeImages = false,
                IncludePricing = true,
                IncludeSchedule = true
            });
            if(!oteActivityRes.Succeeded || oteActivityRes.Result is null)
            {
                return AppResult<OtePurchaseOrderDetailsResult>.CreateFailed(new ApplicationException(oteActivityRes.Message), oteActivityRes.Message);
            }
            var oteActivity = oteActivityRes.Result;

            var location = oteActivity.ExperienceTypeId == 2 ? "Online" : $"{oteActivity.HouseNo}, {oteActivity.BarangayName}, {oteActivity.CityName}, {oteActivity.RegionName}";
            var result = new OtePurchaseOrderDetailsResult {
                EventDate = oteActivity.ScheduleFrom,
                EventLocation = location,
                EventName = oteActivity.EventName,
                HandlingFee = deserializedPayload.Fees.PaymentProviderFee,
                PaymentMethod = deserializedPayload.PaymentChannel,
                ServiceFee = deserializedPayload.Fees.ServiceFee,
                TotalPurchase = purchaseOrder.OverallTotal,
                SubTotal = purchaseOrder.Total,
                Tickets = deserializedPayload.Tickets.Select(t => {
                    return new OtePurchaseOrderDetailsResult.Ticket {
                        Code = t.Code,
                        Id = t.Id,
                        ImageData = t.ImageData,
                        Name = t.Name,
                        Price = t.Price
                    };
                })
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
    }

    private class Ticket 
    {
        public int Id {get; set;}
        public decimal Price {get; set;}
        public string Name {get; set;}
        public string Code {get; set;}
        public string ImageData {get; set;}
    }

    class Fees {
        public decimal PaymentProviderFee {get; set;}
        public decimal ServiceFee {get; set;}
    }
}