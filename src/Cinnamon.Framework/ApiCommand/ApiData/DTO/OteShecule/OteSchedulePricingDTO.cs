namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.PayoutAccount;

public class OteSchedulePricingDTO
{
    public int OteScheduleId { get; set; }
    public decimal Price { get; set; }
    public int MaxSlots { get; set; }
    public string Description { get; set; }
    public bool IsAbsorbFees { get; set; }

    public IList<OteSchedulePricingDTO> OteSchedulePricingDTOs { get; set; }
}