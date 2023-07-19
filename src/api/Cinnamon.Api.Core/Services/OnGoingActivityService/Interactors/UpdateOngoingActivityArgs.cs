using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;
public class UpdateOngoingActivityArgs : IInteractor
{
    public int Id { get; set; }
    public int? CustomerId { get; set; }
    public int? ActivityId { get; set; }
    public int? ScheduleId { get; set; }
    public string? Name { get; set; }
    public string? StudentNo { get; set; }
    public int? NumberOfSessions { get; set; }
    public int? SessionsAttended { get; set; }
    public int? NumberOfBacktracking { get; set; }
    public string? Remarks { get; set; }
    public string? Status { get; set; }
    public DateTime? ExpirationStartDate { get; set; }
    public DateTime? ExpirationEndDate { get; set; }
    public bool? HasReview { get; set; }


}
