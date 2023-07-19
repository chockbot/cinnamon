using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results;
using Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Handlers;

public interface IGetReviewsByMakerIdHandler : IInteractorHandler<GetReviewsByMakerIdArgs, AppResult<GetReviewsByMakerIdResult>>
{
}
