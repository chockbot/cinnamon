using Cinnamon.Api.Core.Services.Disbursement.Handlers;
using Quartz;

namespace Cinnamon.Api.Core.Services.JobService;

public class GenerateDisbursementPayoutJob : IJob 
{
    private readonly IGenerateDisbursementPayout generateDisbursementPayout;

    public GenerateDisbursementPayoutJob(IGenerateDisbursementPayout generateDisbursementPayout)
    {
        this.generateDisbursementPayout = generateDisbursementPayout;
    }

    public Task Execute(IJobExecutionContext context)
    {
        return generateDisbursementPayout.ExecuteAsync(new Disbursement.Interactors.GenerateDisbursementPayoutArgs {});
    }
}