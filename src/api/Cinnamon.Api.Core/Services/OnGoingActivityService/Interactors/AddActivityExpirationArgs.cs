using Cinnamon.Framework.Interactor;

namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors;

public class AddActivityExpirationArgs : IInteractor
{
    public int Id { get; set; }
    public int ScheduleId { get; set; }
    public string SessionName { get; set; }
    public DateTime ExpirationStartDate { get; set; }
    public DateTime ExpirationEndDate { get; set; }
}
