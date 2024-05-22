using Cinnamon.Api.Core.Services.TransactionService.Interactors;
using Cinnamon.Api.Core.Services.TransactionService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.TransactionService.Handlers;
public interface IGetDirectStudentSalesHandler : IInteractorHandler<GetDirectStudentSalesArgs, AppResult<GetDirectStudentSalesResult>>
{
}
