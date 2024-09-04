namespace Cinnamon.Web.Models.Ote;

public class OteDate 
{
    public int Id {get; set;}
    public int OteScheduleId {get; set;}
    public DateTime Date {get; set;}
    public DateTime DateStart {get; set;}
    public DateTime DateEnd {get; set;}
    public string? SeatPlanPayload { get; set; }
}