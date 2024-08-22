using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.SeatPlanService.Interactors;

public class SeatPlanFormatterArgs : IInteractor
{
    public string FileLocation { get; set; }
    public string Handler { get; set; }
}