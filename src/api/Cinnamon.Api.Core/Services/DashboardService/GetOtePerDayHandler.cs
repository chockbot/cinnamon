using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.DashboardService.Interactors;
using Cinnamon.Api.Core.Services.DashboardService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.DashboardService;

public class GetOtePerDayHandler : IGetOtePerDayHandler
{
    private readonly IActivityData activityData;
    private readonly IGetProfileHandler getProfileHandler;

    public GetOtePerDayHandler(IActivityData activityData, IGetProfileHandler getProfileHandler)
    {
        this.activityData = activityData;
        this.getProfileHandler = getProfileHandler;
    }
    
    public AppResult<GetOtePerDayResult> Execute(GetOtePerDayArgs interactor)
    {
        return ExecuteAsync(interactor).Result;
    }

    public async Task<AppResult<GetOtePerDayResult>> ExecuteAsync(GetOtePerDayArgs interactor)
    {
        try
        {
            var profileRes = await this.getProfileHandler.ExecuteAsync(new ());
            if(!profileRes.Succeeded || profileRes.Result is null)
            {
                return AppResult<GetOtePerDayResult>.CreateFailed(new ApplicationException(profileRes.Message), profileRes.Message);
            }

            var customerId = profileRes.Result.Id;

            var otePerDayRes = await activityData.OtePerDate(new Framework.ApiCommand.ApiData.OteTicket.Request.OtePerDateArgs {
                ProviderId = customerId
            });

            if(!otePerDayRes.Succeeded || otePerDayRes.Result is null || !otePerDayRes.Result.IsSuccess)
            {
                return AppResult<GetOtePerDayResult>.CreateFailed(new ApplicationException(otePerDayRes.Message), otePerDayRes.Message);
            }

            return AppResult<GetOtePerDayResult>.CreateSucceeded(new GetOtePerDayResult {
                OtePerDays = otePerDayRes.Result.Result.Select(e => {
                    return new GetOtePerDayResult.OtePerDay {
                        ActivityId       = e.ActivityId,
                        CityName         = e.CityName,
                        Date             = e.Date,
                        DateEnd          = e.DateEnd,
                        DateId           = e.DateId,
                        DateStart        = e.DateStart,
                        Description      = e.Description,
                        EventImage       = e.EventImage,
                        ExperienceTypeId = e.ExperienceTypeId,
                        Handler          = e.Handler,
                        PinnedLocation   = e.PinnedLocation,
                        RegionName       = e.RegionName,
                        Title            = e.Title,
                        ForceDisable     = e.ForceDisable,
                        ReserveSeat      = e.ReserveSeat
                    };
                })
            }, "Ote per day successfully get.");
        }
        catch (Exception ex)
        {
            return AppResult<GetOtePerDayResult>.CreateFailed(ex, "An error occured in GetOtePerDayHandler");
        }
    }
}