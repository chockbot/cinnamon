using Cinnamon.Api.Core.Services.DirectStudentService.Interactors;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Results;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.DirectStudentService.Handlers;

public interface IDirectStudentHandler : IInteractorHandler<DirectStudentArgs, AppResult<DirectStudentResult>>
{
}
