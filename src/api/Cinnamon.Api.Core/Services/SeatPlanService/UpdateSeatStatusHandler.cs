using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.SeatPlanService.Handler;
using Cinnamon.Api.Core.Services.SeatPlanService.Interactors;
using Cinnamon.Api.Core.Services.SeatPlanService.Interactors.Result;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.SeatPlanService;

public class UpdateSeatStatusHandler : IUpdateSeatStatusHandler
{
    private readonly IGetActivityHandler getActivityHandler;
    private readonly IOteFindByHandler oteFindByHandler;

    public UpdateSeatStatusHandler(IGetActivityHandler getActivityHandler, IOteFindByHandler oteFindByHandler)
    {
        this.getActivityHandler = getActivityHandler;
        this.oteFindByHandler = oteFindByHandler;
    }

    public AppResult<UpdateSeatStatusResult> Execute(UpdateSeatStatusArgs args)
    {
        throw new NotImplementedException();
    }

    public async Task<AppResult<UpdateSeatStatusResult>> ExecuteAsync(UpdateSeatStatusArgs args)
    {
        try
        {
            var activityRes = await getActivityHandler.ExecuteAsync(new ActivityService.Interactors.GetActivityArgs {
                ActivityId = args.ActivityId
            });
            if(!activityRes.Succeeded || activityRes.Result is null)
            {
                return AppResult<UpdateSeatStatusResult>.CreateFailed(
                    new ApplicationException(activityRes.Message), activityRes.Message);
            }
            var activity = activityRes.Result;

            var oteRes = await oteFindByHandler.ExecuteAsync(new ActivityService.Interactors.OteFindByHandlerArgs {
                Handler = activity.Handler,
                IncludeSchedule = true,
                IncludePricing = true
            });
            if(!oteRes.Succeeded || oteRes.Result is null)
            {
                return AppResult<UpdateSeatStatusResult>.CreateFailed(
                    new ApplicationException(oteRes.Message), oteRes.Message);
            }
            var oteActivity = oteRes.Result;
            

        }
        catch (Exception ex)
        {
            return AppResult<UpdateSeatStatusResult>.CreateFailed(ex, "An error occured in UpdateSeatStatusHandler");
        }
    }
}