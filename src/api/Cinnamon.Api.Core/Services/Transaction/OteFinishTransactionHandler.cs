using System.Text;
using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;
using Microsoft.AspNetCore.WebUtilities;
using QRCoder;
using Flurl;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class OteFinishTransactionHandler : IOteFinishTransactionHandler
{
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IOteFindByHandler oteFindByHandler;
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly IPurchaseOrderData purchaseOrderData;
    private readonly ICustomerData customerData;
    private readonly IUpdateCreditBalanceHandler updateCreditBalanceHandler;
    private readonly IOteTicketData oteTicketData;
    private readonly IOteCustomerPayedNotificationHandler oteCustomerPayedNotificationHandler;
    private readonly ITokenGeneratedData tokenGeneratedData;
    private readonly ApplicationConfig applicationConfig;

    public OteFinishTransactionHandler(IGetActivityHandler getActivityHandler, IOteFindByHandler oteFindByHandler,
        IJsonSerializationProvider jsonSerializationProvider, IPurchaseOrderData purchaseOrderData,
        ICustomerData customerData, IUpdateCreditBalanceHandler updateCreditBalanceHandler,
        IOteTicketData oteTicketData, IOteCustomerPayedNotificationHandler oteCustomerPayedNotificationHandler,
        ITokenGeneratedData tokenGeneratedData, ApplicationConfig applicationConfig)
    {
        this.getActivityHandler = getActivityHandler;
        this.oteFindByHandler = oteFindByHandler;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.purchaseOrderData = purchaseOrderData;
        this.customerData = customerData;
        this.updateCreditBalanceHandler = updateCreditBalanceHandler;
        this.oteTicketData = oteTicketData;
        this.oteCustomerPayedNotificationHandler = oteCustomerPayedNotificationHandler;
        this.tokenGeneratedData = tokenGeneratedData;
        this.applicationConfig = applicationConfig;
    }
    
    public AppResult<OteFinishTransactionResult> Execute(OteFinishTransactionArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<OteFinishTransactionResult>.CreateFailed(ex, "An error occured in OteFinishTransactionHandler");
        }
    }

    public async Task<AppResult<OteFinishTransactionResult>> ExecuteAsync(OteFinishTransactionArgs args)
    {
        try
        {
            var getPurchaseOrder = await purchaseOrderData.GetPurchaseOrderById(args.TransactionId);
            if(!getPurchaseOrder.Succeeded || getPurchaseOrder.Result is null || !getPurchaseOrder.Result.IsSuccess)
            {
                return AppResult<OteFinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }
            var purchaseOrder = getPurchaseOrder.Result.Result;

            var customerDetail = await customerData.GetCustomerById(purchaseOrder.CustomerId);
            if(!customerDetail.Succeeded || customerDetail.Result is null || !customerDetail.Result.IsSuccess)
            {
                return AppResult<OteFinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }
            var customer = customerDetail.Result.Result;

            var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                ActivityId = purchaseOrder.ActivityId
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<OteFinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }
            var activity = activityRes.Result;

            var oteActivityRes = await oteFindByHandler.ExecuteAsync(new ActivityService.Interactors.OteFindByHandlerArgs {
                Handler = activity.Handler,
                IncludeAddress = true,
                IncludeDescription = true,
                IncludeImages = true,
                IncludePricing = true,
                IncludeSchedule = true,
            });
            if(!oteActivityRes.Succeeded || oteActivityRes.Result is null)
            {
                return AppResult<OteFinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }
            var oteActivity = oteActivityRes.Result;

            var providerRes = await customerData.GetCustomerById(oteActivity.ProviderId);
            if(!providerRes.Succeeded || providerRes.Result is null || !providerRes.Result.IsSuccess)
            {
                return AppResult<OteFinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }
            var provider = providerRes.Result.Result;

            var deserializedPayload = jsonSerializationProvider.Deserialize<PayloadData>(purchaseOrder.Payload);
            if(deserializedPayload == null)
            {
                return AppResult<OteFinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }

            foreach(var item in deserializedPayload.Tickets)
            {
                item.Code = CreateCode();
                item.ImageData = GenerateQRCode(item.Code);
            }

            var createTicketRes = await oteTicketData.CreateTickets(new Framework.ApiCommand.ApiData.OteTicket.Request.CreateManyOteTicketsArgs {
                IncludeImageAsResult = false,
                Tickets = deserializedPayload.Tickets.Select(t => {
                    return new Framework.ApiCommand.ApiData.OteTicket.Request.CreateOteTicketArgs {
                        ActivityId = oteActivity.Id,
                        Amount = t.Price,
                        CustomerId = purchaseOrder.CustomerId,
                        OteScheduleId = deserializedPayload.OteScheduleId,
                        OteSchedulePricingId = t.Id,
                        PurchaseOrderId = purchaseOrder.Id,
                        QRCode = t.Code,
                        QRImageData = t.ImageData,
                        Status = "UNVERIFIED",
                        Title = t.Name
                    };
                })
            });
            if(!createTicketRes.Succeeded || createTicketRes.Result is null || !createTicketRes.Result.IsSuccess)
            {
                return AppResult<OteFinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }

            // generate token and guid
            var guid = Guid.NewGuid();
            var timestamp = DateTime.UtcNow;
            byte[] time = BitConverter.GetBytes(timestamp.ToBinary());
            byte[] key = guid.ToByteArray();
            var token = Convert.ToBase64String(time.Concat(key).ToArray());
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var tokenGeneratedPayload = new TokenGeneratedPayload {
                PurchaseOrderId = purchaseOrder.Id
            };
            var tokenSerializedPayload = jsonSerializationProvider.Serialize(tokenGeneratedPayload);
            var createTokenRes = await tokenGeneratedData.CreateTokenGenerated(new Framework.ApiCommand.ApiData.TokenGenerated.Request.CreateTokenArgs {
                Guid = guid.ToString(),
                Payload = tokenSerializedPayload,
                Token = token,
                TokenType = "OTE-TICKET"
            });
            if(!createTokenRes.Succeeded || createTokenRes.Result is null || !createTokenRes.Result.IsSuccess)
            {
                return AppResult<OteFinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }

            // create link for ticket details
            var url = applicationConfig.FrontendUrl
                .AppendPathSegment("transactions")
                .AppendPathSegment("ote-tickets")
                .AppendPathSegment(guid.ToString())
                .AppendPathSegment(encodedToken);

            // if there is credit applied in purchase order then subract in balance credit
            if(purchaseOrder.CreditAmount > 0)
            {
                var updateCredit = await updateCreditBalanceHandler.ExecuteAsync(new AccountService.Interactors.UpdateCreditBalanceArgs {
                    ActionFlag = 1,
                    Amount = purchaseOrder.CreditAmount,
                    CustomerId = purchaseOrder.CustomerId
                });
                if(!updateCredit.Succeeded || updateCredit.Result == null)
                {
                    return AppResult<OteFinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
                }
            }

            var referenceId = "000000000000000".Substring(purchaseOrder.Id.ToString().Length) + purchaseOrder.Id;
            var tickets = new Dictionary<int, Ticket>();
            foreach(var item in deserializedPayload.Tickets)
            {
                if(!tickets.ContainsKey(item.Id))
                {
                    tickets.Add(item.Id, new Ticket {
                        Name = item.Name,
                        Price = item.Price,
                        Count = 1
                    });
                }
                else 
                {
                    tickets[item.Id].Count++;
                }
            }

            var notifyEmailRes = await oteCustomerPayedNotificationHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.OteCustomerPayedNotificationArgs {
                Email = customer.Email,
                CustomerName = customer.FirstName,
                EventDate = oteActivity.ScheduleFrom,
                EventLocation = $"{oteActivity.HouseNo} {oteActivity.BarangayName}, {oteActivity.CityName}, {oteActivity.RegionName}",
                EventName = oteActivity.EventName,
                HandlingFee = deserializedPayload.Fees.ServiceFee,
                PaymentMethod = deserializedPayload.PaymentMethod,
                ProviderEmail = provider.Email,
                ProviderName = $"{provider.FirstName} {provider.LastName}",
                ProviderNumber = provider.PhoneNumber,
                ReferenceNumber = referenceId,
                ServiceFee = deserializedPayload.Fees.PaymentProviderFee,
                SubTotal = purchaseOrder.Total,
                Tickets = tickets.Select(t => {
                    return new Modules.NotificationDriver.Interactors.OteCustomerPayedNotificationArgs.TicketDetails {
                        TicketCount = t.Value.Count,
                        TicketName = t.Value.Name,
                        TicketPrice = t.Value.Price
                    };
                }),
                TotalAmount = purchaseOrder.OverallTotal,
                TicketDetailsLink = url
            });
            if(!notifyEmailRes.Succeeded || notifyEmailRes.Result is null)
            {
                return AppResult<OteFinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }

            return AppResult<OteFinishTransactionResult>.CreateSucceeded(new OteFinishTransactionResult {}, "Successfully finish transaction");
        }
        catch (Exception ex)
        {
            return AppResult<OteFinishTransactionResult>.CreateFailed(ex, "An error occured in OteFinishTransactionHandler");
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
        public string ImageData {get; set;}
        public string Code {get; set;}

        // extra field
        public int Count {get; set;}
    }

    class Fees {
        public decimal PaymentProviderFee {get; set;}
        public decimal ServiceFee {get; set;}
    }

    class TokenGeneratedPayload 
    {
        public int PurchaseOrderId {get; set;}
    }
}