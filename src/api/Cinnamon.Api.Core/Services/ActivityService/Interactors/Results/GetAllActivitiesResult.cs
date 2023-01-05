using System.Diagnostics;

namespace Cinnamon.Api.Core.Services.ActivityService.Interactors.Results;

public class GetAllActivitiesResult
{
    public IEnumerable<Activity> Activities { get; set; }
    public class Activity
    {
        public int Id { get; set; }
        public string SubTitle { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SpecificsYouWillProvide { get; set; }
        public string CustomerBringWithThem { get; set; }
        public string? AdditionalRequirements { get; set; }
        public string ActivityLevel { get; set; }
        public string SkillLevel { get; set; }
        public int MinimumAge { get; set; }
        public bool CanAdultsJoin { get; set; }
        public string Price { get; set; }
        public string Remarks { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string[] SearchTags { get; set; }
        public string ExperienceType { get; set; }
        public bool IsPublished { get; set; }
        public int ExperienceCategoryId { get; set; }
        public int SubCategoryId { get; set; }
    }
}
