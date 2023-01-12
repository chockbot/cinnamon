using Cinnamon.Framework.ApiCommand.ApiData.DTO.ActivitySchedule;
using Cinnamon.Framework.ApiCommand.ApiData.DTO.ActivityImage;

namespace Cinnamon.Framework.ApiCommand.ApiData.DTO.Activity;

public class ActivityDTO
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
    public IEnumerable<string> SearchTags { get; set; }
    public string ExperienceType { get; set; }
    public bool IsPublished { get; set; }
    public int ExperienceTypeId {get; set;}
    public int ExperienceCategoryId {get; set;}
    public int SubCategoryId {get; set;}
    public int CreatedBy { get; set; }

    public IList<ActivityScheduleDTO> Schedules { get; set; }
    public IList<ActivityImageDTO> Images { get; set; }
}