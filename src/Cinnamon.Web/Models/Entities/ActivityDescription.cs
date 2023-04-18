namespace Cinnamon.Web.Models.Entities;

public class ActivityDescription
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public string Description { get; set; }
    public string SpecificsYouWillProvide { get; set; }
    public string CustomerBringWithThem { get; set; }
    public string? AdditionalRequirements { get; set; }
    public string ActivityLevel { get; set; }
    public string SkillLevel { get; set; }
    public int MinimumAge { get; set; }
    public bool CanAdultsJoin { get; set; }
}
