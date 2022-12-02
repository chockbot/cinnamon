using System.Collections.Generic;

namespace Cinnamon.Api.Data.Repository.Entities;

public class Activity : BaseEntity
{
    public string Title {get; set;}
    public string Subtitle {get; set;}
    public string Description {get; set;}
    public string ScheduleIndicator {get; set;}
    public string MapDetails {get; set;}
    public string Guarantee {get; set;}
    public string Remarks {get; set;}

    public virtual ActivityAddress Address {get; set;}
    public virtual ActivityDescription Description {get; set;}
    public virtual ActivitySearchTag SearchTag {get; set;}
    public virtual IList<ActivitySchedule> Schedules {get; set;}
    public virtual IList<ActivityImage> Images {get; set;}
}