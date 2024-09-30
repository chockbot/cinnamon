using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class OteCreateRequestPaymentHandler : IOteCreateRequestPaymentHandler
{
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IOteFindByHandler oteFindByHandler;
    private readonly ITokenGeneratedData tokenGeneratedData;
    private readonly ITokenGeneratorProvider tokenGeneratorProvider;

    public OteCreateRequestPaymentHandler(IJsonSerializationProvider jsonSerializationProvider,
        IGetActivityHandler getActivityHandler, IOteFindByHandler oteFindByHandler,
        ITokenGeneratedData tokenGeneratedData, ITokenGeneratorProvider tokenGeneratorProvider)
    {
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.getActivityHandler = getActivityHandler;
        this.oteFindByHandler = oteFindByHandler;
        this.tokenGeneratedData = tokenGeneratedData;
        this.tokenGeneratorProvider = tokenGeneratorProvider;
    }
    
    public AppResult<OteCreateRequestPaymentResult> Execute(OteCreateRequestPaymentArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OteCreateRequestPaymentResult>> ExecuteAsync(OteCreateRequestPaymentArgs args)
    {
        try
        {
            var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                ActivityId = args.ActivityId
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<OteCreateRequestPaymentResult>.CreateFailed(new ApplicationException(activityRes.Message), activityRes.Message);
            }

            var oteActivityRes = await oteFindByHandler.ExecuteAsync(new ActivityService.Interactors.OteFindByHandlerArgs {
                Handler = activityRes.Result.Handler,
                IncludePricing = true,
                IncludeSchedule = true
            });
            if(!oteActivityRes.Succeeded || oteActivityRes.Result is null)
            {
                return AppResult<OteCreateRequestPaymentResult>.CreateFailed(new ApplicationException(oteActivityRes.Message), oteActivityRes.Message);
            }
            var oteActivity = oteActivityRes.Result;

            var oteDate = oteActivity.OteDates.FirstOrDefault(d => d.Date.Date == args.SelectedDate.Date);
            if(oteDate is null)
            {
                return AppResult<OteCreateRequestPaymentResult>.CreateFailed(
                    new ApplicationException("Invalid selected event date."), "Invalid selected event date.");
            }

            var dateTickets = oteActivity.Pricings.Where(p => p.OteDateId == oteDate.Id);
            List<Ticket> validTickets = new();

            foreach(var ticket in args.SelectedTickets.DistinctBy(t => t.TicketId))
            {
                var dateTicket = dateTickets.FirstOrDefault(t => t.Id == ticket.TicketId);
                if(dateTicket is not null)
                {
                    validTickets.Add(new Ticket {
                        Count        = ticket.TicketCount,
                        Id           = ticket.TicketId,
                        SeatNumber   = ticket.SeatNumber,
                        CategoryUUID = ticket.CategoryUUID, 
                        RowUUID      = ticket.RowUUID,
                        SeatUUID     =ticket.SeatUUID
                    });
                }
            }

            var payload = new PayloadData {
                CustomerId = args.CustomerId,
                ActivityId = args.ActivityId,
                DateId = oteDate.Id,
                Tickets = validTickets.Select(t => new Ticket {
                    Count        = t.Count,
                    Id           = t.Id,
                    SeatNumber   = t.SeatNumber,
                    CategoryUUID = t.CategoryUUID,
                    RowUUID      = t.RowUUID,
                    SeatUUID     = t.SeatUUID
                }),
                Questions = args.Questions?.Select(q => new ProviderQuestion {
                    Answer = q.Answer,
                    Question = q.Question,
                    Id = q.Id
                }),
                ForceCreateTicket = args.ForceCreateTicket,
                Waitlisted = args.Waitlisted,
                WaitListId = args.WaitListId,
                Used = args.Used
            };

            bool updateTokenPayload = !string.IsNullOrEmpty(args.Guid) && !string.IsNullOrEmpty(args.Token);
            var serializedPayload = jsonSerializationProvider.Serialize(payload);

            const string TOKEN_TYPE = "REQUEST-PAYMENT";

            if(updateTokenPayload)
            {
                var getTokenRes = await tokenGeneratedData.GetTokenGenerated(args.Guid, args.Token);
                if(!getTokenRes.Succeeded || getTokenRes.Result is null || !getTokenRes.Result.IsSuccess)
                {
                    return AppResult<OteCreateRequestPaymentResult>.CreateFailed(
                        new ApplicationException("Invalid guid and token. Invalid request."), "Invalid guid and token. Invalid request.");
                }
                var token = getTokenRes.Result.Result;

                if(!token.TokenType.Equals(TOKEN_TYPE, StringComparison.OrdinalIgnoreCase))
                {
                    return AppResult<OteCreateRequestPaymentResult>.CreateFailed(
                        new ApplicationException("Invalid guid and token. Invalid request."), "Invalid guid and token. Invalid request.");
                }

                var deserializedData = jsonSerializationProvider.Deserialize<PayloadData>(token.Payload);
                if(deserializedData is null || deserializedData.CustomerId != args.CustomerId)
                {
                    return AppResult<OteCreateRequestPaymentResult>.CreateFailed(
                        new ApplicationException("Invalid guid and token. Invalid request."), "Invalid guid and token. Invalid request.");
                }

                var updateTokenRes = await tokenGeneratedData.UpdateToken(new Framework.ApiCommand.ApiData.TokenGenerated.Request.UpdateTokenArgs {
                    Payload = serializedPayload
                }, token.Id);
                if(!updateTokenRes.Succeeded || updateTokenRes.Result is null || !updateTokenRes.Result.IsSuccess)
                {
                    return AppResult<OteCreateRequestPaymentResult>.CreateFailed(
                        new ApplicationException("An error occured when creating request payment."), "An error occured when creating request payment.");
                }
            }

            if(!updateTokenPayload)
            {
                var generatedToken = tokenGeneratorProvider.Generator();
                args.Guid = generatedToken.Guid;
                args.Token = generatedToken.Token;

                var createTokenRes = await tokenGeneratedData.CreateTokenGenerated(new Framework.ApiCommand.ApiData.TokenGenerated.Request.CreateTokenArgs {
                    Guid = args.Guid,
                    Payload = serializedPayload,
                    Token = args.Token,
                    TokenType = TOKEN_TYPE
                });
                if(!createTokenRes.Succeeded || createTokenRes.Result is null || !createTokenRes.Result.IsSuccess)
                {
                    return AppResult<OteCreateRequestPaymentResult>.CreateFailed(
                        new ApplicationException("An error occured when creating request payment."), "An error occured when creating request payment.");
                }
            }

            return AppResult<OteCreateRequestPaymentResult>.CreateSucceeded(new OteCreateRequestPaymentResult {
                ActivityId = args.ActivityId,
                Guid = args.Guid,
                SelectedDate = args.SelectedDate,
                SelectedTickets = validTickets.Select(t => new OteCreateRequestPaymentResult.RequestPaymentTicket {
                    TicketCount  = t.Count,
                    TicketId     = t.Id,
                    SeatNumber   = t.SeatNumber,
                    CategoryUUID = t.CategoryUUID,
                    RowUUID      = t.RowUUID,
                    SeatUUID     = t.SeatUUID
                }),
                Token = args.Token,
                Used = args.Used,
            }, "Successfully request payment");           
        }
        catch (Exception ex)
        {
            return AppResult<OteCreateRequestPaymentResult>.CreateFailed(ex, "An error occured in OteCreateRequestPaymentHandler.");
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