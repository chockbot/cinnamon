using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
public class GetReviewsByActivityIdArgs : IInteractor
{
    public int ActivityId { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
}
