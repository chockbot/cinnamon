using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;

public class CreateReviewArgs : IInteractor
{
    public int CustomerId { get; set; }

    public int MakerId { get; set; }

    public int ActivityId { get; set; }

    public int ScheduleId { get; set; }

    public int StudentId { get; set; }

    public int Rating { get; set; }

    public string Review { get; set; }

    public DateTime ReviewDate { get; set; }
}
