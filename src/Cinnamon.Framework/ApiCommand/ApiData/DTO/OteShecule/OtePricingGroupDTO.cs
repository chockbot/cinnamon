namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;

public class OtePricingGroupDTO
{
    public int Id {get; set;}
    public int OteScheduleId {get; set;}
    public decimal Price {get; set;}
    public int MaxSlots {get; set;}
    public string Description {get; set;}
    public bool IsAbsorbFees {get; set;}
    public string Name {get; set;}
    public int TicketSold {get; set;}
    public bool RequiredApproval {get; set;}
    public bool IsUnlimited { get; set; }
}