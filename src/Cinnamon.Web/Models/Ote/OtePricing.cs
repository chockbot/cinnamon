namespace Cinnamon.Web.Models.Ote;

public class OtePricing 
{
    public int Id {get; set;}
    public int OteScheduleId { get; set; }
    public decimal Price { get; set; }
    public int MaxSlots { get; set; }
    public string Description { get; set; }
    public bool IsAbsorbFees { get; set; }
}