using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Quartz;

namespace Cinnamon.Api.Core.Services.JobService;

public class UpdateActivitySummaryJob : IJob
{
    private readonly IBatchSummaryUpdateHandler batchSummaryUpdateHandler;

    public UpdateActivitySummaryJob(IBatchSummaryUpdateHandler batchSummaryUpdateHandler)
    {
        this.batchSummaryUpdateHandler = batchSummaryUpdateHandler;
    }

    public Task Execute(IJobExecutionContext context)
    {
        return batchSummaryUpdateHandler.ExecuteAsync(new ActivityService.Interactors.BatchSummaryUpdateArgs {});
    }
}