namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;
public class OteOnlineEventsDTO
{
    public int Id { get; set; }
    public int OteScheduleId { get; set; }
    public string Videolink { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string TicketRestriction { get; set; }
    public int OteSchedulePricingGroupId { get; set; }
}
