using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.SeatPlanService.Interactors;

public class CreateSeatPlanTemplateArgs : IInteractor
{
    public IFormFile JsonFile {get; set;}
    public IFormFile ImageFile {get; set;}
    public string Name {get; set;}
    public string Address {get; set;}
    public int FormatterId {get; set;}
}