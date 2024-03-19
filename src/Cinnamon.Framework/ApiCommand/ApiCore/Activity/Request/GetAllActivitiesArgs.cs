using Cinnamon.Framework.Interactor;
using Cinnamon.Framework.Enums;
namespace Cinnamon.Framework.ApiCommand.ApiCore.Activity.Request;

public class GetAllActivitiesArgs
{
    public bool? IncludeAtivitySchedules { get; set; }
    public bool? IncludeActivityAddress { get; set; }
    public bool? IncludeActivityDescription { get; set; }
    public bool? IncludeActivitySearchTags { get; set; }
    public bool? IncludeActivityImages { get; set; }
    public bool? IncludeExperienceTypes { get; set; }
    public bool? IncludeExperienceCategories { get; set; }
    public bool? IncludeSubCategories { get; set; }
    public bool? IsActive { get; set; }
    public bool? IncludeCustomer {get; set;}
    public int? PageIndex { get; set; }
    public int? CountPerPage { get; set; }
    public int? ExperienceCategoryId { get; set; }
    public string? SearchValue { get; set; }
    public bool? IncludeStudents { get; set; }
    public bool? IsDeactivated { get; set; }
    public Enums.Enums.ActivityStatus? Status { get; set; }
    public bool? IsAdmin { get; set; }
    public bool? IncludeReviews { get; set; }
    public bool? IncludeTickets { get; set; }
    public bool? ForceDisable {get; set;}
}
