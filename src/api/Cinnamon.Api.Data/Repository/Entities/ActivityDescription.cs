namespace Cinnamon.Api.Data.Repository.Entities;

public class ActivityDescription : BaseEntity 
{
    public int ActivityId {get; set;}
    public string Description {get; set;} = string.Empty;
    public string SpecificsYouWillProvide {get; set;} = string.Empty;
    public string CustomerBringWithThem {get; set;} = string.Empty;
    public string? AdditionalRequirements {get; set;} = string.Empty;
    public string? ClassPolicies { get; set; } = string.Empty;
    public string? AdditionalData { get; set;} = string.Empty;  
    public string ActivityLevel {get; set;} = string.Empty;
    public string SkillLevel {get; set;} = string.Empty;
    public int MinimumAge {get; set;}
    public bool CanAdultsJoin {get; set;}

    public virtual Activity Activity {get; set;}
}