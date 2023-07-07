using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class UpdateActivity
{
    public int? ExperienceTypeId { get; set; }
    [Required]
    public int ActivityId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Price { get; set; }
    public string? ScheduleIndicator { get; set; } = string.Empty;
    public string? Remarks { get; set; } = string.Empty;
    public bool? IsPublished { get; set; }
    public string? Address1 { get; set; } = string.Empty;
    public string? Address2 { get; set; } = string.Empty;
    public string? District { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public string? Subdivision { get; set; } = string.Empty;
    public string? Region { get; set; } = string.Empty;
    public string? Barangay { get; set; } = string.Empty;
    public string? PostalCode { get; set; } = string.Empty;
    public string? SpecificsYouWillProvide { get; set; }
    public string? CustomerBringWithThem { get; set; }
    public string? AdditionalRequirements { get; set; }
    public string? ActivityLevel { get; set; } = string.Empty;
    public string? SkillLevel { get; set; } = string.Empty;
    public int? MinimumAge { get; set; }
    public bool? CanAdultsJoin { get; set; }
    public string? Searchtag1 { get; set; }
    public string? Searhtag2 { get; set; }
    public string? Searhtag3 { get; set; }
    public string? Searchtag4 { get; set; }
    public string? Searchtag5 { get; set; }
    public int? ExperienceCategoryId {get; set;}
    public int? SubCategoryId {get; set;}
    public string? PinnedLocation { get; set; }
    public bool? IsDeactivated { get; set; }
    public Enums.Enums.ActivityStatus? Status { get; set; }
    public string? Handler {get; set;}
}