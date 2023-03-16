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
    public string? ScheduleIndicator {get; set;}
    public string? Remarks {get; set;}
    [Required]
    public bool IsPublished {get; set;}
    public string? Address1 {get; set;}
    public string? Address2 {get; set;}
    public string? District {get; set;}
    public string? City {get; set;}
    public string? Subdivision { get; set; }
    public string? Region { get; set; }
    public string? Barangay { get; set; }
    public string? PostalCode { get; set; }
    public string? PinnedLocation { get; set; }
    public string? SpecificsYouWillProvide {get; set;}
    public string? CustomerBringWithThem {get; set;}
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
    public bool IsSetSession { get; set; }
    public string? SessionName { get; set; }
    [Required]
    public IEnumerable<string> SearchTags {get; set;}
    [Required]
    public IEnumerable<Schedule> ActivitySchedules {get; set;}
    

    public class Schedule 
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
        [Required]
        public int Order {get; set;}
        [Required]
        public bool IsActiveSchedule { get; set; }
    }
}