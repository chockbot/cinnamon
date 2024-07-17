using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Providers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class ApprovedWaitListHandler : IApprovedWaitListHandler
{
    private readonly IActivityData activityData;
    private readonly IJsonSerializationProvider jsonSerializationProvider;
    private readonly IApprovedFreeWaitListHandler approvedFreeWaitListHandler;
    private readonly IApprovedPaidWaitListHandler approvedPaidWaitListHandler;
    private readonly IDynamicContentData dynamicContentData;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IOteApprovedNotificationHandler oteApprovedNotificationHandler;
    private readonly IGetCustomerByIdHandler getCustomerByIdHandler;
    private readonly IOteFindByHandler oteFindByHandler;

    public ApprovedWaitListHandler(IActivityData activityData, 
        IJsonSerializationProvider jsonSerializationProvider, 
        IApprovedFreeWaitListHandler approvedFreeWaitListHandler,
        IApprovedPaidWaitListHandler approvedPaidWaitListHandler,
        IDynamicContentData dynamicContentData,
        IGetActivityHandler getActivityHandler,
        IOteApprovedNotificationHandler oteApprovedNotificationHandler,
        IGetCustomerByIdHandler getCustomerByIdHandler,
        IOteFindByHandler oteFindByHandler)
    {
        this.activityData = activityData;
        this.jsonSerializationProvider = jsonSerializationProvider;
        this.approvedFreeWaitListHandler = approvedFreeWaitListHandler;
        this.approvedPaidWaitListHandler = approvedPaidWaitListHandler;
        this.dynamicContentData = dynamicContentData;
        this.getActivityHandler = getActivityHandler;
        this.oteApprovedNotificationHandler = oteApprovedNotificationHandler;
        this.getCustomerByIdHandler = getCustomerByIdHandler;
        this.oteFindByHandler = oteFindByHandler;
    }

    public AppResult<ApprovedWaitListResult> Execute(ApprovedWaitListArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<ApprovedWaitListResult>> ExecuteAsync(ApprovedWaitListArgs args)
    {
        try
        {
            var waitListRes = await activityData.GetOteWaitList(args.WaitListId);
            if(!waitListRes.Succeeded || waitListRes.Result is null || !waitListRes.Result.IsSuccess)
            {
                return AppResult<ApprovedWaitListResult>.CreateFailed(new ApplicationException("Unable to get waitlist."), "Unable to get waitlist.");
            }
            var waitlist = waitListRes.Result.Result;

            var purchasedCustomerRes = await getCustomerByIdHandler.ExecuteAsync(new AccountService.Interactors.GetCustomerByIdArgs {
                Id = waitlist.CustomerId
            });
            if(!purchasedCustomerRes.Succeeded || purchasedCustomerRes.Result is null)
            {
                return AppResult<ApprovedWaitListResult>.CreateFailed(
                    new ApplicationException("Unable to find customer id."), "Unable to find customer id.");
            }
            var purchasedCustomer = purchasedCustomerRes.Result;

            var deserializedPayload = jsonSerializationProvider.Deserialize<PayloadData>(waitlist.Payload);

            if(deserializedPayload is null || deserializedPayload.Tickets is null || deserializedPayload.Tickets.Count() == 0)
            {
                return AppResult<ApprovedWaitListResult>.CreateFailed(
                    new ApplicationException("Unable to locate selected tickets."), "Unable to locate selected tickets.");
            }

            bool havePaidTickets = deserializedPayload.Tickets.Any(t => t.Price > 0);

            var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                ActivityId = waitlist.ActivityId,
                IncludeCustomer = true,
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<ApprovedWaitListResult>.CreateFailed(new ApplicationException(activityRes.Message), activityRes.Message);
            }
            var activity = activityRes.Result;

            var oteActivityRes = await oteFindByHandler.ExecuteAsync(new ActivityService.Interactors.OteFindByHandlerArgs {
                Handler = activity.Handler,
                IncludePricing = true,
                IncludeSchedule = true
            });
            if(!oteActivityRes.Succeeded || oteActivityRes.Result is null)
            {
                return AppResult<ApprovedWaitListResult>.CreateFailed(new ApplicationException(oteActivityRes.Message), oteActivityRes.Message);
            }
            var oteActivity = oteActivityRes.Result;

            var firstTicket = deserializedPayload.Tickets.First();
            var oteDate = oteActivity.OteDates.First(d => d.Id == firstTicket.OteDateId);

            if(oteDate is null)
            {
                return AppResult<ApprovedWaitListResult>.CreateFailed(
                    new ApplicationException("Unable to find ote date id."), "Unable to find ote date id.");
            }

            string resultLink = string.Empty;

            if(havePaidTickets)
            {
                var approvedPaidRes = await approvedPaidWaitListHandler.ExecuteAsync(new ApprovedPaidWaitListArgs {
                    WaitListId = args.WaitListId
                });
                if(!approvedPaidRes.Succeeded || approvedPaidRes.Result is null)
                {
                    return AppResult<ApprovedWaitListResult>.CreateFailed(new ApplicationException(approvedPaidRes.Message), approvedPaidRes.Message);
                }

                resultLink = approvedPaidRes.Result.PurchaseLink;
            }

            if(!havePaidTickets)
            {
                var approvedFreeRes = await approvedFreeWaitListHandler.ExecuteAsync(new ApprovedFreeWaitListArgs {
                    WaitListId = args.WaitListId
                });
                if(!approvedFreeRes.Succeeded || approvedFreeRes.Result is null)
                {
                    return AppResult<ApprovedWaitListResult>.CreateFailed(new ApplicationException(approvedFreeRes.Message), approvedFreeRes.Message);
                }

                resultLink = approvedFreeRes.Result.TicketLink;
            }

            var updateWaitListRes = await activityData.UpdateOteWaitlist(new Framework.ApiCommand.ApiData.OteWaitlist.Request.UpdateOteWaitlistArgs {
                ActivityId = waitlist.ActivityId,
                CustomerId = waitlist.CustomerId,
                CustomerName =  waitlist.CustomerName,
                Id = waitlist.Id,
                Payload = waitlist.Payload,
                ProviderId = waitlist.ProviderId,
                ScheduleId = waitlist.ScheduleId,
                Status = 2
            });
            if(!updateWaitListRes.Succeeded || updateWaitListRes.Result is null || !updateWaitListRes.Result.IsSuccess)
            {
                return AppResult<ApprovedWaitListResult>.CreateFailed(
                    new ApplicationException("An error occured when approving waitlist."), "An error occured when approving waitlist.");
            }

            string subject = string.Empty, body = string.Empty;

            // get custom subject and custom body for approve waitlist
            var customSubBodyRes = await dynamicContentData.GetEmailTemplates(new Framework.ApiCommand.ApiData.DynamicContent.Request.GetEmailTemplatesArgs
            {
                ActivityId = activity.Id,
                ProviderId = activity.Owner?.Id ?? 0,
                TemplateType = Cinnamon.Framework.Enums.EmailTemplateType.OteConfirmed.ToString()
            });
            if (customSubBodyRes.Succeeded && customSubBodyRes.Result is not null && 
                customSubBodyRes.Result.IsSuccess && customSubBodyRes.Result.Result.Any())
            {
                var template = customSubBodyRes.Result.Result.First();
                subject = template.Subject;
                body = template.Body;
            }

            //send email approval email
            var sendApprovalEmail = await oteApprovedNotificationHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.OteApprovedNotificationArgs
            {
                Body = body,
                CustomerEmail = purchasedCustomer.Email,
                CustomerName = $"{purchasedCustomer.FirstName} {purchasedCustomer.LastName}",
                EventDate = oteDate.Date,
                EventLocation = oteActivity.ExperienceTypeId == 2 ? "Online" : $"{oteActivity.PinnedLocation}".Trim(),
                EventName = oteActivity.EventName,
            });           

            return AppResult<ApprovedWaitListResult>.CreateSucceeded(new ApprovedWaitListResult {Link = resultLink}, "Successfully approved waitlist.");
        }
        catch (Exception ex)
        {
            return AppResult<ApprovedWaitListResult>.CreateFailed(ex, "An error occured in ApprovedWaitListHandler.");
        }
    }

    private class Ticket 
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public int OteDateId {get; set;}
        public decimal Price {get; set;}
    }

    private class PayloadData 
    {
        public IEnumerable<Ticket> Tickets {get; set;}
        public int TransactionId {get; set;}
    }
}