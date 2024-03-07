namespace Cinnamon.Api.Data.Repository.Entities;

public class OteOnlineEvent: BaseEntity
{
    public int OteScheduleId { get; set; }
    public string Videolink { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string TicketRestriction { get; set; }
    public int OteSchedulePricingGroupId { get; set; }
    public virtual OteSchedule OteSchedule { get; set; }
}
