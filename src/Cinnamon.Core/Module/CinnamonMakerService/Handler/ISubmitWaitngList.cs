using Cinnamon.Core.Common;
using Cinnamon.Core.Interactor;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors;
using Cinnamon.Core.Module.CinnamonMakerService.Interactors.Results;

namespace Cinnamon.Core.Module.CinnamonMakerService.Handler;

public interface ISubmitWaitngList : IInteractorHandler<SubmitWaitingList, AppResult<SubmitWaitingListResult>>
{
}