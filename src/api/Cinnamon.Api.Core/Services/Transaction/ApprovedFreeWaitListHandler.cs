using Cinnamon.Api.Core.Config;
using Cinnamon.Api.Core.Hubs;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.EmailDriver.Handlers;
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
using QRCoder;
using static Cinnamon.Framework.Enums.Enums;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class ApprovedFreeWaitListHandler : IApprovedFreeWaitListHandler
{
    private readonly IActivityData activityData;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly IOteTicketData oteTicketData;
    private readonly IOteFindByHandler oteFindByHandler;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly ITokenGeneratedData tokenGeneratedData;
    private readonly ApplicationConfig applicationConfig;
    private readonly IPurchaseOrderData purchaseOrderData;
    private readonly ICreateChatRoomHandler createChatRoomHandler;
    private readonly IHubContext<ChatHub> chathub;
    private readonly ICustomerData customerData;
    private readonly ISendInviteEventHandler sendInviteEventHandler;
    private readonly ICreateChatHistoryHandler createChatHistoryHandler;

    public ApprovedFreeWaitListHandler(IActivityData activityData, IGetProfileHandler getProfileHandler,
        IJsonSerializationProvider jsonSerializationProvider, IOteTicketData oteTicketData,
        IOteFindByHandler oteFindByHandler, IGetActivityHandler getActivityHandler,
        ITokenGeneratedData tokenGeneratedData, ApplicationConfig applicationConfig,
        IPurchaseOrderData purchaseOrderData, ICreateChatRoomHandler createChatRoomHandler,
        IHubContext<ChatHub> chathub, ICustomerData customerData,
        ISendInviteEventHandler sendInviteEventHandler, ICreateChatHistoryHandler createChatHistoryHandler)
    {
        this.activityData = activityData;
        this.getProfileHandler = getProfileHandler;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.oteTicketData = oteTicketData;
        this.oteFindByHandler = oteFindByHandler;
        this.getActivityHandler = getActivityHandler;
        this.tokenGeneratedData = tokenGeneratedData;
        this.applicationConfig = applicationConfig;
        this.purchaseOrderData = purchaseOrderData;
        this.createChatRoomHandler = createChatRoomHandler;
        this.chathub = chathub;
        this.customerData = customerData;
        this.sendInviteEventHandler = sendInviteEventHandler;
        this.createChatHistoryHandler = createChatHistoryHandler;
    }

    public AppResult<ApprovedFreeWaitListResult> Execute(ApprovedFreeWaitListArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<ApprovedFreeWaitListResult>> ExecuteAsync(ApprovedFreeWaitListArgs args)
    {
        try
        {
            var profileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!profileRes.Succeeded || profileRes.Result is null)
            {
                return AppResult<ApprovedFreeWaitListResult>.CreateFailed(new ApplicationException(profileRes.Message), profileRes.Message);
            }
            var profile = profileRes.Result;

            var waitListRes = await activityData.GetOteWaitList(args.WaitListId);
            if(!waitListRes.Succeeded || waitListRes.Result is null || !waitListRes.Result.IsSuccess)
            {
                return AppResult<ApprovedFreeWaitListResult>.CreateFailed(new ApplicationException("Unable to get waitlist."), "Unable to get waitlist.");
            }
            var waitlist = waitListRes.Result.Result;

            var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                ActivityId = waitlist.ActivityId,
                IncludeCustomer = true
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<ApprovedFreeWaitListResult>.CreateFailed(new ApplicationException(activityRes.Message), activityRes.Message);
            }
            if(activityRes.Result.Owner?.Id != profile.Id)
            {
                return AppResult<ApprovedFreeWaitListResult>.CreateFailed(
                    new ApplicationException("Invalid selected activity. Invalid request."), "Invalid selected activity. Invalid request.");
            }
            var activity = activityRes.Result;

            var oteActivityRes = await oteFindByHandler.ExecuteAsync(new ActivityService.Interactors.OteFindByHandlerArgs {
                Handler = activityRes.Result.Handler,
                IncludePricing = true,
                IncludeSchedule = true,
                IncludeAddress = true,
            });
            if(!oteActivityRes.Succeeded || oteActivityRes.Result is null)
            {
                return AppResult<ApprovedFreeWaitListResult>.CreateFailed(new ApplicationException(oteActivityRes.Message), oteActivityRes.Message);
            }
            var oteActivity = oteActivityRes.Result;

            var deserializedPayload = jsonSerializationProvider.Deserialize<PayloadData>(waitlist.Payload);

            if(deserializedPayload is null || deserializedPayload.Tickets is null || deserializedPayload.Tickets.Count() == 0)
            {
                return AppResult<ApprovedFreeWaitListResult>.CreateFailed(
                    new ApplicationException("Unable to locate selected tickets."), "Unable to locate selected tickets.");
            }

            var purchaseOrderRes = await purchaseOrderData.GetPurchaseOrderById(deserializedPayload.TransactionId);
            if(!purchaseOrderRes.Succeeded || purchaseOrderRes.Result is null || !purchaseOrderRes.Result.IsSuccess)
            {
                return AppResult<ApprovedFreeWaitListResult>.CreateFailed(
                    new ApplicationException("Unable to locate transaction."), "Unable to locate transaction.");
            }
            var purchaseOrder = purchaseOrderRes.Result.Result;

            var customerDetail = await customerData.GetCustomerById(purchaseOrder.CustomerId);
            if(!customerDetail.Succeeded || customerDetail.Result is null || !customerDetail.Result.IsSuccess)
            {
                return AppResult<ApprovedFreeWaitListResult>.CreateFailed(
                    new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }
            var customer = customerDetail.Result.Result;

            var purchaseOrderPayload = jsonSerializationProvider.Deserialize<PurchaseOrderPayload>(purchaseOrder.Payload);
            if(purchaseOrderPayload is null || string.IsNullOrEmpty(purchaseOrderPayload.Guid) || string.IsNullOrEmpty(purchaseOrderPayload.Token))
            {
                return AppResult<ApprovedFreeWaitListResult>.CreateFailed(
                    new ApplicationException("Invalid transaction payload."), "Invalid transaction payload.");
            }

            int oteScheduleId = oteActivity.OteDates.First().OteScheduleId;

            List<Framework.ApiCommand.ApiData.OteTicket.Request.CreateOteTicketArgs> ticketsToCreate = new();

            foreach(var item in deserializedPayload.Tickets)
            {
                // if ticket is old, set count to 1
                bool oldTicketPayload = item.Count == 0;
                if(oldTicketPayload)
                {
                    item.Count = 1;
                }

                for(int i = 0; i < item.Count; i++)
                {
                    var qrcode = CreateCode();
                    ticketsToCreate.Add(new Framework.ApiCommand.ApiData.OteTicket.Request.CreateOteTicketArgs {
                        ActivityId = waitlist.ActivityId,
                        Amount = 0,
                        CustomerId = waitlist.CustomerId,
                        OteDateId = item.OteDateId,
                        OteScheduleId = oteScheduleId,
                        OteSchedulePricingId = item.Id,
                        PurchaseOrderId = deserializedPayload.TransactionId,
                        QRCode = qrcode,
                        QRImageData = GenerateQRCode(qrcode),
                        Status = "UNVERIFIED",
                        SeatNumber = string.Empty,
                        Title = item.Name
                    });
                }
            }

            var createTicketRes = await oteTicketData.CreateTickets(new Framework.ApiCommand.ApiData.OteTicket.Request.CreateManyOteTicketsArgs {
                IncludeImageAsResult = false,
                Tickets = ticketsToCreate
            });
            if(!createTicketRes.Succeeded || createTicketRes.Result is null || !createTicketRes.Result.IsSuccess)
            {
                return AppResult<ApprovedFreeWaitListResult>.CreateFailed(
                    new ApplicationException("An error occured when creating tickets."), "An error occured when creating tickets.");
            }

            var tokenGeneratedPayload = new {
                PurchaseOrderId = deserializedPayload.TransactionId
            };
            var serializedTokenPayload = jsonSerializationProvider.Serialize(tokenGeneratedPayload);

            var createTokenRes = await tokenGeneratedData.CreateTokenGenerated(new Framework.ApiCommand.ApiData.TokenGenerated.Request.CreateTokenArgs {
                Guid = purchaseOrderPayload.Guid,
                Payload = serializedTokenPayload,
                Token = purchaseOrderPayload.Token,
                TokenType = "OTE-TICKET"
            });
            if(!createTokenRes.Succeeded || createTicketRes.Result is null || !createTicketRes.Result.IsSuccess)
            {
                return AppResult<ApprovedFreeWaitListResult>.CreateFailed(
                    new ApplicationException("An error occured when creating token."), "An error occured when creating token.");
            }

            // create link for ticket details
            var url = applicationConfig.FrontendUrl
                .AppendPathSegment("transactions")
                .AppendPathSegment("ote-tickets")
                .AppendPathSegment(purchaseOrderPayload.Guid)
                .AppendPathSegment(purchaseOrderPayload.Token);

            // update tickets sold
            var addTicketSoldRes = await activityData.AddTicketSolds(new Framework.ApiCommand.ApiData.Activity.Request.AddTicketSoldArgs {
                TicketSolds = deserializedPayload.Tickets.Select(t => new Framework.ApiCommand.ApiData.Activity.Request.AddTicketSoldArgs.AddTicketSold {
                    Id = t.Id,
                    TicketSold = t.Count
                })
            });
            if(!addTicketSoldRes.Succeeded || addTicketSoldRes.Result is null || !addTicketSoldRes.Result.IsSuccess)
            {
                return AppResult<ApprovedFreeWaitListResult>.CreateFailed(
                    new ApplicationException("An error occured when updating ticket sold."), "An error occured when updating ticket sold.");
            }

            var attendees = new List<Modules.EmailDriver.Interactors.SendInviteEventArgs.Attendee> 
            {
                new Modules.EmailDriver.Interactors.SendInviteEventArgs.Attendee {Email = customer.Email, Name = $"{customer.FirstName} {customer.LastName}"}
            };

            var firstTicket = deserializedPayload.Tickets.First();
            var oteDate = oteActivity.OteDates.FirstOrDefault(d => d.Id == firstTicket.OteDateId);
            if(oteDate is null)
            {
                return AppResult<ApprovedFreeWaitListResult>.CreateFailed(
                    new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }

            string Location = oteActivity.ExperienceTypeId == 2 ? "Online" : !string.IsNullOrEmpty(oteActivity.RegionName) ? $"{oteActivity.HouseNo} {oteActivity.BarangayName}, {oteActivity.CityName}, {oteActivity.RegionName}": oteActivity.PinnedLocation;

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
                return AppResult<ApprovedFreeWaitListResult>.CreateFailed(
                    new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
            }
            // add in group chat
            //prevent user from creating chat room with themselves
            if (purchaseOrder.CustomerId != activity.Owner?.Id)
            {
                var groupName = Guid.NewGuid().ToString();
                var createChatRes = await createChatRoomHandler.ExecuteAsync(new ChatService.Interactors.CreateChatRoomArgs
                {
                    ChatName = $"{activity.Owner?.FirstName} {activity.Owner?.LastName}'s Chat Group",
                    ChatType = Framework.Enums.Enums.ChatType.GroupChat,
                    FromUserId = purchaseOrder.CustomerId,
                    GroupName = groupName,
                    ToUserId = activity.Owner?.Id ?? 0
                });
                if (!createChatRes.Succeeded || createChatRes.Result is null)
                {
                    return AppResult<ApprovedFreeWaitListResult>.CreateFailed(
                        new ApplicationException("An error occured when creating chat group."), "An error occured when creating chat group.");
                }

                await chathub.Clients.All.SendAsync("AddToGroupAfterPayment", $"{createChatRes.Result.GroupName}|{createChatRes.Result.ChatRoomId}|{customer.Id}|{customer.FirstName}|{customer.LastName}|{customer.ProfileImg}|{activity.Owner?.FirstName} {activity.Owner?.LastName}'s Chat Group");

                var createChatHistoryRes = await createChatHistoryHandler.ExecuteAsync(new Services.ChatService.Interactors.CreateChatHistoryArgs
                {
                    ChatRoomId = createChatRes.Result.ChatRoomId,
                    FromConnectionId = string.Empty,
                    FromUserId = purchaseOrder.CustomerId,
                    ToConnectionId = string.Empty,
                    ToUserId = 0,
                    IsViewed = false,
                    Message = $"{customer.FirstName} {customer.LastName} has joined the group.",
                    ChatHistoryType = Framework.Enums.Enums.ChatHistoryType.Notification
                });

                if (!createChatHistoryRes.Succeeded || createChatHistoryRes.Result is null)
                {
                    return AppResult<ApprovedFreeWaitListResult>.CreateFailed(
                        new ApplicationException("An error occured. Please contact support"), "An error occured. Please contact support");
                }

                int chatRoomId = createChatRes.Result.ChatRoomId;

                string joinedPayload = $"{chatRoomId}|{DateTime.Now}|{purchaseOrder.CustomerId}|{customer.FirstName}|{customer.LastName}|{customer.ProfileImg}|{false}|{customer.FirstName} {customer.LastName} has joined the group.|{groupName}|{(int)ChatType.GroupChat}|{(int)ChatHistoryType.Notification}|{true}|{string.Empty}";
                await chathub.Clients.Group(createChatRes.Result.GroupName).SendAsync("ReceiveGroupMessage", joinedPayload);
            }
            return AppResult<ApprovedFreeWaitListResult>.CreateSucceeded(new ApprovedFreeWaitListResult {
                TicketLink = url
            }, "Successfully approved free ticket.");
        }
        catch (Exception ex)
        {
            return AppResult<ApprovedFreeWaitListResult>.CreateFailed(ex, "An error occured in ApprovedFreeWaitListHandler.");
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
        public string Name {get; set;}
        public int OteDateId {get; set;}
        public int Count {get; set;}
    }

    private class PayloadData 
    {
        public IEnumerable<Ticket> Tickets {get; set;}
        public int TransactionId {get; set;}
   }

   private class PurchaseOrderPayload 
   {
        public string Guid {get; set;}
        public string Token {get; set;}
   }
}