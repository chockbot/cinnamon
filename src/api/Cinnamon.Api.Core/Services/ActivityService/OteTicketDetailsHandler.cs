using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class OteTicketDetailsHandler : IOteTicketDetailsHandler
{
    private readonly ITokenGeneratedData tokenGeneratedData;
    private readonly IPurchaseOrderData purchaseOrderData;
    private readonly IOteTicketData oteTicketData;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IOteFindByHandler oteFindByHandler;
    private readonly IJsonSerializationProvider jsonSerializationProvider;

    public OteTicketDetailsHandler(ITokenGeneratedData tokenGeneratedData, IPurchaseOrderData purchaseOrderData,
        IOteTicketData oteTicketData, IGetActivityHandler getActivityHandler, IOteFindByHandler oteFindByHandler,
        IJsonSerializationProvider jsonSerializationProvider)
    {
        this.tokenGeneratedData = tokenGeneratedData;
        this.purchaseOrderData = purchaseOrderData;
        this.oteTicketData = oteTicketData;
        this.getActivityHandler = getActivityHandler;
        this.oteFindByHandler = oteFindByHandler;
        this.jsonSerializationProvider = jsonSerializationProvider;
    }
    
    public AppResult<OteTicketDetailsResult> Execute(OteTicketDetailsArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<OteTicketDetailsResult>.CreateFailed(ex, "An error occured in OteTicketDetailsHandler.");
        }
    }

    public async Task<AppResult<OteTicketDetailsResult>> ExecuteAsync(OteTicketDetailsArgs args)
    {
        try
        {
            var tokenRes = await tokenGeneratedData.GetTokenGenerated(args.Guid, args.Token);
            if(!tokenRes.Succeeded || tokenRes.Result is null || !tokenRes.Result.IsSuccess)
            {
                return AppResult<OteTicketDetailsResult>.CreateFailed(new ApplicationException(tokenRes.Error?.Description), tokenRes.Message);
            }
            var tokenGenerated = tokenRes.Result.Result;

            if(tokenGenerated.TokenType != "OTE-TICKET")
            {
                return AppResult<OteTicketDetailsResult>.CreateFailed(new ApplicationException("Invalid Request."), "Invalid Request.");
            }

            var deserializedPayload = jsonSerializationProvider.Deserialize<TokenGeneratedPayload>(tokenGenerated.Payload);
            if(deserializedPayload is null)
            {
                throw new Exception("An error external error occured.");
            }

            var purchaseOrderRes = await purchaseOrderData.GetPurchaseOrderById(deserializedPayload.PurchaseOrderId);
            if(!purchaseOrderRes.Succeeded || purchaseOrderRes.Result is null || !purchaseOrderRes.Result.IsSuccess)
            {
                return AppResult<OteTicketDetailsResult>.CreateFailed(new ApplicationException(purchaseOrderRes.Error?.Description), purchaseOrderRes.Message);
            }
            var purchaseOrder = purchaseOrderRes.Result.Result;

            var activityRes = await getActivityHandler.ExecuteAsync(new GetActivityArgs {
                ActivityId = purchaseOrder.ActivityId,
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<OteTicketDetailsResult>.CreateFailed(new ApplicationException(activityRes.Error?.Description), activityRes.Message);
            }
            var activity = activityRes.Result;

            var oteActivityRes = await oteFindByHandler.ExecuteAsync(new OteFindByHandlerArgs {
                Handler = activity.Handler,
                IncludeAddress = true,
                IncludeDescription = true,
                IncludeImages = true,
            });
            if(!oteActivityRes.Succeeded || oteActivityRes.Result is null)
            {
                return AppResult<OteTicketDetailsResult>.CreateFailed(new ApplicationException(oteActivityRes.Error?.Description), oteActivityRes.Message);
            }
            var oteActivity = oteActivityRes.Result;

            var ticketsRes = await oteTicketData.GetByPurchaseOrderId(purchaseOrder.Id, new Framework.ApiCommand.ApiData.OteTicket.Request.GetByPurchaseOrderIdArgs {
                IncludeImageAsResult = true,
                IncludeCustomer = true
            });
            if(!ticketsRes.Succeeded || ticketsRes.Result is null || !ticketsRes.Result.IsSuccess)
            {
                return AppResult<OteTicketDetailsResult>.CreateFailed(new ApplicationException(ticketsRes.Error?.Description), ticketsRes.Message);
            }
            var tickets = ticketsRes.Result.Result;

            var imageSrc = oteActivity.Images.OrderBy(i => i.Order).ThenBy(i => i.Id).First().ImageLocation;
            var location = oteActivity.ExperienceTypeId == 2 ? "Online" : $"{oteActivity.HouseNo}, {oteActivity.BarangayName}, {oteActivity.CityName}, {oteActivity.RegionName}";
            var result = new OteTicketDetailsResult {
                EventDate = oteActivity.ScheduleFrom,
                EventLocation = location,
                EventName = oteActivity.EventName,
                ImageSrc = imageSrc,
                Tickets = tickets.Select(t => {
                    return new OteTicketDetailsResult.Ticket {
                        Name = t.Title,
                        QRCodeData = t.QRImageData
                    };
                })
            };

            return AppResult<OteTicketDetailsResult>.CreateSucceeded(result, "Successfully get ticket details.");
        }
        catch (Exception ex)
        {
            return AppResult<OteTicketDetailsResult>.CreateFailed(ex, "An error occured in OteTicketDetailsHandler.");
        }
    }

    class TokenGeneratedPayload 
    {
        public int PurchaseOrderId {get; set;}
    }
}