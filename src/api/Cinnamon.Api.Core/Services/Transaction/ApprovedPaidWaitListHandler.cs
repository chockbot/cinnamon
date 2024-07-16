using Cinnamon.Api.Core.Services.TransactionService.Handlers;
using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;

namespace Cinnamon.Api.Core.Services.TransactionService;

public class ApprovedPaidWaitListHandler : IApprovedPaidWaitListHandler
{
    public AppResult<ApprovedPaidWaitListResult> Execute(ApprovedPaidWaitListArgs interactor)
    {
        throw new NotImplementedException();
    }

    public Task<AppResult<ApprovedPaidWaitListResult>> ExecuteAsync(ApprovedPaidWaitListArgs interactor)
    {
        throw new NotImplementedException();
    }
}