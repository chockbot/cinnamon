namespace Cinnamon.Api.Data.Repository.Entities;

public class OteSchedule : BaseEntity
{
    public int ActivityId {get; set;}
    public DateTime From {get; set;}
    public DateTime To {get; set;}
    public string Recurrences {get; set;}

    public virtual Activity Activity {get; set;}
    public virtual IList<OteSchedulePricing> OteSchedulePricing {get; set;}
}