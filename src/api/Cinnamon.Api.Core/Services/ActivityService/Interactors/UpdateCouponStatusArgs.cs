using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class UpdateCouponStatusArgs : IInteractor 
{
    public int Id {get; set;}
    public int Status {get; set;}
}