using Cinnamon.Api.Core.Services.OngoingActivityService.Interactors;
using Cinnamon.Api.Core.Services.OngoingActivityService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.OngoingActivityService.Handlers;

public interface ICreateOngoingActivityHandler : IInteractorHandler<CreateOngoingActivityArgs,AppResult<CreateOngoingActivityResult>>
{}