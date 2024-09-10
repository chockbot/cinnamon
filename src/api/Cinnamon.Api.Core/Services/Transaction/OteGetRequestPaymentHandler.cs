using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class OteGetRequestPaymentHandler : IGetOteRequestPaymentHandler
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly ITokenGeneratedData tokenGeneratedData;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IOteFindByHandler oteFindByHandler;

    public OteGetRequestPaymentHandler(IGetProfileHandler getProfileHandler, IJsonSerializationProvider jsonSerializationProvider,
        ITokenGeneratedData tokenGeneratedData, IGetActivityHandler getActivityHandler, 
        IOteFindByHandler oteFindByHandler)
    {
        this.getProfileHandler = getProfileHandler;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.tokenGeneratedData = tokenGeneratedData;
        this.getActivityHandler = getActivityHandler;
        this.oteFindByHandler = oteFindByHandler;
    }

    public AppResult<OteGetRequestPaymentResult> Execute(OteGetRequestPaymentArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OteGetRequestPaymentResult>> ExecuteAsync(OteGetRequestPaymentArgs args)
    {
        try
        {
            var getProfileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!getProfileRes.Succeeded || getProfileRes.Result is null)
            {
                return AppResult<OteGetRequestPaymentResult>.CreateFailed(new ApplicationException(getProfileRes.Message), getProfileRes.Message);
            }
            var profile = getProfileRes.Result;

            var tokenRes = await tokenGeneratedData.GetTokenGenerated(args.Guid, args.Token);
            if(!tokenRes.Succeeded || tokenRes.Result is null || !tokenRes.Result.IsSuccess)
            {
                return AppResult<OteGetRequestPaymentResult>.CreateFailed(
                    new ApplicationException("Invalid guid and token. Invalid request."), "Invalid guid and token. Invalid request.");
            }
            var tokenData = tokenRes.Result.Result;

            const string TOKEN_TYPE = "REQUEST-PAYMENT";
            
            if(!tokenData.TokenType.Equals(TOKEN_TYPE, StringComparison.OrdinalIgnoreCase))
            {
                return AppResult<OteGetRequestPaymentResult>.CreateFailed(
                    new ApplicationException("Invalid guid and token. Invalid request."), "Invalid guid and token. Invalid request.");
            }

            var deserializedPayload = jsonSerializationProvider.Deserialize<PayloadData>(tokenData.Payload);

            if(deserializedPayload is null || deserializedPayload.CustomerId != profile.Id)
            {
                return AppResult<OteGetRequestPaymentResult>.CreateFailed(
                    new ApplicationException("Invalid guid and token. Invalid request."), "Invalid guid and token. Invalid request.");
            }

            var getActivityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                ActivityId = deserializedPayload.ActivityId
            });
            if(!getActivityRes.Succeeded || getActivityRes.Result is null)
            {
                return AppResult<OteGetRequestPaymentResult>.CreateFailed(new ApplicationException(getActivityRes.Message), getActivityRes.Message);
            }

            var oteActivityRes = await oteFindByHandler.ExecuteAsync(new ActivityService.Interactors.OteFindByHandlerArgs {
                Handler = getActivityRes.Result.Handler,
                IncludePricing = true,
                IncludeSchedule = true
            });
            if(!oteActivityRes.Succeeded || oteActivityRes.Result is null)
            {
                return AppResult<OteGetRequestPaymentResult>.CreateFailed(new ApplicationException(oteActivityRes.Message), oteActivityRes.Message);
            }

            var selectedOteDate = oteActivityRes.Result.OteDates.FirstOrDefault(d => d.Id == deserializedPayload.DateId);
            if(selectedOteDate is null)
            {
                return AppResult<OteGetRequestPaymentResult>.CreateFailed(new ApplicationException("Unable to identify selected ote date."), "Unable to identify selected ote date.");
            }

            return AppResult<OteGetRequestPaymentResult>.CreateSucceeded(new OteGetRequestPaymentResult {
                CustomerId = deserializedPayload.CustomerId,
                ActivityId = deserializedPayload.ActivityId,
                Guid = args.Guid,
                SelectedDate = selectedOteDate.Date.Date,
                SelectedTickets = deserializedPayload.Tickets.Select(t => new OteGetRequestPaymentResult.RequestPaymentTicket {
                    TicketCount  = t.Count,
                    TicketId     = t.Id,
                    SeatNumber   = t.SeatNumber,
                    CategoryUUID = t.CategoryUUID,
                    RowUUID      = t.RowUUID,
                    SeatUUID     = t.SeatUUID
                }),
                Token = args.Token,
                Questions = deserializedPayload.Questions?.Select(q => new OteGetRequestPaymentResult.ProviderQuestion {
                    Answer = q.Answer,
                    Id = q.Id,
                    Question = q.Question
                }),
                ForceCreateTicket = deserializedPayload.ForceCreateTicket,
                Waitlisted = deserializedPayload.Waitlisted,
                WaitListId = deserializedPayload.WaitListId,
                Used = deserializedPayload.Used
            }, "Successfully get payment request details.");
        }
        catch (Exception ex)
        {
            return AppResult<OteGetRequestPaymentResult>.CreateFailed(ex, "An error occured in OteGetRequestPaymentHandler.");
        }
    }

    private class PayloadData 
    {
        public int CustomerId {get; set;}
        public int ActivityId {get; set;}
        public int DateId {get; set;}
        public IEnumerable<Ticket> Tickets {get; set;} = Enumerable.Empty<Ticket>();
        public IEnumerable<ProviderQuestion>? Questions {get; set;}
        public bool ForceCreateTicket {get; set;}

        public bool Waitlisted {get; set;}
        public int WaitListId {get; set;}

        public bool Used {get; set;}
    }

    private class Ticket
    {
        public int Id {get; set;}
        public int Count {get; set;}
        public string SeatNumber { get; set; }
        public string CategoryUUID { get; set; }
        public string RowUUID { get; set; }
        public string SeatUUID { get; set; }

    }

    private class ProviderQuestion
    {
        public int Id {get; set;}
        public string Question {get; set;}
        public string? Answer {get; set;}
    }
}