using Cinnamon.Api.Core.Services.SeatPlanService.Interactors;
using Cinnamon.Api.Core.Services.SeatPlanService.Interactors.Result;
using Cinnamon.Framework.Common;
using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.SeatPlanService.Handler;

public interface IChangeSeatPlanStatusHandler : IInteractorHandler<ChangeSeatPlanStatusArgs,AppResult<ChangeSeatPlanStatusResult>>
{}