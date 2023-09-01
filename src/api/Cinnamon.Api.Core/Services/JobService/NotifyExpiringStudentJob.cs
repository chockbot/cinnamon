using Cinnamon.Api.Core.Services.AccountService.Handlers;
using Quartz;

namespace Cinnamon.Api.Core.Services.JobService;

public class NotifyExpiringStudentJob : IJob
{
    private readonly IExpiringStudentsHandler expiringStudentsHandler;
    
    public NotifyExpiringStudentJob(IExpiringStudentsHandler expiringStudentsHandler)
    {
        this.expiringStudentsHandler = expiringStudentsHandler;   
    }

    public Task Execute(IJobExecutionContext context)
    {
        return expiringStudentsHandler.ExecuteAsync(new AccountService.Interactors.ExpiringStudentsArgs {});
    }
}