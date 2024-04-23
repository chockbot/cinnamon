using Cinnamon.Api.Core.Modules.DataAccess.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.ActivityService;

public class BatchSummaryUpdateHandler : IBatchSummaryUpdateHandler
{
    private readonly IActivityData activityData;

    public BatchSummaryUpdateHandler(IActivityData activityData)
    {
        this.activityData = activityData;
    }

    public AppResult<BatchSummaryUpdateResult> Execute(BatchSummaryUpdateArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<BatchSummaryUpdateResult>> ExecuteAsync(BatchSummaryUpdateArgs args)
    {
        try
        {
            var updateRes = await activityData.BatchSummaryUpdate();
            if(!updateRes.Succeeded || updateRes.Result is null || !updateRes.Result.IsSuccess)
            {
                return AppResult<BatchSummaryUpdateResult>.CreateFailed(new ApplicationException(updateRes.Result?.ErrorInfo?.Message), updateRes.Message);
            }

            return AppResult<BatchSummaryUpdateResult>.CreateSucceeded(
                new BatchSummaryUpdateResult {Success = updateRes.Result.Result}, "Successfully update batch summary.");
        }
        catch (Exception ex)
        {
            return AppResult<BatchSummaryUpdateResult>.CreateFailed(ex, "An error occured in BatchSummaryUpdateHandler.");
        }
    }
}