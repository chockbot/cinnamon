namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class GetOteWaitlistByProviderResult
{
    public IEnumerable<OteWaitlist> OteWaitlists { get; set; }
    public class OteWaitlist
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ActivityId { get; set; }
        public int ScheduleId { get; set; }
        public string Payload { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ChangedOn { get; set; }
    }
}
