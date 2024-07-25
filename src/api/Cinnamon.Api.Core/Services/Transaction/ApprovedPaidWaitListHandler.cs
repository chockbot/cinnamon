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

namespace Cinnamon.Api.Core.Services.TransactionService;

public class ApprovedPaidWaitListHandler : IApprovedPaidWaitListHandler
{
    private readonly IActivityData activityData;
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly ApplicationConfig applicationConfig;
    private readonly IOteCreateRequestPaymentHandler createRequestPaymentHandler;

    public ApprovedPaidWaitListHandler(IActivityData activityData, IGetProfileHandler getProfileHandler,
        IJsonSerializationProvider jsonSerializationProvider, IGetActivityHandler getActivityHandler,
        ApplicationConfig applicationConfig, IOteCreateRequestPaymentHandler createRequestPaymentHandler)
    {
        this.activityData = activityData;
        this.getProfileHandler = getProfileHandler;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.getActivityHandler = getActivityHandler;
        this.applicationConfig = applicationConfig;
        this.createRequestPaymentHandler = createRequestPaymentHandler;
    }
    
    public AppResult<ApprovedPaidWaitListResult> Execute(ApprovedPaidWaitListArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<ApprovedPaidWaitListResult>> ExecuteAsync(ApprovedPaidWaitListArgs args)
    {
        try
        {
            var profileRes = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!profileRes.Succeeded || profileRes.Result is null)
            {
                return AppResult<ApprovedPaidWaitListResult>.CreateFailed(new ApplicationException(profileRes.Message), profileRes.Message);
            }
            var profile = profileRes.Result;

            var waitListRes = await activityData.GetOteWaitList(args.WaitListId);
            if(!waitListRes.Succeeded || waitListRes.Result is null || !waitListRes.Result.IsSuccess)
            {
                return AppResult<ApprovedPaidWaitListResult>.CreateFailed(new ApplicationException("Unable to get waitlist."), "Unable to get waitlist.");
            }
            var waitlist = waitListRes.Result.Result;

            var deserializedPayload = jsonSerializationProvider.Deserialize<PayloadData>(waitlist.Payload);
            if(deserializedPayload is null || deserializedPayload.Tickets is null || deserializedPayload.Tickets.Count() == 0)
            {
                return AppResult<ApprovedPaidWaitListResult>.CreateFailed(
                    new ApplicationException("Unable to locate selected tickets."), "Unable to locate selected tickets.");
            }

            var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                ActivityId = waitlist.ActivityId,
                IncludeCustomer = true
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<ApprovedPaidWaitListResult>.CreateFailed(new ApplicationException(activityRes.Message), activityRes.Message);
            }
            if(activityRes.Result.Owner?.Id != profile.Id)
            {
                return AppResult<ApprovedPaidWaitListResult>.CreateFailed(
                    new ApplicationException("Invalid selected activity. Invalid request."), "Invalid selected activity. Invalid request.");
            }
            var activity = activityRes.Result;

            var firstTicket = deserializedPayload.Tickets.FirstOrDefault();
            if(firstTicket is null)
            {
                return AppResult<ApprovedPaidWaitListResult>.CreateFailed(
                    new ApplicationException("Invalid waitlist data. Invalid request."), "Invalid waitlist data. Invalid request.");
            }

            var createRequestPaymentRes = await createRequestPaymentHandler.ExecuteAsync(new OteCreateRequestPaymentArgs {
                ActivityId = waitlist.ActivityId,
                CustomerId = waitlist.CustomerId,
                SelectedDate = firstTicket.Date,
                SelectedTickets = deserializedPayload.Tickets.Select(t => new OteCreateRequestPaymentArgs.RequestPaymentTicket {
                    TicketCount = t.Count,
                    TicketId = t.Id
                })
            });
            if(!createRequestPaymentRes.Succeeded || createRequestPaymentRes.Result is null)
            {
                return AppResult<ApprovedPaidWaitListResult>.CreateFailed(
                    new ApplicationException(createRequestPaymentRes.Message), createRequestPaymentRes.Message);
            }
            var createdToken = createRequestPaymentRes.Result;

            // create link for ticket details
            var url = applicationConfig.FrontendUrl
                .AppendPathSegment("payment")
                .AppendPathSegment("ote")
                .AppendPathSegment(activity.Handler)
                .SetQueryParam("guid", createdToken.Guid)
                .SetQueryParam("token", createdToken.Token);
            
            return AppResult<ApprovedPaidWaitListResult>.CreateSucceeded(new ApprovedPaidWaitListResult {PurchaseLink = url}, "Successfully approved paid wait list.");
        }
        catch (Exception ex)
        {
            return AppResult<ApprovedPaidWaitListResult>.CreateFailed(ex, "An error occured in ApprovedPaidWaitListHandler.");
        }
    }

    private class Ticket 
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public int OteDateId {get; set;}
        public int Count {get; set;}
        public DateTime Date {get; set;}

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