namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Activity;

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
    public string EventDurationTimeUnit {get; set;}
    public int EventTicketLimit { get; set; }
    public bool IsOpen { get; set; }
    public bool IsCapacity { get; set; }
    public int CapacityCount { get; set; }
    public int EmailFeedbackDays {get; set;}
    public int EmailReminderDays {get; set;}

    public OtePricingDTO OtePricingDTO { get; set; }
    public IList<OtePricingGroupDTO> OteSchedulePricingGroups {get; set;}
}
