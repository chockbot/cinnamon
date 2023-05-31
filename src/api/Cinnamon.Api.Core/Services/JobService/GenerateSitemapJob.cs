using Cinnamon.Api.Core.Services.SystemService.Handlers;
using Quartz;

namespace Cinnamon.Api.Core.Services.JobService;

public class GenerateSitemapJob : IJob
{
    private readonly IGenerateSitemapHandler generateSitemapHandler;

    public GenerateSitemapJob(IGenerateSitemapHandler generateSitemapHandler)
    {
        this.generateSitemapHandler = generateSitemapHandler;
    }

    public Task Execute(IJobExecutionContext context)
    {
        return generateSitemapHandler.ExecuteAsync(new SystemService.Interactors.GenerateSitemapArgs {});
    }
}