using Cinnamon.Api.Core.Services.ActivityService.Handlers;
using Cinnamon.Api.Core.Services.PaymentGatewayService.Handlers;
using Quartz;

namespace Cinnamon.Api.Core.Services.JobService;

public class UpdateActivityGuidJob : IJob
{
    private readonly IUpdateActivityGuidHandler updateActivityGuidHandler;

    public UpdateActivityGuidJob(IUpdateActivityGuidHandler updateActivityGuidHandler)
    {
        this.updateActivityGuidHandler = updateActivityGuidHandler;
    }

    public Task Execute(IJobExecutionContext context)
    {
        return updateActivityGuidHandler.ExecuteAsync(new ActivityService.Interactors.UpdateActivityArgs { });
    }
}