namespace Cinnamon.Api.Data.Repository.Entities;
public class OteWaitlist : BaseEntity
{
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public int ActivityId { get; set; }
    public int ScheduleId { get; set; }
    public string Payload { get; set; }
    public int Status { get; set; }
}
