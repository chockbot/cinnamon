namespace Cinnamon.Api.Data.Repository.Entities;

public class OteSchedulePricingGroup : BaseEntity 
{
    public int OteScheduleId {get; set;}
    public decimal Price {get; set;}
    public int MaxSlots {get; set;}
    public string Description {get; set;}
    public bool IsAbsorbFees {get; set;}
    public string Name {get; set;}
    public int TicketSold {get; set;}
    public bool RequiredApproval {get; set;}
    public bool IsUnlimited { get; set; }
    public string ReserveSeatUuid {get; set;}
    public virtual OteSchedule OteSchedule {get; set;}
}