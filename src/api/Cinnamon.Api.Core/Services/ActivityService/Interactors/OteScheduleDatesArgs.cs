using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors;

public class OteScheduleDatesArgs : IInteractor 
{
    public int ActivityId {get; set;}
    public DateTime? DateFrom {get; set;}
    public DateTime? DateTo {get; set;}
}