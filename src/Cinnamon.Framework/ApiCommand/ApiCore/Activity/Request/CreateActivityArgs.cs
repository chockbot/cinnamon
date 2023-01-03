using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class CreateActivityArgs 
{
    [Required]
    public int ExperienceTypeId {get; set;}
    [Required]
    public int ExperienceCategoryId {get; set;}
    [Required]
    public int SubCategoryId {get; set;}
    [Required]
    public string Title {get; set;}
    [Required]
    public string Description {get; set;}
    [Required]
    public string Price {get; set;}
    [Required]
    public string ScheduleIndicator {get; set;}
    [Required]
    public string Remarks {get; set;}
    [Required]
    public bool IsPublished {get; set;}
    [Required]
    public string Address1 {get; set;}
    [Required]
    public string Address2 {get; set;}
    [Required]
    public string District {get; set;}
    [Required]
    public string City {get; set;}
    [Required]
    public string SpecificsYouWillProvide {get; set;}
    [Required]
    public string CustomerBringWithThem {get; set;}
    public string? AdditionalRequirements {get; set;}
    [Required]
    public string ActivityLevel {get; set;}
    [Required]
    public string SkillLevel {get; set;}
    [Required]
    public int MinimumAge {get; set;}
    [Required]
    public bool CanAdultsJoin {get; set;}
    [Required]
    public IEnumerable<string> SearchTags {get; set;}
    [Required]
    public IEnumerable<ActivitySchedule> ActivitySchedules {get; set;}
    

    public class ActivitySchedule 
    {
        [Required]
        public string Name {get; set;}
        [Required]
        public string DateTime {get; set;}
        [Required]
        public decimal Price {get; set;}
        [Required]
        public string UnitPrice {get; set;}
        [Required]
        public int PerUnit1 {get; set;}
        [Required]
        public string PriceUnit1 {get; set;}
        [Required]
        public int PerUnit2 {get; set;}
        [Required]
        public string PriceUnit2 {get; set;}
    }
}