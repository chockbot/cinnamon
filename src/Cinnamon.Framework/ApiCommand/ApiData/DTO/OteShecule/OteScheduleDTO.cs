namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.OteSchedule;

public class OteScheduleDTO
{
    public int ActivityId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string Recurrences { get; set; }
    public DateTime RecurrenceDateStart {get; set;}
    public DateTime RecurrenceDateEnd {get; set;}
    public int RepeatEvery {get; set;}
    public string SelectedDays {get; set;}
    public string ExtraOptions {get; set;}
    public int EventDurationCount {get; set;}
    public int EventTicketLimit { get; set; }
    public string EventDurationTimeUnit {get; set;}
    public bool IsOpen { get; set; }
    public bool IsCapacity { get; set; }
    public int CapacityCount { get; set; }

    public OteSchedulePricingDTO OteSchedulePricingDTO { get; set; }
    public OteOnlineEventsDTO OteOnlineEventsDTO { get; set; }
    public IList<OteSchedulePricingDTO> OteSchedulePricingDTOs { get; set; }
    public virtual IList<OtePricingGroupDTO> OteSchedulePricingGroups {get; set;}
    //public virtual IList<OteOnlineEventsDTO> OteOnlineEvent { get; set; }
}