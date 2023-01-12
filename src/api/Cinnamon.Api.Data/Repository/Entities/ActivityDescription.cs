namespace Cinnamon.Api.Data.Repository.Entities;

public class ActivityDescription : BaseEntity 
{
    public int ActivityId {get; set;}
    public string Description {get; set;}
    public string SpecificsYouWillProvide {get; set;}
    public string CustomerBringWithThem {get; set;}
    public string? AdditionalRequirements {get; set;}
    public string ActivityLevel {get; set;}
    public string SkillLevel {get; set;}
    public int MinimumAge {get; set;}
    public bool CanAdultsJoin {get; set;}

    public virtual Activity Activity {get; set;}
}