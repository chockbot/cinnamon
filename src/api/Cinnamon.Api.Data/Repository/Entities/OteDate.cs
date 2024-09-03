namespace Cinnamon.Api.Data.Repository.Entities;

public class OteDate : BaseEntity
{
    public int OteScheduleId {get; set;}
    public DateTime Date {get; set;}
    public DateTime DateStart {get; set;}
    public DateTime DateEnd {get; set;}
    public string? SeatPlanPayload {get; set;}
    
    public virtual IList<OteSchedulePricing> OteSchedulePricing {get; set;}
    public virtual OteSchedule OteSchedule {get; set;}
}