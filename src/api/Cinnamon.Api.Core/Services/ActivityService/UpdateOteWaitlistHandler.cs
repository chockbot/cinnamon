using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.NotificationDriver.Handler;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteWaitlist;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;
public class UpdateOteWaitlistHandler : IUpdateOteWaitlistHandler
{
    private readonly IOteApprovedNotificationHandler oteApprovedNotificationHandler;
    private readonly IOteDeclinedNotificationHandler declinedNotificationHandler;
    private readonly IDynamicContentData dynamicContentData;
    private readonly IOteFindByHandler oteFindByHandler;
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IActivityData activityData;
    private readonly IMapper mapper;
    public UpdateOteWaitlistHandler(IActivityData activityData, IMapper mapper,
        IOteApprovedNotificationHandler oteApprovedNotificationHandler, 
        IOteDeclinedNotificationHandler declinedNotificationHandler,
        IGetActivityHandler getActivityHandler, IOteFindByHandler oteFindByHandler,
        IDynamicContentData dynamicContentData)
    {
        this.activityData                   = activityData;
        this.mapper                         = mapper;
        this.getActivityHandler             = getActivityHandler;
        this.oteFindByHandler               = oteFindByHandler;
        this.dynamicContentData             = dynamicContentData;
        this.oteApprovedNotificationHandler = oteApprovedNotificationHandler;
        this.declinedNotificationHandler    = declinedNotificationHandler;

    }

    public AppResult<UpdateOteWaitlistResult> Execute(UpdateOteWaitlistArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<UpdateOteWaitlistResult>.CreateFailed(ex, "An error occured in IUpdateOteWaitlistHandler");
        }
    }

    public async Task<AppResult<UpdateOteWaitlistResult>> ExecuteAsync(UpdateOteWaitlistArgs args)
    {
        try
        {
            var oteWaitlist = await activityData.UpdateOteWaitlist(new Framework.ApiCommand.ApiData.OteWaitlist.Request.UpdateOteWaitlistArgs
            {
                Id           = args.Id,
                ActivityId   = args.ActivityId,
                CustomerId   = args.CustomerId,
                CustomerName = args.CustomerName,
                Payload      = args.Payload,
                ProviderId   = args.ProviderId,
                ScheduleId   = args.ScheduleId,
                Status       = args.Status,
            });
            if (!oteWaitlist.Succeeded || oteWaitlist.Result is null || !oteWaitlist.Result.IsSuccess)
            {
                return AppResult<UpdateOteWaitlistResult>.CreateFailed(new ApplicationException(oteWaitlist.Message), oteWaitlist.Message);
            }
            else
            {
                //Get Activity Details
                var activityRes = await getActivityHandler.ExecuteAsync(new GetActivityArgs
                {
                    ActivityId = args.ActivityId,
                    IncludeCustomer = true,
                });
                var oteByHandlerRes = await oteFindByHandler.ExecuteAsync(new OteFindByHandlerArgs
                {
                    Handler = activityRes.Result.Handler,
                    IncludeSchedule = true,
                    IncludeAddress = true
                });
                var oteActivity = oteByHandlerRes.Result;
                string subject = string.Empty;
                string body = string.Empty;

                //0-NO STATUS 1-PENDING 2-APPROVED 3-DECLINED
                if (args.Status == 2)
                {
                    // get custom subject and custom body for approve waitlist
                    var customSubBodyRes = await dynamicContentData.GetEmailTemplates(new Framework.ApiCommand.ApiData.DynamicContent.Request.GetEmailTemplatesArgs
                    {
                        ActivityId = args.ActivityId,
                        ProviderId = args.ProviderId,
                        TemplateType = Cinnamon.Framework.Enums.EmailTemplateType.OteConfirmed.ToString()
                    });
                    if (customSubBodyRes.Succeeded && customSubBodyRes.Result is not null && customSubBodyRes.Result.IsSuccess && customSubBodyRes.Result.Result.Any())
                    {
                        var template = customSubBodyRes.Result.Result.First();
                        subject = template.Subject;
                        body = template.Body;
                    }
                    //send email approval email
                    var sendApprovalEmail = await oteApprovedNotificationHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.OteApprovedNotificationArgs
                    {
                        Body = body,
                        CustomerEmail = args.CustomerEmail,
                        CustomerName = args.CustomerName,
                        EventDate = args.EventDate,
                        EventLocation = oteActivity.ExperienceTypeId == 2 ? "Online" : $"{oteActivity.PinnedLocation}".Trim(),
                        EventName = oteActivity.EventName,
                    });
                }
                else
                {
                    // get custom subject and custom body for declined waitlist
                    var customSubBodyRes = await dynamicContentData.GetEmailTemplates(new Framework.ApiCommand.ApiData.DynamicContent.Request.GetEmailTemplatesArgs
                    {
                        ActivityId = args.ActivityId,
                        ProviderId = args.ProviderId,
                        TemplateType = Cinnamon.Framework.Enums.EmailTemplateType.OteDeclined.ToString()
                    });
                    if (customSubBodyRes.Succeeded && customSubBodyRes.Result is not null && customSubBodyRes.Result.IsSuccess && customSubBodyRes.Result.Result.Any())
                    {
                        var template = customSubBodyRes.Result.Result.First();
                        subject = template.Subject;
                        body = template.Body;
                    }
                    //send email decline email
                    var sendApprovalEmail = await declinedNotificationHandler.ExecuteAsync(new Modules.NotificationDriver.Interactors.OteDeclinedNotificationArgs
                    {
                        Body = body,
                        CustomerEmail = args.CustomerEmail,
                        CustomerName = args.CustomerName,
                        EventName = oteActivity.EventName,
                    });
                }
            }
            var result = mapper.Map<OteWaitlistDTO, UpdateOteWaitlistResult>(oteWaitlist.Result.Result);
            return AppResult<UpdateOteWaitlistResult>.CreateSucceeded(result, "Successfully updated ote waitlist");
        }
        catch (Exception ex)
        {
            return AppResult<UpdateOteWaitlistResult>.CreateFailed(ex, "An error occured in IUpdateOteWaitlistHandler");
        }
    }
}
