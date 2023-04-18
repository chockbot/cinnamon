using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;

public interface IGetOngoingActivityByIdHandler: IInteractorHandler<GetOngoingActivityByIdArgs, AppResult<GetOngoingActivityByIdResult>>
{

}
