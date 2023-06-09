using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
public class GetReviewsByMakerIdArgs : IInteractor
{
    public int MakerId { get; set; }
}
