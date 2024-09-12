namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.OteWaitlist;

public class OteWaitlistDTO
{
    public int Id { get; set; }
    public int ProviderId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; }
    public int ActivityId { get; set; }
    public int ScheduleId { get; set; }
    public string Payload { get; set; }
    public int Status { get; set; }
    public string Type { get; set; }
    public int OteDateId {get; set;}
    public DateTime CreatedOn { get; set; }
    public DateTime ChangedOn { get; set; }
}
