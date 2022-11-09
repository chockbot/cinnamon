using Cinnamon.Core.Common;
using Cinnamon.Core.Interactor;
using Cinnamon.Core.Module.ActivityService.Interactors;
using Cinnamon.Core.Module.ActivityService.Interactors.Results;

namespace Cinnamon.Core.Module.ActivityService.Handler;

public interface IUpdateActivityHandler : IInteractorHandler<UpdateActivity, AppResult<UpdateActivityResult>> 
{
}