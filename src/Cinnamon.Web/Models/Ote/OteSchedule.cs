namespace Cinnamon.Web.Models.Ote;
public class OteSchedule
{
    public int ActivityId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string Recurrences { get; set; }
    public bool IsOpen { get; set; }
    public bool IsCapacity { get; set; }
    public int CapacityCount { get; set; }
    public int EventTicketLimit { get; set; }
    public bool ReserveSeat { get; set; }
    public int SeatPlanTemplateId { get; set; }
    public OtePricing OtePricing { get; set; }
}
