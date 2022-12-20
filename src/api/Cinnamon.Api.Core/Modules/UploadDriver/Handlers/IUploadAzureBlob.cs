using Cinnamon.Api.Core.Modules.UploadDriver.Interactors;
using Cinnamon.Api.Core.Modules.UploadDriver.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.UploadDriver.Handlers;

public interface IUploadAzureBlob : IInteractorHandler<AzureUploadArgs,AppResult<AzureUploadResult>>
{
}