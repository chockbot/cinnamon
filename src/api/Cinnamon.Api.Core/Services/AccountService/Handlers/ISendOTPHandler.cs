using Cinnamon.Api.Core.Services.AccountService.Interactors;
using Cinnamon.Api.Core.Services.AccountService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.AccountService.Handlers;
public interface ISendOTPHandler : IInteractorHandler<SendOTPArgs,AppResult<SendOTPResult>>
{
}
