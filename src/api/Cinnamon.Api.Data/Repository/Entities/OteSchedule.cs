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

    public virtual Activity Activity {get; set;}
    public virtual IList<OteSchedulePricing> OteSchedulePricing {get; set;}
}