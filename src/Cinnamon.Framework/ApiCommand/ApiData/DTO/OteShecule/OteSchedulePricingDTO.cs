namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;

public class OteSchedulePricingDTO
{
    public int Id {get; set;}
    public int OteScheduleId { get; set; }
    public decimal Price { get; set; }
    public int MaxSlots { get; set; }
    public string Description { get; set; }
    public string Name {get; set;}
    public bool IsAbsorbFees { get; set; }
    public int Sold { get; set; }
    public int Available { get; set; }
    public int OteDateId {get; set;}
    public int TicketSold { get; set; }
    public int OteSchedulePricingsId { get; set; }
    public int OteSchedulePricingGroupId { get; set; }
}