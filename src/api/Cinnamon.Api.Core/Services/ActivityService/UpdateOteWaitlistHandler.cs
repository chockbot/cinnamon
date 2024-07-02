using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.OteWaitlist;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;
public class UpdateOteWaitlistHandler : IUpdateOteWaitlistHandler
{
    private readonly IActivityData activityData;
    private readonly IMapper mapper;
    public UpdateOteWaitlistHandler(IActivityData activityData, IMapper mapper)
    {
        this.activityData = activityData;
        this.mapper = mapper;
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
                Status       = args.Status
            });
            if (!oteWaitlist.Succeeded || oteWaitlist.Result is null || !oteWaitlist.Result.IsSuccess)
            {
                return AppResult<UpdateOteWaitlistResult>.CreateFailed(new ApplicationException(oteWaitlist.Message), oteWaitlist.Message);
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
