using Cinnamon.Core.Common;
using Cinnamon.Core.Interactor;
using Cinnamon.Core.Module.PurchaseService.Interactors;
using Cinnamon.Core.Module.PurchaseService.Interactors.Results;

namespace Cinnamon.Core.Module.PurchaseService.Handler;

public interface ISubmitOrderHandler : IInteractorHandler<SubmitOrderArgs, AppResult<SubmitOrderResult>>
{}