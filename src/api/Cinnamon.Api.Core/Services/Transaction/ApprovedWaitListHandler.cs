using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class ApprovedWaitListHandler : IApprovedWaitListHandler
{
    public AppResult<ApprovedWaitListResult> Execute(ApprovedWaitListArgs args)
    {
        return ExecuteAsync(args).Result;
    }

    public async Task<AppResult<ApprovedWaitListResult>> ExecuteAsync(ApprovedWaitListArgs args)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (Exception ex)
        {
            return AppResult<ApprovedWaitListResult>.CreateFailed(ex, "An error occured in ApprovedWaitListHandler.");
        }
    }
}