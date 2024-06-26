using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
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

    public CreateOteWaitlistHandler(IActivityData activityData, IMapper mapper)
    {
        this.activityData = activityData;
        this.mapper       = mapper;
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
            var result = mapper.Map<OteWaitlistDTO, CreateOteWaitlistResult>(oteWaitlist.Result.Result);
            return AppResult<CreateOteWaitlistResult>.CreateSucceeded(result, "Successfully created ote waitlist");
        }
        catch (Exception ex)
        {
            return AppResult<CreateOteWaitlistResult>.CreateFailed(ex, "An error occured in ICreateOteWaitlistHandler");
        }
    }
}
