using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Hubs;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ChatService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;
using Flurl;
using Microsoft.AspNetCore.SignalR;

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
    private readonly ICreateChatRoomHandler createChatRoomHandler;
    private readonly IHubContext<ChatHub> chathub;
    private readonly IGetOteRequestPaymentHandler getOteRequestPaymentHandler;
    private readonly IOteCreateRequestPaymentHandler createRequestPaymentHandler;

    public OteFinishTransactionHandler(IGetActivityHandler getActivityHandler, IOteFindByHandler oteFindByHandler,
        IJsonSerializationProvider jsonSerializationProvider, IPurchaseOrderData purchaseOrderData,
        ICustomerData customerData, IUpdateCreditBalanceHandler updateCreditBalanceHandler,
        IOteTicketData oteTicketData, IOteCustomerPayedNotificationHandler oteCustomerPayedNotificationHandler,
        ITokenGeneratedData tokenGeneratedData, ApplicationConfig applicationConfig, IActivityData activityData,
        IOteDateData oteDateData, ISendInviteEventHandler sendInviteEventHandler,
        ICreateChatRoomHandler createChatRoomHandler, IHubContext<ChatHub> chathub,
        IGetOteRequestPaymentHandler getOteRequestPaymentHandler, IOteCreateRequestPaymentHandler createRequestPaymentHandler)
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
        this.createChatRoomHandler = createChatRoomHandler;
        this.chathub = chathub;
        this.getOteRequestPaymentHandler = getOteRequestPaymentHandler;
        this.createRequestPaymentHandler = createRequestPaymentHandler;
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
                ActivityId = purchaseOrder.ActivityId,
                IncludeCustomer = true
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

            var ticketsToCreate = deserializedPayload.Tickets;

            var createTicketRes = await oteTicketData.CreateTickets(new Framework.ApiCommand.ApiData.OteTicket.Request.CreateManyOteTicketsArgs {
                IncludeImageAsResult = false,
                Tickets = ticketsToCreate.Select(t => {
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
                        OteDateId = t.OteDateId,
                    };
                })
            });
            if(!createTicketRes.Succeeded || createTicketRes.Result is null || !createTicketRes.Result.IsSuccess)
            {
                return AppResult<OteFinishTransactionResult>.CreateFailed(new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }

            // update if waitlisted
            if(deserializedPayload.Waitlisted && deserializedPayload.WaitListId > 0)
            {
                var getWaitlistRes = await activityData.GetOteWaitList(deserializedPayload.WaitListId);
                if(getWaitlistRes.Succeeded && getWaitlistRes.Result is not null && getWaitlistRes.Result.IsSuccess)
                {
                    var waitlist = getWaitlistRes.Result.Result;
                    
                    var updateWaitlistStatusRes = await activityData.UpdateOteWaitlist(new Framework.ApiCommand.ApiData.OteWaitlist.Request.UpdateOteWaitlistArgs {
                        ActivityId = waitlist.ActivityId,
                        CustomerId = waitlist.CustomerId,
                        CustomerName = waitlist.CustomerName,
                        Id = waitlist.Id,
                        Payload = waitlist.Payload,
                        ProviderId = waitlist.ProviderId,
                        ScheduleId = waitlist.ScheduleId,
                        Status = 4 // approved and purchased the waitlist
                    });
                }
            }

            // invalidate request payment token
            if(!string.IsNullOrEmpty(deserializedPayload.PaymentRequestGuid) && !string.IsNullOrEmpty(deserializedPayload.PaymentRequestToken))
            {
                var getOteRequestPaymentRes = await getOteRequestPaymentHandler.ExecuteAsync(new OteGetRequestPaymentArgs {
                    Guid = deserializedPayload.PaymentRequestGuid,
                    Token = deserializedPayload.PaymentRequestToken
                });
                if(getOteRequestPaymentRes.Succeeded && getOteRequestPaymentRes.Result is not null)
                {
                    var oteRequestPayment = getOteRequestPaymentRes.Result;
                    var createRequestPaymentRes = await createRequestPaymentHandler.ExecuteAsync(new OteCreateRequestPaymentArgs {
                        ActivityId = oteRequestPayment.ActivityId,
                        CustomerId = oteRequestPayment.CustomerId,
                        SelectedDate = oteRequestPayment.SelectedDate,
                        SelectedTickets = oteRequestPayment.SelectedTickets.Select(t => new OteCreateRequestPaymentArgs.RequestPaymentTicket {
                            TicketCount = t.TicketCount,
                            TicketId = t.TicketId
                        }),
                        ForceCreateTicket = oteRequestPayment.ForceCreateTicket,
                        Waitlisted = oteRequestPayment.Waitlisted,
                        WaitListId = oteRequestPayment.WaitListId,
                        Used = true,
                        Guid = oteRequestPayment.Guid,
                        Token = oteRequestPayment.Token,
                        Questions = oteRequestPayment.Questions?.Select(q => new OteCreateRequestPaymentArgs.ProviderQuestion {
                            Answer = q.Answer,
                            Id = q.Id,
                            Question = q.Question
                        })
                    });
                }
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
            foreach(var item in ticketsToCreate)
            {
                if(!tickets.ContainsKey(item.Id))
                {
                    tickets.Add(item.Id, new TicketSummary {
                        Name       = item.Name,
                        Price      = item.Price,
                        Id         = item.Id,
                        Count      = 1,
                        SeatNumber = item.SeatNumber
                    });
                }
                else
                {
                    tickets[item.Id].Count++;
                    tickets[item.Id].SeatNumber += $",{item.SeatNumber}";
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
                        TicketPrice = t.Value.Price,
                        TicketSeatNumber = t.Value.SeatNumber
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

            // add in group chat
            var groupName = Guid.NewGuid().ToString();
            var createChatRes = await createChatRoomHandler.ExecuteAsync(new ChatService.Interactors.CreateChatRoomArgs {
                ChatName = $"{activity.Owner?.FirstName} {activity.Owner?.LastName}'s Chat Group",
                ChatType = Framework.Enums.Enums.ChatType.GroupChat,
                FromUserId = purchaseOrder.CustomerId,
                GroupName = groupName,
                ToUserId = activity.Owner?.Id ?? 0
            });
            if(createChatRes.Succeeded && createChatRes.Result is not null)
            {
                await chathub.Clients.All.SendAsync("AddToGroupAfterPayment", $"{createChatRes.Result.GroupName}|{createChatRes.Result.ChatRoomId}|{customer.Id}|{customer.FirstName}|{customer.LastName}|{customer.ProfileImg}|{activity.Owner?.FirstName} {activity.Owner?.LastName}'s Chat Group");
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

        public bool Waitlisted {get; set;}
        public int WaitListId {get; set;}

        public string PaymentRequestToken {get; set;}
        public string PaymentRequestGuid {get; set;}
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

    private class TicketSummary 
    {
        public int Id {get; set;}
        public decimal Price {get; set;}
        public string Name {get; set;}
        public int Count {get; set;}
        public string SeatNumber { get; set; }
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