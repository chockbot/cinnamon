namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.PayoutAccount;

public class OteScheduleDTO
{
    public int ActivityId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string Recurrences { get; set; }
}