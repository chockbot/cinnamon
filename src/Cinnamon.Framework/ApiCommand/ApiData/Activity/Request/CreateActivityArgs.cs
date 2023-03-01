using System.ComponentModel.DataAnnotations;

namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class CreateActivityArgs
{
    [Required]
    public int ExperienceTypeId { get; set; }
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string Price { get; set; }
    public string ScheduleIndicator { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
    [Required]
    public bool IsPublished { get; set; }
    public string Address1 { get; set; } = string.Empty;
    public string Address2 { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Subdivision { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Barangay { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;

    public string? SpecificsYouWillProvide { get; set; }
    public string? CustomerBringWithThem { get; set; }
    public string? AdditionalRequirements { get; set; }
    public string ActivityLevel { get; set; } = string.Empty;
    public string SkillLevel { get; set; } = string.Empty;
    public int MinimumAge { get; set; }
    public bool CanAdultsJoin { get; set; }
    public string? Searchtag1 { get; set; }
    public string? Searhtag2 { get; set; }
    public string? Searhtag3 { get; set; }
    public string? Searchtag4 { get; set; }
    public string? Searchtag5 { get; set; }
    [Required]
    public int ExperienceCategoryId {get; set;}
    [Required]
    public int SubCategoryId {get; set;}
    [Required]
    public string Handler {get; set;}
    public bool IsSetSession { get; set; }
    public string SessionName { get; set; }
}