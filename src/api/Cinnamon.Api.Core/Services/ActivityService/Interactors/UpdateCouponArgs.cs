using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class UpdateCouponArgs : IInteractor 
{
    public int Id {get; set;}
    public string Name {get; set;}
    public DateTime From {get; set;}
    public DateTime To {get; set;}
}