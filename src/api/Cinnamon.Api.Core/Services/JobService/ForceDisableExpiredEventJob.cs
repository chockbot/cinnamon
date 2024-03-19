using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Quartz;

namespace Cinnamon.Api.Core.Services.JobService;

public class ForceDisableExpiredEventJob : IJob 
{
    private readonly IDisabledExpiredEventHandler disabledExpiredEventHandler;

    public ForceDisableExpiredEventJob(IDisabledExpiredEventHandler disabledExpiredEventHandler)
    {
        this.disabledExpiredEventHandler = disabledExpiredEventHandler;
    }

    public Task Execute(IJobExecutionContext context)
    {
        return disabledExpiredEventHandler.ExecuteAsync(new());
    }
}