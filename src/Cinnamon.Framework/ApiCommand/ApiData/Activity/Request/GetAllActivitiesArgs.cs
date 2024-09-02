namespace Cinnamon.Framework.ApiCommand.ApiData.Activity.Request;

public class GetAllActivities
{
    public int? CustomerId {get; set;}
    public bool? IsActive { get; set; }
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public bool? IncludeAddress {get; set;}
    public bool? IncludeDescription {get; set;}
    public bool? IncludeSearchTags {get; set;}
    public bool? IncludeSchedules {get; set;}
    public bool? IncludeImages {get; set;}
    public bool? IncludeExperienceTypes { get; set; }
    public bool? IncludeExperienceCategories { get; set; }
    public bool? IncludeSubCategories { get; set; }
    public string? Ids {get; set;}
    public string? LikeHandler {get; set;}
    public bool? IncludeCustomer {get; set;}
    public int? ExperienceCategoryId { get; set; }
    public string? SearchValue { get; set; }
    public bool? IncludeStudents { get; set; }
    public bool? IncludeReviews { get; set; }
    public bool? IncludeTickets { get; set; }
    public bool? IsDeactivated { get; set; }
    public Enums.Enums.ActivityStatus? Status { get; set; }
    public bool? IsAdmin { get; set; }
    public bool? ForceDisable {get; set;}
    public bool? IncludeOteSchedule { get; set;}
}