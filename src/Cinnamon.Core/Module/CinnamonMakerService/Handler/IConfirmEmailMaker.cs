using Cinnamon.Core.Common;
using Cinnamon.Core.Interactor;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors.Results;

namespace Cinnamon.Core.Module.CinnamonMakerService.Handler;

public interface IConfirmEmailMaker : IInteractorHandler<ConfirmEmail, AppResult<ConfirmEmailResult>> 
{

}