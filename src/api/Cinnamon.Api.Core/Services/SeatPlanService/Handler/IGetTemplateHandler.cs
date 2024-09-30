using Cinnamon.Api.Core.Services.SeatPlanService.Interactors;
using Cinnamon.Api.Core.Services.SeatPlanService.Interactors.Result;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.SeatPlanService.Handler;

public interface IGetTemplateHandler : IInteractorHandler<GetTemplateArgs,AppResult<GetTemplateResult>>
{}