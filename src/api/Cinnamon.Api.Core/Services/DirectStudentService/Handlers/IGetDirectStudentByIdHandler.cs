using Cinnamon.Api.Core.Services.DirectStudentService.Interactors;
using Cinnamon.Api.Core.Services.DirectStudentService.Interactors.Result;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;
namespace Cinnamon.Api.Core.Services.DirectStudentService.Handlers;
public interface IGetDirectStudentByIdHandler : IInteractorHandler<GetDirectStudentByIdArgs,AppResult<GetDirectStudentByIdResult>>
{
}
