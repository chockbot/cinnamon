namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;

public class OteScheduleDTO
{
    public int ActivityId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string Recurrences { get; set; }
    public OteSchedulePricingDTO OteSchedulePricingDTO { get; set; }
    public IList<OteSchedulePricingDTO> OteSchedulePricingDTOs { get; set; }
}