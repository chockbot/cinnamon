namespace Cinnamon.Web.Models.Entities;

public class Activity 
{
    public int ActivityId {get; set;}
    public int ExperienceTypeId {get; set;}
    public int ExperienceCategoryId {get; set;}
    public int SubCategoryId {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    public string Price {get; set;}
    public string ScheduleIndicator {get; set;} = " ";
    public string Remarks {get; set;} = " ";
    public bool IsPublished {get; set;}
    public string Address1 {get; set;} = " ";
    public string Address2 {get; set;} = " ";
    public string District {get; set;} = " ";
    public string City {get; set;} = " ";
    public string SpecificsYouWillProvide {get; set;}
    public string CustomerBringWithThem {get; set;}
    public string? AdditionalRequirements {get; set;}
    public string ActivityLevel {get; set;}
    public string SkillLevel {get; set;}
    public int MinimumAge {get; set;}
    public bool CanAdultsJoin {get; set;}
    public int CreatedBy { get; set; }
    public IList<string> SearchTags {get; set;} = new List<string>();
    public IList<ActivitySchedule> ActivitySchedules {get; set;} = new List<ActivitySchedule>();
    public IList<ActivityImage> Images {get; set;}
}