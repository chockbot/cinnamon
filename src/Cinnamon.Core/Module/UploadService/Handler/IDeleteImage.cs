using Cinnamon.Core.Common;
using Cinnamon.Core.Interactor;
using Cinnamon.Core.Module.UploadService.Interactors;
using Cinnamon.Core.Module.UploadService.Interactors.Results;

namespace Cinnamon.Core.Module.UploadService.Handler;
public interface IDeleteImage : IInteractorHandler<DeleteImage, AppResult<DeleteImageResult>>
{
}

