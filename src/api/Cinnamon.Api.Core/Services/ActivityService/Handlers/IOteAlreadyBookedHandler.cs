using Cinnamon.Api.Core.Services.ActivityService.Interactors;
using Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Handlers;

public interface IOteAlreadyBookedHandler : IInteractorHandler<OteAlreadyBookedArgs,AppResult<OteAlreadyBookedResult>> 
{

}