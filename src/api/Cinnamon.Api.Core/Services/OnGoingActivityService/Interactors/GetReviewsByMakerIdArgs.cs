using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
public class GetReviewsByMakerIdArgs : IInteractor
{
    public int MakerId { get; set; }

    public int? PageIndex { get; set; }

    public int? CountPerPage { get; set; }
}
