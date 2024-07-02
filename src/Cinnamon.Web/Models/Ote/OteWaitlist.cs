namespace Cinnamon.Web.Models.Ote;
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
    public string TicketName { get; set; }
    public decimal TicketPrice { get; set; }
    public string CustomerProfile { get; set; }
    public string CustomerEmail { get; set; }
    public DateTime EventDate { get; set; }
}
