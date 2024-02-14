using Cinnamon.Api.Core.Services.Disbursement.Handlers;
using Quartz;

namespace Cinnamon.Api.Core.Services.JobService;

public class GenerateDisbursementJob : IJob 
{
    private readonly IGenerateDisbursement generateDisbursement;

    public GenerateDisbursementJob(IGenerateDisbursement generateDisbursement)
    {
        this.generateDisbursement = generateDisbursement;
    }

    public Task Execute(IJobExecutionContext context)
    {
        return generateDisbursement.ExecuteAsync(new());
    }
}