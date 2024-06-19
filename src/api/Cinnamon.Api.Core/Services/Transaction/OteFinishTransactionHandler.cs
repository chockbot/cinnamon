using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;
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
    private readonly IActivityData activityData;
    private readonly IOteDateData oteDateData;
    private readonly ISendInviteEventHandler sendInviteEventHandler;

    public OteFinishTransactionHandler(IGetActivityHandler getActivityHandler, IOteFindByHandler oteFindByHandler,
        IJsonSerializationProvider jsonSerializationProvider, IPurchaseOrderData purchaseOrderData,
        ICustomerData customerData, IUpdateCreditBalanceHandler updateCreditBalanceHandler,
        IOteTicketData oteTicketData, IOteCustomerPayedNotificationHandler oteCustomerPayedNotificationHandler,
        ITokenGeneratedData tokenGeneratedData, ApplicationConfig applicationConfig, IActivityData activityData,
        IOteDateData oteDateData, ISendInviteEventHandler sendInviteEventHandler)
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
        this.activityData = activityData;
        this.oteDateData = oteDateData;
        this.sendInviteEventHandler = sendInviteEventHandler;
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

            // get first ticket for ote date reference
            var firstTicket = deserializedPayload.Tickets.FirstOrDefault();
            if(firstTicket is null)
            {
                return AppResult<OteFinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }
            var oteDateRes = await oteDateData.GetOteDate(firstTicket.OteDateId);
            if(!oteDateRes.Succeeded || oteDateRes.Result is null || !oteDateRes.Result.IsSuccess)
            {
                return AppResult<OteFinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }
            var oteDate = oteDateRes.Result.Result;

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
                        Title = t.Name,
                        OteDateId = t.OteDateId
                    };
                })
            });
            if(!createTicketRes.Succeeded || createTicketRes.Result is null || !createTicketRes.Result.IsSuccess)
            {
                return AppResult<OteFinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }

            var tokenGeneratedPayload = new TokenGeneratedPayload {
                PurchaseOrderId = purchaseOrder.Id
            };
            var tokenSerializedPayload = jsonSerializationProvider.Serialize(tokenGeneratedPayload);
            var createTokenRes = await tokenGeneratedData.CreateTokenGenerated(new Framework.ApiCommand.ApiData.TokenGenerated.Request.CreateTokenArgs {
                Guid = deserializedPayload.Guid,
                Payload = tokenSerializedPayload,
                Token = deserializedPayload.Token,
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
                .AppendPathSegment(deserializedPayload.Guid)
                .AppendPathSegment(deserializedPayload.Token);

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
            var tickets = new Dictionary<int, TicketSummary>();
            foreach(var item in deserializedPayload.Tickets)
            {
                if(!tickets.ContainsKey(item.Id))
                {
                    tickets.Add(item.Id, new TicketSummary {
                        Name = item.Name,
                        Price = item.Price,
                        Id = item.Id,
                        Count = 1
                    });
                }
                else
                {
                    tickets[item.Id].Count++;
                }
            }

            // update tickets sold
            var addTicketSoldRes = await activityData.AddTicketSolds(new Framework.ApiCommand.ApiData.Activity.Request.AddTicketSoldArgs {
                TicketSolds = tickets.Select(t => {
                    return new Framework.ApiCommand.ApiData.Activity.Request.AddTicketSoldArgs.AddTicketSold {
                        Id = t.Value.Id,
                        TicketSold = t.Value.Count
                    };
                })
            });
            if(!addTicketSoldRes.Succeeded || addTicketSoldRes.Result is null || !addTicketSoldRes.Result.IsSuccess)
            {
                return AppResult<OteFinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }
            string Location = oteActivity.ExperienceTypeId == 2 ? "Online" : !string.IsNullOrEmpty(oteActivity.RegionName) ? $"{oteActivity.HouseNo} {oteActivity.BarangayName}, {oteActivity.CityName}, {oteActivity.RegionName}": oteActivity.PinnedLocation;
            var notifyEmailRes = await oteCustomerPayedNotificationHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.OteCustomerPayedNotificationArgs {
                Email = customer.Email,
                CustomerName = customer.FirstName,
                EventDate = oteDate.DateStart,
                EventLocation = Location,
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
                TicketDetailsLink = url,
                Discount = purchaseOrder.CouponAmount
            });
            if(!notifyEmailRes.Succeeded || notifyEmailRes.Result is null)
            {
                return AppResult<OteFinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }

            var attendees = new List<Modules.EmailDriver.Interactors.SendInviteEventArgs.Attendee> 
            {
                new Modules.EmailDriver.Interactors.SendInviteEventArgs.Attendee {Email = customer.Email, Name = $"{customer.FirstName} {customer.LastName}"}
            };

            var sendInviteEventRes = await sendInviteEventHandler.ExecuteAsync(new Modules.EmailDriver.Interactors.SendInviteEventArgs
            {
                Attendees = attendees,
                Content = "Cinnamon Experience Event",
                DateEnd = oteDate.DateEnd,
                DateStart = oteDate.DateStart,
                Location = Location,
                Subject = oteActivity.EventName
            });
            if(!sendInviteEventRes.Succeeded || sendInviteEventRes.Result is null)
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
        public int OteDateId {get; set;}
        public decimal Price {get; set;}
        public string Name {get; set;}
        public string Code {get; set;}
        public string ImageData {get; set;}
    }

    private class TicketSummary 
    {
        public int Id {get; set;}
        public decimal Price {get; set;}
        public string Name {get; set;}
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