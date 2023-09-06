using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Modules.DataAccess.Student;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Api.Core.Services.DashboardService.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService;

public class AddActivityExpirationHandler: IAddActivityExpirationHandler
{
    private readonly IGetActivitySchedulesHandler getActivitySchedulesHandler;
    private readonly IUpdateOngoingActivityHadler updateOngoingActivityHadler;

    public AddActivityExpirationHandler(IGetActivitySchedulesHandler getActivitySchedulesHandler, IUpdateOngoingActivityHadler updateOngoingActivityHadler)
    {
        this.getActivitySchedulesHandler = getActivitySchedulesHandler;
        this.updateOngoingActivityHadler = updateOngoingActivityHadler;
    }

    public AppResult<AddActivityExpirationResult> Execute(AddActivityExpirationArgs args)
    {
        try
        {
            return ExecuteAsync(args).Result;
        }
        catch (Exception ex)
        {
            return AppResult<AddActivityExpirationResult>.CreateFailed(ex, "An error occured in AddActivityExpirationHandler");
        }
    }

    public async Task<AppResult<AddActivityExpirationResult>> ExecuteAsync(AddActivityExpirationArgs args)
    {
        try
        {
            switch (args.SessionName)
            {
                case "2 Weeks":
                    args.ExpirationEndDate = args.ExpirationStartDate.AddDays(15);
                    break;
                case "3 Weeks":
                    args.ExpirationEndDate = args.ExpirationStartDate.AddDays(22);
                    break;
                case "1 Month":
                    args.ExpirationEndDate = args.ExpirationStartDate.AddMonths(1).AddDays(1);
                    break;
                case "2 Months":
                    args.ExpirationEndDate = args.ExpirationStartDate.AddMonths(2).AddDays(1);
                    break;
                case "3 Months":
                    args.ExpirationEndDate = args.ExpirationStartDate.AddMonths(3).AddDays(1);
                    break;
                default:
                    break;
            }
            var updated = await updateOngoingActivityHadler.ExecuteAsync(new UpdateOngoingActivityArgs
            {
                Id = args.Id,
                ExpirationStartDate = args.ExpirationStartDate,
                ExpirationEndDate = args.ExpirationEndDate
            });
            if (!updated.Succeeded || updated.Result == null)
            {
                return AppResult<AddActivityExpirationResult>.CreateFailed(new ApplicationException(updated.Message), updated.Message);
            }
            return AppResult<AddActivityExpirationResult>.CreateSucceeded(new AddActivityExpirationResult
            {
                Id = updated.Result.Id,
                ScheduleId = updated.Result.ScheduleId,
                ExpirationStartDate = updated.Result.ExpirationStartDate,
                ExpirationEndDate = updated.Result.ExpirationEndDate
            }, "Successfully update student ongoing activity");
        }
        catch (Exception ex)
        {
            return AppResult<AddActivityExpirationResult>.CreateFailed(ex, "An error occured in AddActivityExpirationHandler");
        }
    }
}
