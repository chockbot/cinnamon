using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;

public class GetEnrolledStudentsArgs : IInteractor
{
    public int ActivityId { get; set; }
}
