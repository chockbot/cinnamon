using Cinnamon.Api.Core.Modules.EmailDriver.Interactors;
using Cinnamon.Api.Core.Modules.EmailDriver.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Modules.EmailDriver.Handlers;

public interface ISendMailHandler : IInteractorHandler<SendMailArgs, AppResult<SendMailResult>>
{
}