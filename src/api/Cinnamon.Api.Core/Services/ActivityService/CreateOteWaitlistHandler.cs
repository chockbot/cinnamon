using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteWaitlist;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;
public class CreateOteWaitlistHandler : ICreateOteWaitlistHandler
{
    private readonly IActivityData activityData;
    private readonly IMapper mapper;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IGetCustomerByIdHandler getCustomerByIdHandler;
    private readonly IOtePendingNotificationHandler pendingNotificationHandler;
    private readonly IDynamicContentData dynamicContentData;

    public CreateOteWaitlistHandler(IActivityData activityData, IMapper mapper,
        IGetActivityHandler getActivityHandler, IGetCustomerByIdHandler getCustomerByIdHandler,
        IOtePendingNotificationHandler pendingNotificationHandler, IDynamicContentData dynamicContentData)
    {
        this.activityData = activityData;
        this.mapper       = mapper;
        this.getActivityHandler = getActivityHandler;
        this.getCustomerByIdHandler = getCustomerByIdHandler;
        this.pendingNotificationHandler = pendingNotificationHandler;
        this.dynamicContentData = dynamicContentData;
    }

    public AppResult<CreateOteWaitlistResult> Execute(CreateOteWaitlistArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<CreateOteWaitlistResult>.CreateFailed(ex, "An error occured in ICreateOteWaitlistHandler");
        }
    }

    public async Task<AppResult<CreateOteWaitlistResult>> ExecuteAsync(CreateOteWaitlistArgs args)
    {
        try
        {
            var activityRes = await getActivityHandler.ExecuteAsync(new GetActivityArgs {
                ActivityId = args.ActivityId,
                IncludeCustomer = true
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<CreateOteWaitlistResult>.CreateFailed(new ApplicationException(activityRes.Message), activityRes.Message);
            }
            var activity = activityRes.Result;

            var customerRes = await getCustomerByIdHandler.ExecuteAsync(new AccountService.Interactors.GetCustomerByIdArgs {
                Id = args.CustomerId
            });
            if(!customerRes.Succeeded || customerRes.Result is null)
            {
                return AppResult<CreateOteWaitlistResult>.CreateFailed(new ApplicationException(customerRes.Message), customerRes.Message);
            }
            var customer = customerRes.Result;
            
            var oteWaitlist = await activityData.CreateOteWaitlist(new Framework.ApiCommand.ApiData.OteWaitlist.Request.CreateOteWaitlistArgs
            {
                ActivityId = args.ActivityId,
                CustomerId = args.CustomerId,
                CustomerName = args.CustomerName,
                Payload = args.Payload,
                ProviderId = args.ProviderId,
                ScheduleId = args.ScheduleId,
                Status = args.Status,
            });
            if (!oteWaitlist.Succeeded || oteWaitlist.Result is null || !oteWaitlist.Result.IsSuccess)
            {
                return AppResult<CreateOteWaitlistResult>.CreateFailed(new ApplicationException(oteWaitlist.Message), oteWaitlist.Message);
            }

            // get custom template body and subject
            string subject = string.Empty;
            string body = string.Empty;
            var dynamicContentRes = await dynamicContentData.GetEmailTemplates(new Framework.ApiCommand.ApiData.DynamicContent.Request.GetEmailTemplatesArgs {
                ActivityId = activity.Id,
                ProviderId = activity.Owner?.Id,
                TemplateType = Cinnamon.Framework.Enums.EmailTemplateType.OtePending.ToString()
            });
            if(dynamicContentRes.Succeeded && dynamicContentRes.Result is not null && !dynamicContentRes.Result.IsSuccess)
            {
                var defaultTemplate = dynamicContentRes.Result.Result.FirstOrDefault();
                if(defaultTemplate is not null)
                {
                    subject = defaultTemplate.Subject;
                    body = defaultTemplate.Body;
                }
            }

            var createPendingNotification = await pendingNotificationHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.OtePendingNotificationArgs {
                Body = body,
                CustomerEmail = customer.Email,
                CustomerName = $"{customer.FirstName} {customer.LastName}",
                EventName = activity.Title,
                Subject = subject
            });

            var result = mapper.Map<OteWaitlistDTO, CreateOteWaitlistResult>(oteWaitlist.Result.Result);
            return AppResult<CreateOteWaitlistResult>.CreateSucceeded(result, "Successfully created ote waitlist");
        }
        catch (Exception ex)
        {
            return AppResult<CreateOteWaitlistResult>.CreateFailed(ex, "An error occured in ICreateOteWaitlistHandler");
        }
    }
}
