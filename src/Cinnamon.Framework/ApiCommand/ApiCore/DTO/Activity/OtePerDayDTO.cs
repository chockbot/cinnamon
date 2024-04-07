namespace Cinnamon.Framework.ApiCommand.ApiCore.DTO.Activity;

public class OtePerDayDTO
{
    public int ActivityId {get; set;}
    public int ExperienceTypeId {get; set;}
    public int DateId {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    public string Handler {get; set;}
    public string EventImage {get; set;}
    public string PinnedLocation {get; set;}
    public string CityName {get; set;}
    public string RegionName {get; set;}
    public bool ForceDisable {get; set;}
    public DateTime Date {get; set;}
    public DateTime DateStart {get; set;}
    public DateTime DateEnd {get; set;}
}
