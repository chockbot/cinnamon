using AutoMapper;
using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;
public class GetOteWaitlistByProviderHandler : IGetOteWaitlistByProviderHandler
{
    private readonly IActivityData activityData;
    private readonly IMapper mapper;

    public GetOteWaitlistByProviderHandler(IActivityData activityData, IMapper mapper)
    {
        this.activityData = activityData;
        this.mapper       = mapper;
    }

    public AppResult<GetOteWaitlistByProviderResult> Execute(GetOteWaitlistByProviderArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetOteWaitlistByProviderResult>.CreateFailed(ex, "An error occured in GetOteWaitlistByProviderHandler");
        }
    }

    public async Task<AppResult<GetOteWaitlistByProviderResult>> ExecuteAsync(GetOteWaitlistByProviderArgs args)
    {
        try
        {
            var oteWaitlist = await activityData.GetOteWaitlistByProvider(new Framework.ApiCommand.ApiData.OteWaitlist.Request.GetOteWaitlistByProviderArgs
            {
                ProviderId = args.ProviderId,
                ActivityId = args.ActivityId,
                Status     = args.Status
            });
            if (!oteWaitlist.Succeeded || oteWaitlist.Result is null || !oteWaitlist.Result.IsSuccess)
            {
                return AppResult<GetOteWaitlistByProviderResult>.CreateFailed(
                    new ApplicationException(oteWaitlist.Result?.ErrorInfo?.Message), oteWaitlist.Message);
            }
            var result = mapper.Map<IEnumerable<GetOteWaitlistByProviderResult.OteWaitlist>>(oteWaitlist.Result.Result);

            return AppResult<GetOteWaitlistByProviderResult>.CreateSucceeded(
              new GetOteWaitlistByProviderResult { OteWaitlists = result }, "Successfully get ote already booked dates.");
        }
        catch (Exception ex)
        {
            return AppResult<GetOteWaitlistByProviderResult>.CreateFailed(ex, "An error occured in GetOteWaitlistByProviderHandler.");
        }
    }
}
