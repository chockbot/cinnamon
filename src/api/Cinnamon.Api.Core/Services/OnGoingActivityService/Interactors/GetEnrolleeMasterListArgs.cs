using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
public class GetEnrolleeMasterListArgs : IInteractor
{
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int ProviderId { get; set; }
}
