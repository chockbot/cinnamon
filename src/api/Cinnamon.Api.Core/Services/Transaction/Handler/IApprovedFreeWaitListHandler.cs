using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.TransactionService.Handlers;

public interface IApprovedFreeWaitListHandler : IInteractorHandler<ApprovedFreeWaitListArgs,AppResult<ApprovedFreeWaitListResult>> 
{
}