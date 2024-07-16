using System.Text;
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
using Microsoft.AspNetCore.WebUtilities;
using QRCoder;

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

    public ApprovedFreeWaitListHandler(IActivityData activityData, IGetProfileHandler getProfileHandler,
        IJsonSerializationProvider jsonSerializationProvider, IOteTicketData oteTicketData,
        IOteFindByHandler oteFindByHandler, IGetActivityHandler getActivityHandler,
        ITokenGeneratedData tokenGeneratedData, ApplicationConfig applicationConfig)
    {
        this.activityData = activityData;
        this.getProfileHandler = getProfileHandler;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.oteTicketData = oteTicketData;
        this.oteFindByHandler = oteFindByHandler;
        this.getActivityHandler = getActivityHandler;
        this.tokenGeneratedData = tokenGeneratedData;
        this.applicationConfig = applicationConfig;
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

            var oteActivityRes = await oteFindByHandler.ExecuteAsync(new ActivityService.Interactors.OteFindByHandlerArgs {
                Handler = activityRes.Result.Handler,
                IncludePricing = true,
                IncludeSchedule = true
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

            foreach(var item in deserializedPayload.Tickets)
            {
                var qrcode = CreateCode();

                item.ImageData = GenerateQRCode(qrcode);
                item.QRCode = qrcode;
            }

            int oteScheduleId = oteActivity.OteDates.First().OteScheduleId;

            var createTicketRes = await oteTicketData.CreateTickets(new Framework.ApiCommand.ApiData.OteTicket.Request.CreateManyOteTicketsArgs {
                IncludeImageAsResult = false,
                Tickets = deserializedPayload.Tickets.Select(t => new Framework.ApiCommand.ApiData.OteTicket.Request.CreateOteTicketArgs {
                    ActivityId = waitlist.ActivityId,
                    Amount = 0,
                    CustomerId = waitlist.CustomerId,
                    OteDateId = t.OteDateId,
                    OteScheduleId = oteScheduleId,
                    OteSchedulePricingId = t.Id,
                    PurchaseOrderId = deserializedPayload.TransactionId,
                    QRCode = t.QRCode,
                    QRImageData = t.ImageData,
                    Status = "UNVERIFIED",
                    Title = t.Name
                })
            });
            if(!createTicketRes.Succeeded || createTicketRes.Result is null || !createTicketRes.Result.IsSuccess)
            {
                return AppResult<ApprovedFreeWaitListResult>.CreateFailed(
                    new ApplicationException("An error occured when creating tickets."), "An error occured when creating tickets.");
            }

            // generate token and guid
            var guid = Guid.NewGuid();
            var timestamp = DateTime.UtcNow;
            byte[] time = BitConverter.GetBytes(timestamp.ToBinary());
            byte[] key = guid.ToByteArray();
            var token = Convert.ToBase64String(time.Concat(key).ToArray());
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var tokenGeneratedPayload = new {
                PurchaseOrderId = deserializedPayload.TransactionId
            };
            var serializedTokenPayload = jsonSerializationProvider.Serialize(tokenGeneratedPayload);

            var createTokenRes = await tokenGeneratedData.CreateTokenGenerated(new Framework.ApiCommand.ApiData.TokenGenerated.Request.CreateTokenArgs {
                Guid = guid.ToString(),
                Payload = serializedTokenPayload,
                Token = encodedToken,
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
                .AppendPathSegment(guid.ToString())
                .AppendPathSegment(encodedToken);

            IDictionary<int, int> ticketSolds = new Dictionary<int, int>();

            foreach (var ticket in deserializedPayload.Tickets)
            {
                if(!ticketSolds.ContainsKey(ticket.Id))
                {
                    ticketSolds.Add(ticket.Id, 1);
                }
                else 
                {
                    ticketSolds[ticket.Id] += 1;
                }
            }

            // update tickets sold
            var addTicketSoldRes = await activityData.AddTicketSolds(new Framework.ApiCommand.ApiData.Activity.Request.AddTicketSoldArgs {
                TicketSolds = ticketSolds.Select(t => new Framework.ApiCommand.ApiData.Activity.Request.AddTicketSoldArgs.AddTicketSold {
                    Id = t.Key,
                    TicketSold = t.Value
                })
            });
            if(!addTicketSoldRes.Succeeded || addTicketSoldRes.Result is null || !addTicketSoldRes.Result.IsSuccess)
            {
                return AppResult<ApprovedFreeWaitListResult>.CreateFailed(
                    new ApplicationException("An error occured when updating ticket sold."), "An error occured when updating ticket sold.");
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

        // extra options
        public string ImageData {get; set;}
        public string QRCode {get; set;}
    }

    private class PayloadData 
    {
        public IEnumerable<Ticket> Tickets {get; set;}
        public int TransactionId {get; set;}
    }
}