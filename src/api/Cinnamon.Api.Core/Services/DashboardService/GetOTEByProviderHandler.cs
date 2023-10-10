using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;
public class GetOTEByProviderHandler: IGetOTEByProviderHandler
{
    private readonly IActivityData activityData;
    public GetOTEByProviderHandler(IActivityData activityData)
    {
        this.activityData = activityData;
    }

    public AppResult<GetOTEByProviderResult> Execute(GetOTEByProviderArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<GetOTEByProviderResult>.CreateFailed(ex, "An error occured in GetOTEByProviderHandler");
        }
    }

    public async Task<AppResult<GetOTEByProviderResult>> ExecuteAsync(GetOTEByProviderArgs args)
    {
        try
        {
            var result = await activityData.GetOTEByProvider(new Framework.ApiCommand.ApiData.OteTicket.Request.GetOTEByProvideArgs
            {
                Id = args.Id
            });
            if (!result.Succeeded || result.Result == null || !result.Result.IsSuccess)
            {
                return AppResult<GetOTEByProviderResult>.CreateFailed(new ApplicationException(result.Result?.ErrorInfo?.Message), result.Message);
            }

            return AppResult<GetOTEByProviderResult>.CreateSucceeded(new GetOTEByProviderResult 
            {
                OTEActivities = result.Result.Result.Select(s =>
                {
                    return new GetOTEByProviderResult.OTEActivity
                    {
                        Id               = s.Id,
                        ExperienceTypeId = s.ExperienceTypeId,
                        EventName        = s.EventName,
                        Description      = s.Description,
                        Handler          = s.Handler,
                        CityName         = s.CityName,
                        RegionName       = s.RegionName,
                        PinnedLocation   = s.PinnedLocation,
                        EventImage       = s.EventImage,
                        ScheduleFrom     = s.ScheduleFrom,
                        ScheduleTo       = s.ScheduleTo,
                        Slots            = s.Slots,
                        Sold             = s.Sold,
                        Available        = s.Available
                    };
                })
            }, "Successfully get ote activities");
        }
        catch (Exception ex)
        {
            return AppResult<GetOTEByProviderResult>.CreateFailed(ex, "An error occured in GetOTEByProviderHandler");
        }
    }
}
