namespace Cinnamon.Api.Core.Services.OnGoingActivityService.Interactors.Results; 
public class AddActivityExpirationResult
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int ActivityId { get; set; }
    public int ScheduleId { get; set; }
    public string Name { get; set; }
    public int NumberOfSessions { get; set; }
    public int SessionsAttended { get; set; }
    public DateTime ExpirationStartDate { get; set; }
    public DateTime ExpirationEndDate { get; set; }
}
