using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.SeatPlanService.Interactors;

public class ChangeSeatPlanStatusArgs : IInteractor
{
    public int Id {get; set;}
    public bool Enabled {get; set;}
}