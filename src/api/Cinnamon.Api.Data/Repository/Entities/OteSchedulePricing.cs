namespace Cinnamon.Api.Data.Repository.Entities;

public class OteSchedulePricing : BaseEntity 
{
    public int OteScheduleId {get; set;}
    public decimal Price {get; set;}
    public int MaxSlots {get; set;}
    public string Description {get; set;}
    public bool IsAbsorbFees {get; set;}
    public string Name {get; set;}
    public int TicketSold {get; set;}
    public int? OteDateId {get; set;}
    public int? OteSchedulePricingGroupId {get; set;}
    public bool RequiredApproval {get; set;}
    public bool IsUnlimited { get; set; }
    public virtual OteSchedule OteSchedule {get; set;}
    public virtual OteDate? OteDate  {get; set;}
    public virtual OteSchedulePricingGroup? OteSchedulePricingGroup {get; set;}
}