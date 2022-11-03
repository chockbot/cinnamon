using System;
using Cinnamon.Core.Common;
using Cinnamon.Core.Interactor;
using Cinnamon.Core.Module.EmailService.Interactors;
using Cinnamon.Core.Module.EmailService.Interactors.Results;

namespace Cinnamon.Core.Module.EmailService.Handler;

public interface ISendMailHandler : IInteractorHandler<SendMail, AppResult<SendMailResults>>
{
}