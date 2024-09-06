using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class OteRequestPaymentHandler : IOteRequestPaymentHandler
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IOteCreateRequestPaymentHandler oteCreateRequestPaymentHandler;

    public OteRequestPaymentHandler(IGetProfileHandler getProfileHandler, IOteCreateRequestPaymentHandler oteCreateRequestPaymentHandler)
    {
        this.getProfileHandler = getProfileHandler;
        this.oteCreateRequestPaymentHandler = oteCreateRequestPaymentHandler;
    }

    public AppResult<OteRequestPaymentResult> Execute(OteRequestPaymentArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<OteRequestPaymentResult>> ExecuteAsync(OteRequestPaymentArgs args)
    {
        try
        {
            var profileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!profileRes.Succeeded || profileRes.Result is null)
            {
                return AppResult<OteRequestPaymentResult>.CreateFailed(new ApplicationException(profileRes.Message), profileRes.Message);
            }
            var profile = profileRes.Result;

            var createPaymentRequestRes = await oteCreateRequestPaymentHandler.ExecuteAsync(new OteCreateRequestPaymentArgs {
                ActivityId = args.ActivityId,
                CustomerId = profile.Id,
                Guid = args.Guid,
                Token = args.Token,
                SelectedDate = args.SelectedDate,
                SelectedTickets = args.SelectedTickets.Select(t => new OteCreateRequestPaymentArgs.RequestPaymentTicket {
                    TicketCount = t.TicketCount,
                    TicketId = t.TicketId,
                    SeatNumber = t.SeatNumber
                }),
                Questions = args.Questions?.Select(q => new OteCreateRequestPaymentArgs.ProviderQuestion {
                    Answer = q.Answer,
                    Question = q.Question,
                    Id = q.Id
                })
            });
            if(!createPaymentRequestRes.Succeeded || createPaymentRequestRes.Result is null)
            {
                return AppResult<OteRequestPaymentResult>.CreateFailed(new ApplicationException(createPaymentRequestRes.Message), createPaymentRequestRes.Message);
            }
            var createdRequest = createPaymentRequestRes.Result;

            return AppResult<OteRequestPaymentResult>.CreateSucceeded(new OteRequestPaymentResult {
                ActivityId = createdRequest.ActivityId,
                Guid = createdRequest.Guid,
                SelectedDate = createdRequest.SelectedDate,
                SelectedTickets = createdRequest.SelectedTickets.Select(t => new OteRequestPaymentResult.RequestPaymentTicket {
                    TicketCount = t.TicketCount,
                    TicketId = t.TicketId,
                    SeatNumber = t.SeatNumber
                }),
                Token = createdRequest.Token,
            }, "Successfully request payment");
        }
        catch (Exception ex)
        {
            return AppResult<OteRequestPaymentResult>.CreateFailed(ex, "An error occured in OteRequestPaymentHandler.");
        }
    }
}