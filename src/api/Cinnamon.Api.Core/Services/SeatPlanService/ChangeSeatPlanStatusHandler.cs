using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Cinnamon.Api.Core.Services.AdminService.Handlers;
using Cinnamon.Api.Core.Services.SeatPlanService.Handler;
using Cinnamon.Api.Core.Services.SeatPlanService.Interactors;
using Cinnamon.Api.Core.Services.SeatPlanService.Interactors.Result;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.SeatPlanService;

public class ChangeSeatPlanStatusHandler : IChangeSeatPlanStatusHandler
{
    private readonly IGetProfileHandler getProfileHandler;
    private readonly IGetAdminUserByEmailHandler getAdminUserByEmailHandler;
    private readonly ISeatPlanData seatPlanData;

    public ChangeSeatPlanStatusHandler(IGetProfileHandler getProfileHandler, IGetAdminUserByEmailHandler getAdminUserByEmailHandler,
        ISeatPlanData seatPlanData)
    {
        this.getProfileHandler = getProfileHandler;
        this.getAdminUserByEmailHandler = getAdminUserByEmailHandler;
        this.seatPlanData = seatPlanData;
    }

    public AppResult<ChangeSeatPlanStatusResult> Execute(ChangeSeatPlanStatusArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<ChangeSeatPlanStatusResult>> ExecuteAsync(ChangeSeatPlanStatusArgs args)
    {
        try
        {
            var profile = await getProfileHandler.ExecuteAsync(new AccountService.Interactors.GetProfileArgs {});
            if(!profile.Succeeded || profile.Result is null)
            {
                return AppResult<ChangeSeatPlanStatusResult>.CreateFailed(
                    new ApplicationException(profile.Message), profile.Message);
            }
            var profileData = profile.Result;

            var adminUser = await getAdminUserByEmailHandler.ExecuteAsync(new AdminService.Interactors.GetAdminUserByEmailArgs {Email = profileData.Email});
            if(!adminUser.Succeeded || adminUser.Result is null)
            {
                return AppResult<ChangeSeatPlanStatusResult>.CreateFailed(
                    new ApplicationException(adminUser.Message), adminUser.Message);
            }
            var adminUserData = adminUser.Result;

            var seatPlanRes = await seatPlanData.GetSeatPlanTemplateByIdAsync(args.Id);
            if(!seatPlanRes.Succeeded || seatPlanRes.Result is null || !seatPlanRes.Result.IsSuccess)
            {
                return AppResult<ChangeSeatPlanStatusResult>.CreateFailed(
                    new ApplicationException(seatPlanRes.Result?.ErrorInfo?.Message), seatPlanRes.Message);
            }
            var seatPlan = seatPlanRes.Result.Result;

            var updateSeatPlanRes = await seatPlanData.UpdateSeatPlanTemplateAsync(new Framework.ApiCommand.ApiData.SeatPlan.Request.UpdateSeatPlanTemplateArgs {
                Enabled = args.Enabled,
            }, seatPlan.Id);
            if(!updateSeatPlanRes.Succeeded || updateSeatPlanRes.Result is null || !updateSeatPlanRes.Result.IsSuccess)
            {
                return AppResult<ChangeSeatPlanStatusResult>.CreateFailed(
                    new ApplicationException(updateSeatPlanRes.Result?.ErrorInfo?.Message), updateSeatPlanRes.Message);
            }

            return AppResult<ChangeSeatPlanStatusResult>.CreateSucceeded(
                new ChangeSeatPlanStatusResult {Disable = args.Enabled}, "Seat plan status changed successfully");
        }
        catch (System.Exception ex)
        {
            return AppResult<ChangeSeatPlanStatusResult>.CreateFailed(ex, "An error occured in ChangeSeatPlanStatusHandler");
        }
    }
}