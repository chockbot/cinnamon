using System.ComponentModel;
namespace Cinnamon.Api.Data.Repository.Entities;

public class OteSchedule : BaseEntity
{
    public int ActivityId {get; set;}
    public DateTime From {get; set;}
    public DateTime To {get; set;}
    public string Recurrences {get; set;}
    public DateTime RecurrenceDateStart {get; set;}
    public DateTime RecurrenceDateEnd {get; set;}
    public int RepeatEvery {get; set;}
    public string SelectedDays {get; set;}
    public string ExtraOptions {get; set;}
    public int EventDurationCount {get; set;}
    public string EventDurationTimeUnit {get; set;}
    public int EventTicketLimit { get; set; }
    [DefaultValue(true)]
    public bool IsOpen { get; set; } = true;
    public bool IsCapacity { get; set; }
    public int CapacityCount { get; set; }
    public int EmailReminderDays {get; set;}
    public int EmailFeedbackDays {get; set;}

    public virtual Activity Activity {get; set;}
    public virtual IList<OteSchedulePricing> OteSchedulePricing {get; set;}
    public virtual IList<OteDate> OteDates {get; set;}
    public virtual IList<OteSchedulePricingGroup> OteSchedulePricingGroups {get; set;}
    public virtual IList<OteOnlineEvent> OteOnlineEvent { get; set; }
}