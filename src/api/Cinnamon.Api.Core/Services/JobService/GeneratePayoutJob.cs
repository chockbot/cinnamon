using Cinnamon.Api.Core.Services.PaymentGatewayService.Handlers;
using Quartz;

namespace Cinnamon.Api.Core.Services.JobService;

public class GeneratePayoutJob : IJob
{
    private readonly IGeneratePayoutHandler generatePayoutHandler;

    public GeneratePayoutJob(IGeneratePayoutHandler generatePayoutHandler)
    {
        this.generatePayoutHandler = generatePayoutHandler;
    }

    public Task Execute(IJobExecutionContext context)
    {
        return generatePayoutHandler.ExecuteAsync(new PaymentGatewayService.Interactors.GeneratePayoutArgs {});
    }
}